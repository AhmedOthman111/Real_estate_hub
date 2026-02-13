using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.CQRS.CommentReply.DTO;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class CommentReplyService : ICommentReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICacheService _cacheService;
        public CommentReplyService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cacheService = cacheService;
        }


        public async Task<CommentResponseDto> AddCommentAsync(CreateCommentDto dto, string customerid)
        {
            var ad = await _unitOfWork.Ad.GetByIdAsync(dto.AdId);
            var customer = await _userManager.FindByIdAsync(customerid);

            if (ad.Status != Domain.Enums.AdStatus.Active) throw new BusinessException("Ad is not active to add a comment");
          
            var comment = new Comment()
            {
                Message = dto.Content,
                CustomerID = customer.Id,
                AdId = dto.AdId,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.CommentRepo.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            await _cacheService.RemoveAsync($"ad:{ad.Id}");

            return new CommentResponseDto
            {
                Id = comment.Id,
                AdId = comment.AdId,
                Content = comment.Message,
                CreatedAt = DateTime.Now,
                AuthorName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "Unknown",
                Reply = null
            };

        }

        public async Task<ReplyResponseDto> ReplyToCommentAsync(ReplyCommentDto dto, string AppUserownerId)
        {
            var comment = await _unitOfWork.CommentRepo.GetByIdAsync(dto.CommentId);

            var ad = await _unitOfWork.Ad.GetByIdAsync(comment.AdId);

            var owner = await _unitOfWork.Owner.GetByAppUserIdAsync(AppUserownerId);

            var AppuserOwner = await _userManager.FindByIdAsync(AppUserownerId);


            if (ad.OwnerId != owner.Id) throw new BusinessException("Only the ad owner can reply to this comment.");

            var reply = new Reply()
            {
                Message = dto.Content,
                CreatedAt = DateTime.Now,
                CommentId = comment.Id,
                OwnerId = owner.Id
            };

            await _unitOfWork.ReplyRepo.AddAsync(reply);
            await _unitOfWork.SaveChangesAsync();

            await _cacheService.RemoveAsync($"ad:{ad.Id}");
            await _cacheService.RemoveAsync($"adcomments:{ad.Id}");


            return new ReplyResponseDto()
            {
                Id = reply.Id,
                Content = reply.Message,
                CreatedAt = reply.CreatedAt,
                AuthorName = AppuserOwner != null ? $"{AppuserOwner.FirstName} {AppuserOwner.LastName}" : "Ad Owner"
            };



        }

        public async Task DeleteCommentAsync(int id, string customerId)
        {
            var comment = await _unitOfWork.CommentRepo.GetByIdAsync(id);

            var ad = await _unitOfWork.Ad.GetByIdAsync(comment.AdId);

            if (comment.CustomerID != customerId) throw new BusinessException("You can only delete your own comments.");
            _unitOfWork.CommentRepo.Delete(comment);
            await _unitOfWork.SaveChangesAsync();

            await _cacheService.RemoveAsync($"ad:{ad.Id}");
            await _cacheService.RemoveAsync($"adcomments:{ad.Id}");



        }

        public async Task DeleteReplyAsync(int id, string AppUserownerId)
        {
            var reply = await _unitOfWork.ReplyRepo.GetReplyWithComment(id);

            var owner = await _unitOfWork.Owner.GetByAppUserIdAsync(AppUserownerId);

            if (reply.OwnerId != owner.Id)
                throw new BusinessException("You can only delete your own replies.");

            _unitOfWork.ReplyRepo.Delete(reply);
            await _unitOfWork.SaveChangesAsync();

            await _cacheService.RemoveAsync($"ad:{reply.Comment.AdId}");
            await _cacheService.RemoveAsync($"adcomments:{reply.Comment.AdId}");


        }

        public async Task<List<CommentResponseDto>> GetCommentsByAdIdAsync(int adId)
        {
            var comments = await _unitOfWork.CommentRepo.GetCommentsWithReplies(adId);

            if (!comments.Any())
                return new List<CommentResponseDto>();


            var customerIds = comments
                               .Select(c => c.CustomerID)
                               .Distinct()
                               .ToList();

            var ownerIds = comments
                            .Where(c => c.Reply != null)
                            .Select(c => c.Reply.Owner.AppUserId)
                            .Distinct()
                            .ToList();

            var users = await _userManager.Users
                                .Where(u => customerIds.Contains(u.Id) || ownerIds.Contains(u.Id))
                                .ToListAsync();

            var usersLookup = users.ToDictionary(u => u.Id);

            return comments
           .OrderByDescending(c => c.CreatedAt)
           .Select(c =>
           {
               usersLookup.TryGetValue(c.CustomerID, out var customer);

               ReplyResponseDto replyDto = null;

               if (c.Reply != null)
               {
                   usersLookup.TryGetValue(c.Reply.Owner.AppUserId, out var owner);

                   replyDto = new ReplyResponseDto
                   {
                       Id = c.Reply.Id,
                       Content = c.Reply.Message,
                       CreatedAt = c.Reply.CreatedAt,
                       AuthorName = owner != null
                           ? $"{owner.FirstName} {owner.LastName}"
                           : "Unknown"
                   };
               }

               return new CommentResponseDto
               {
                   Id = c.Id,
                   AdId = c.AdId,
                   Content = c.Message,
                   CreatedAt = c.CreatedAt,
                   AuthorName = customer != null
                       ? $"{customer.FirstName} {customer.LastName}"
                       : "Unknown",
                   Reply = replyDto
               };
           })
           
           .ToList();
        }









    }
}
