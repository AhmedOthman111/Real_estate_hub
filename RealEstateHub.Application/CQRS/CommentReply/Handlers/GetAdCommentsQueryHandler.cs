using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.DTO;
using RealEstateHub.Application.CQRS.CommentReply.Queries;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.CommentReply.Handlers
{
    public class GetAdCommentsQueryHandler : IRequestHandler<GetAdCommentsQuery, List<CommentResponseDto>>
    {

        private readonly ICommentReplyService _commentService;
        private readonly ICacheService _cacheService;
        public GetAdCommentsQueryHandler(ICommentReplyService commentService, ICacheService cacheService)
        {
            _commentService = commentService;
            _cacheService = cacheService;
        }

        public async Task<List<CommentResponseDto>> Handle(GetAdCommentsQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"adcomments:{request.adId}";

            var cachedcomments = await _cacheService.GetAsync<List<CommentResponseDto>>(cacheKey, cancellationToken);
            if (cachedcomments != null) return cachedcomments;


            var comments = await _commentService.GetCommentsByAdIdAsync(request.adId);

            if (comments.Any()) await _cacheService.SetAsync(cacheKey, comments, TimeSpan.FromMinutes(10), null, cancellationToken);


            return comments;
        }


    }
}
