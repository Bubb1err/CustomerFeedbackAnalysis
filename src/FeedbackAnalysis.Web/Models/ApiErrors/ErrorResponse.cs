using System.Net;

namespace FeedbackAnalysis.Web.Models.ApiErrors
{
    public class ErrorResponse
    {
        public ErrorResponse(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public HttpStatusCode Code { get; }

        public string Message { get; }
    }
}
