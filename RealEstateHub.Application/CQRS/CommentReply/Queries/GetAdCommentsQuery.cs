using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.CommentReply.Queries
{
    public record GetAdCommentsQuery(int adId) : IRequest<List<CommentResponseDto>>;
 
}
