using ExcelPdf_Microservice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelPdf_Microservice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public static explicit operator ActionResult(ApplicationDbContext v)
        {
            throw new NotImplementedException();
        }

        public DbSet<Agent> Agents { get; set; }
    }
}