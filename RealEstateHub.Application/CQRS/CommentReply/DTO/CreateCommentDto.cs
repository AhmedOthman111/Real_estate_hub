namespace RealEstateHub.Application.CQRS.CommentReply.DTO
{
    public class CreateCommentDto
    {
        public int AdId { get; set; }
        public string Content { get; set; }
    }
}
