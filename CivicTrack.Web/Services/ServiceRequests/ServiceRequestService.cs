using CivicTrack.Web.Data;
using CivicTrack.Web.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CivicTrack.Web.Services.ServiceRequests
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly ApplicationDbContext _dbContext;


        public ServiceRequestService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(
            CreateServiceRequestDto request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.ServiceCategoryId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(request.ServiceCategoryId),
                    "Service category ID must be greater than zero.");
            }

            bool categoryIsAvailable = await _dbContext.ServiceCategories.AnyAsync(
                category =>
                    category.Id == request.ServiceCategoryId &&
                     category.IsActive,
                cancellationToken);

            if (!categoryIsAvailable)
            { 
                throw new InvalidOperationException(
                    "The selected service category does not exist or is inactive.");
            }

            var serviceRequest = new ServiceRequest(
                request.Title,
                request.Description,
                request.ServiceCategoryId);

            _dbContext.ServiceRequests.Add(serviceRequest);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return serviceRequest.Id;
        }
  
    }
}
