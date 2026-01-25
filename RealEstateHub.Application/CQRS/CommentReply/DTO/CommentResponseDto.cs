using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.CommentReply.DTO
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReplyResponseDto? Reply { get; set; }
    }
}
