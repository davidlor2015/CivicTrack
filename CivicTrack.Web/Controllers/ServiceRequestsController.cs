using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CivicTrack.Web.Data;
using CivicTrack.Web.Services.ServiceRequests;

namespace CivicTrack.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceRequestService _serviceRequestService;

        public ServiceRequestsController(ApplicationDbContext context, IServiceRequestService serviceRequestService)
        {
            _context = context;
            _serviceRequestService = serviceRequestService;
        }
        public async Task<IActionResult> Index()
        {
            var requests = _context.ServiceRequests;

            return View(await requests.ToListAsync());
        }

        

       
    }
}
