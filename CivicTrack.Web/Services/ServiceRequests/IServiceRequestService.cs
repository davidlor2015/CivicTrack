namespace CivicTrack.Web.Services.ServiceRequests
{
    public interface IServiceRequestService
    {
        Task<int> CreateAsync(
            CreateServiceRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
