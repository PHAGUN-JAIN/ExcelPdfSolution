using CsvHelper;
using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Text.Json;

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
        [Route("jsonParameter")]
        public IActionResult jsonvVals()
        {

            return Content("hello json");
        }

        [HttpPost]
        [Route("jsonParameter")]
        public ActionResult jsonvValsPost(object jsonParam)
        {
            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            var jobject = JsonConvert.DeserializeObject<JObject>(jsonParam.ToString());


            foreach (var oParam in jobject)
            {
                int count = oParam.Value.Count();

                for (int i = 0; i < count; i++)
                {
                    string temp = JsonConvert.SerializeObject(oParam.Value[i]);
                    JObject jObj = JObject.Parse(temp);

                    //foreach(JProperty property in jObj.Properties())
                    //{
                    //    csvWriter.WriteField(property.Name);
                    //}

                    foreach (JProperty defProp in jObj.Properties())
                    {
                        //Console.WriteLine(defProp.Name + " " + defProp.Value);
                        String tempVal = defProp.Value.ToString();
                        csvWriter.WriteField(tempVal);
                    }
                    csvWriter.NextRecord();
                }

            }
            writer.Flush();

            return File(mem.ToArray(), "application/octet-stream", "reports.csv");

        }
    }
}
