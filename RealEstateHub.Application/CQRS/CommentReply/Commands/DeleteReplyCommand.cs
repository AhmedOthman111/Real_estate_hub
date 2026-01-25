using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.CommentReply.Commands
{
    public  record DeleteReplyCommand(int id  , string AppUserownerId) : IRequest;
    
    
}
