using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ExcelPdf_Microservice.Models
{
    //[DefaultMember("myMc")]
    public class Agent
    {
        [Key]
        public string AGENT_CODE { get; set; }
        public string AGENT_NAME { get; set; }
        public string WORKING_AREA { get; set; }
        public decimal COMMISSION { get; set; }
        public string PHONE_NO { get; set; }
        public string COUNTRY { get; set; }

        //PropertyAttributes AGENT_CODE { get; set; }

        //PropertyAttributes AGENT_NAME { get; set; }
        //PropertyAttributes WORKING_AREA { get; set;}
        //PropertyAttributes COMMISSION { get; set; }
        //PropertyAttributes PHONE_NO { get; set; }
        //PropertyAttributes Country { get; set; }
        //public static void Main(Agent myMC)
        //{
        //    Agent myMC = new Agent();
        //    int j = myMC[1]; // CS0021
        //}
    }
}
