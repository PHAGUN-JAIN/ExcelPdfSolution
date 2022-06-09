using System.ComponentModel.DataAnnotations;

namespace ExcelPdf_Microservice.Models
{
    public class Agent
    {
        [Key]
        public string AGENT_CODE { get; set; }
        public string AGENT_NAME { get; set; }
        public string WORKING_AREA { get; set; }
        public decimal COMMISSION { get; set; }
        public string PHONE_NO { get; set; }
        public string COUNTRY { get; set; }
    }
}
