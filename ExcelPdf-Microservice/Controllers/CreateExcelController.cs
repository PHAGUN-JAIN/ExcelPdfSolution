using ExcelPdf_Microservice.Data;
using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExcelPdf_Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateExcelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CreateExcelController(ApplicationDbContext db)
        {
            _context = db;
        }
        [HttpGet]   
        public IEnumerable<Agent> Index()
        {
            IEnumerable<Agent> obj = (IEnumerable<Agent>)_context.Agents;
            return obj;
        }


    }
}
