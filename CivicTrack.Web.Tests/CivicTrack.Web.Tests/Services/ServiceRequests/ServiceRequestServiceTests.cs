using CivicTrack.Web.Data;
using Microsoft.EntityFrameworkCore;
using CivicTrack.Web.Services.ServiceRequests;
using CivicTrack.Web.Domain.Entities;

namespace CivicTrack.Web.Tests.Services.ServiceRequests
{
    public class ServiceRequestServiceTests
    {
        private static ApplicationDbContext CreateDbContext()
        {
            var option =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(option);
        }

        [Fact]
        public async Task CreateAsync_WhenRequestIsNull_ThrowsArgumentNullException()
        {
            await using ApplicationDbContext dbContext = CreateDbContext();

            var service = new ServiceRequestService(dbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(null!));
        }
        [Fact]
        public async Task CreateAsync_WhenCategoryDoesNotExist_ThrowsInvalidOperationsException()
        {
            await using ApplicationDbContext dbContext = CreateDbContext();

            var service = new ServiceRequestService(dbContext);

            var request = new CreateServiceRequestDto
            {
                Title = "Broken streetlight",
                Description = "The streetlight has not worked for several days",
                ServiceCategoryId = 999
            };

            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));

            Assert.Equal(
                "The selected service category does not exist or is inactive.",
                exception.Message);
        }
        [Fact]
        public async Task CreateAsync_WhenCategoryIsActive_CreatesRequestAndReturnsId()
        {
            await using ApplicationDbContext dbContext = CreateDbContext();

            var category = new ServiceCategory("Road Repair");

            dbContext.ServiceCategories.Add(category);
            await dbContext.SaveChangesAsync();

            var service = new ServiceRequestService(dbContext);

            var request = new CreateServiceRequestDto
            {
                Title = "Large pothole",
                Description = "There is a large pothole near the intersection.",
                ServiceCategoryId = category.Id
            };

            int createId = await service.CreateAsync(request);

            ServiceRequest? savedRequest =
                await dbContext.ServiceRequests.FindAsync(createId);

            Assert.NotEqual(0, createId);
            Assert.NotNull(savedRequest);
            Assert.Equal(request.Title, savedRequest.Title);
            Assert.Equal(request.Description, savedRequest.Description);
            Assert.Equal(request.ServiceCategoryId, savedRequest.ServiceCategoryId);
        }
        [Fact]
        public async Task CreateAsync_WhenCategoryIsInactive_ThrowsInvalidOperationException()
        {
            await using ApplicationDbContext dbContext = CreateDbContext();
            var category = new ServiceCategory("Road Repair");
            category.Deactivate();

            dbContext.ServiceCategories.Add(category);
            await dbContext.SaveChangesAsync();

            var service = new ServiceRequestService(dbContext);
            var request = new CreateServiceRequestDto
            {
                Title = "Large pothole",
                Description = "There is a large pothole near the intersection.",
                ServiceCategoryId = category.Id
            };

            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));

            Assert.Equal(
               "The selected service category does not exist or is inactive.",
               exception.Message);

            int requestCount = await dbContext.ServiceRequests.CountAsync();

            Assert.Equal(0, requestCount);

        }
        [Fact]
        public async Task CreateAsync_WhenTitleIsBlank_ThrowsArgumentException()
        {
            await using ApplicationDbContext dbContext = CreateDbContext();

            var category = new ServiceCategory("Road Repair");

            dbContext.ServiceCategories.Add(category);

            await dbContext.SaveChangesAsync();

            var service = new ServiceRequestService(dbContext);
            var request = new CreateServiceRequestDto
            {
                Title = " ",
                Description = "There is a large pothole near intersection",
                ServiceCategoryId = category.Id
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateAsync(request));

            int requestCount = await dbContext.ServiceRequests.CountAsync();

            Assert.Equal(0, requestCount);
        }
        [Fact]
        public async Task CreateAsync_WhenDescriptionIsBlank_ThrowsArgumentException()
        {
            await using ApplicationDbContext dbContext = CreateDbContext();
            var category = new ServiceCategory("Road Repair");

            dbContext.ServiceCategories.Add(category);

            await dbContext.SaveChangesAsync();

            var service = new ServiceRequestService(dbContext);
            var request = new CreateServiceRequestDto
            {
                Title = "Road Repair",
                Description = " ",
                ServiceCategoryId = category.Id
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateAsync(request));

            int requestCount = await dbContext.ServiceRequests.CountAsync();
            Assert.Equal(0, requestCount);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CreateAsync_WhenCategoryIdIsZero_ThrowsArgumentOutOfRangeException(int serviceCategoryId)
        {
            await using ApplicationDbContext dbContext = CreateDbContext();

            await dbContext.SaveChangesAsync();

            var service = new ServiceRequestService(dbContext);
            var request = new CreateServiceRequestDto
            {
                Title = "Road Repair",
                Description = "There is a large pothole on the intersection",
                ServiceCategoryId = serviceCategoryId
            };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => service.CreateAsync(request));

            int requestCount = await dbContext.ServiceRequests.CountAsync();
            Assert.Equal(0, requestCount);
        }
    }
}
