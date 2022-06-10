using ExcelPdf_Microservice.Models;

namespace ExcelPdf_Microservice
{
    public class Dtos
    {
        public record AgentDto(string AGENT_CODE, string AGENT_NAME, string WORKING_AREA, decimal COMMISSIION, string PHONE, string COUNTRY)
        {
            public static implicit operator AgentDto(Agent v)
            {
                throw new NotImplementedException();
            }
        }
    }
}
