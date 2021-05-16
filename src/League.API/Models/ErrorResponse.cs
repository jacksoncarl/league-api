using System.Text.Json.Serialization;

namespace League.API.Models
{
    public class ErrorResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("param")]
        public string ParameterName { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public static ErrorResponse GenerateErrorResponse(string parameterName, string errorMessage)
        {
            return new()
            {
                ParameterName = parameterName,
                Message = errorMessage
            };
        }
    }
}
