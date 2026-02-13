namespace RealEstateHub.Application.CQRS.Rating.Dto
{
    public class RatingResponseDto
    {
        public int id { get; set; }
        public int OwnerId { get; set; }
        public int Stars { get; set; }
        public string Review { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
