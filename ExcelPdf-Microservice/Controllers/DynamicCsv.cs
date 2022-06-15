using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ExcelPdf_Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DynamicCsv : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            BindingFlags bindingFlags = BindingFlags.Public |
                                     BindingFlags.NonPublic |
                                     BindingFlags.Instance |
                                     BindingFlags.Static;

            List<string> names = new List<string>();

            foreach (PropertyInfo field in typeof(Agent).GetProperties(bindingFlags))
            {
                Console.WriteLine(field.Name);
                names.Add(field.Name);
            }

            return Content("hello There!");
        }

        [HttpGet]
        public IActionResult jsonvVals()
        {

            return null;
        }
    }
}
