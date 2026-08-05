namespace CivicTrack.Web.Services.ServiceRequests
{
    public class CreateServiceRequestDto
    {
        public required string Title { get; init; }
        public required string Description { get; init; }

        public int ServiceCategoryId { get; init; }
    }
}
