using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Domain.Entities
{
    public class Reply
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }

}
