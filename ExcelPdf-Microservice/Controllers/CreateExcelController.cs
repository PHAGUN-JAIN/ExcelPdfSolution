using ExcelPdf_Microservice.Data;
using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using static ExcelPdf_Microservice.Dtos;

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

            BindingFlags bindingFlags = BindingFlags.Public |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Instance |
                                        BindingFlags.Static;

            List<string> names = new List<string>();

            //foreach (PropertyInfo field in typeof(Agent).GetProperties(bindingFlags))
            //{
            //    Console.WriteLine(field.Name);
            //    names.Add(field.Name);
            //}

            foreach (FieldInfo field in typeof(Agent).GetFields(bindingFlags))
            {
                //Console.WriteLine(field.Name);
                names.Add(field.Name.Split('<', '>')[1]);
            }

            names.ForEach(Console.WriteLine);

            IEnumerable<Agent> obj = (IEnumerable<Agent>)_context.Agents;

            // Console.WriteLine(_context.Agents);



            foreach (var val in obj)
            {
                //foreach (var name in names)
                //{
                //    //Console.WriteLine(GetValues(obj, name));


                //}
                //List<String> vals = new List<String>();
                //vals.Add(val.AGENT_CODE);
                //vals.Add(val.AGENT_NAME);
                //vals.Add(val.WORKING_AREA);
                //vals.Add(val.COMMISSION.ToString());
                //vals.Add(val.PHONE_NO);
                //vals.Add(val.COUNTRY);

                AgentDto vals = new(val.AGENT_CODE, val.AGENT_NAME, val.WORKING_AREA, val.COMMISSION, val.PHONE_NO, val.COUNTRY );

                Console.WriteLine(vals);


            }

            return obj;
        }
        //public static IEnumerable<object> GetValues<T>(IEnumerable<T> items, string propertyName)
        //{
        //    Type type = typeof(T);
        //    var prop = type.GetProperty(propertyName);
        //    foreach (var item in items)
        //        yield return prop.GetValue(item, null);
        //}

    }
}
