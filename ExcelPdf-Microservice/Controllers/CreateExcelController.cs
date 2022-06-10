using ExcelPdf_Microservice.Data;
using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
        public void Index()
        {
            //IEnumerable<Agent>

            BindingFlags bindingFlags = BindingFlags.Public |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Instance |
                                        BindingFlags.Static;

            var fieldNames = typeof(Agent).GetFields()
                                .Select(field => field.Name)
                                .ToList();


            List<String> names = new List<string>();

            foreach (FieldInfo field in typeof(Agent).GetFields(bindingFlags))
            {
                Console.WriteLine(field.Name);
                names.Add(field.Name.Split('<', '>')[1]);
            }

            names.ForEach(Console.WriteLine);
            //int count = 0;
            //IEnumerable<Agent> obj = (IEnumerable<Agent>)_context.Agents;
            //foreach (var item in obj)
            //{
            //    String retString = String.Empty;

            //    Console.WriteLine(item.AGENT_CODE);
            //    count++;
            //}
            //Console.WriteLine(count);
            //return obj;
        }


    }
}
