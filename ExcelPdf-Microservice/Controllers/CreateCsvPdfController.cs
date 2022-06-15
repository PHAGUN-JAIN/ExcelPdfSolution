using CsvHelper;
using ExcelPdf_Microservice.Data;
using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.IO;
using static ExcelPdf_Microservice.Dtos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Grpc.Core;
using Microsoft.Extensions.FileProviders;

namespace ExcelPdf_Microservice.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CsvPdfController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CsvPdfController(ApplicationDbContext db)
        {
            _context = db;
        }

        [HttpGet]
        [Route("csv")]
        public ActionResult csv()
        {
            string csv = string.Empty;

            BindingFlags bindingFlags = BindingFlags.Public |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Instance |
                                        BindingFlags.Static;

            List<string> names = new List<string>();

            foreach (FieldInfo field in typeof(Agent).GetFields(bindingFlags))
            {
                //Console.WriteLine(field.Name);
                names.Add(field.Name.Split('<', '>')[1]);
            }
            //names.ForEach(Console.WriteLine);
            IEnumerable<Agent> obj = (IEnumerable<Agent>)_context.Agents;

            List<AgentDto> valsArr = new List<AgentDto>();

            foreach (var val in obj)
            {
                AgentDto vals = new(val.AGENT_CODE, val.AGENT_NAME, val.WORKING_AREA, val.COMMISSION, val.PHONE_NO, val.COUNTRY);
                valsArr.Add(vals);
                //Console.WriteLine(vals);
            }
            Console.WriteLine(valsArr.ToString());

            using var mem = new MemoryStream();
            using var writer = new StreamWriter(mem);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            names.ForEach(csvWriter.WriteField);
            csvWriter.NextRecord();

            foreach (AgentDto val in valsArr)
            {
                csvWriter.WriteRecord(val);
                csvWriter.NextRecord();
            }

            writer.Flush();
            var res = Encoding.UTF8.GetString(mem.ToArray());

            return File(mem.ToArray(), "application/octet-stream", "reports.csv");

            //return Content(res);
            //return File(mem.ToArray(), "text/csv", "awesome.csv");
        }

        [HttpGet]
        [Route("pdf")]
        public ActionResult pdf()
        {
            var filePath = "E:\\data\\VisualStudio\\source\\repos\\ExcelPdfSolution\\ExcelPdf-Microservice\\Temp\\gud.pdf";
            
            System.IO.File.Delete(filePath);
            BindingFlags bindingFlags = BindingFlags.Public |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Instance |
                                        BindingFlags.Static;

            List<string> names = new List<string>();

            foreach (FieldInfo field in typeof(Agent).GetFields(bindingFlags))
            {
                //Console.WriteLine(field.Name);
                names.Add(field.Name.Split('<', '>')[1]);
            }

            IEnumerable<Agent> obj = (IEnumerable<Agent>)_context.Agents;
            List<AgentDto> valsArr = new List<AgentDto>();

            foreach (var val in obj)
            {
                AgentDto vals = new(val.AGENT_CODE, val.AGENT_NAME, val.WORKING_AREA, val.COMMISSION, val.PHONE_NO, val.COUNTRY);
                valsArr.Add(vals);
            }

            //var lines = System.IO.File.ReadAllLines("E:\\data\\VisualStudio\\source\\repos\\ExcelPdfSolution\\ExcelPdf-Microservice\\Temp\\reports.csv", Encoding.UTF8).Select(a => a.Split(";"));

            // This code is for export Database data to PDF file
            string fileName = Guid.NewGuid() + ".pdf";
            //string filePath = Path.Combine(Server.MapPath("~/PDFFiles"), fileName);

            Document doc = new Document(PageSize.A4, 2, 2, 2, 2);

            // Create paragraph for show in PDF file header
            Paragraph p = new Paragraph("Export Database data to PDF file in ASP.NET");
            
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                //Create table here for write database data
                PdfPTable pdfTab = new PdfPTable(6); // here 6 is no of column
                pdfTab.HorizontalAlignment = 1; // 0- Left, 1- Center, 2- right
                pdfTab.SpacingBefore = 20f;
                pdfTab.SpacingAfter = 20f;

                names.ForEach(pdfTab.AddCell);

                foreach (var val in obj)
                {
                    AgentDto vals = new(val.AGENT_CODE, val.AGENT_NAME, val.WORKING_AREA, val.COMMISSION, val.PHONE_NO, val.COUNTRY);
                    valsArr.Add(vals);
                }

                foreach (AgentDto val in valsArr)
                {
                    pdfTab.AddCell(val.AGENT_CODE);
                    pdfTab.AddCell(val.AGENT_NAME);
                    pdfTab.AddCell(val.COMMISSIION.ToString());
                    pdfTab.AddCell(val.WORKING_AREA);
                    pdfTab.AddCell(val.PHONE);
                    pdfTab.AddCell(val.COUNTRY);

                }

                doc.Open();
                doc.Add(p);
                doc.Add(pdfTab);
                doc.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                doc.Close();
            }
            var filepath = @"E:\data\VisualStudio\source\repos\ExcelPdfSolution\ExcelPdf-Microservice\Temp\";
            var filename = "gud.pdf";
            IFileProvider provider = new PhysicalFileProvider(filepath);
            IFileInfo fileInfo = provider.GetFileInfo(filename);
            var readStream = fileInfo.CreateReadStream();
            return File(readStream, "application/octet-stream", "file.pdf");
            //return lines;
        }

    }
}
