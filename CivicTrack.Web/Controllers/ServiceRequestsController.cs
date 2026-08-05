using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CivicTrack.Web.Data;

namespace CivicTrack.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var requests = _context.ServiceRequests;

            return View(await requests.ToListAsync());
        }

       
    }
}
