namespace ExcelPdf_Microservice
{
    public class Dtos
    {
        public record AgentDto(string agent_code, string agent_name, string working_area, decimal commissiion, string phone,string country);
    }
}
