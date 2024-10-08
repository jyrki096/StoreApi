using System.Net;

namespace Api.Models
{
    public class ServerResponse
    {
        public ServerResponse()
        {
            isSuccess = true;
            ErrorMessages = new();
        }

        public bool isSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    
    }
}