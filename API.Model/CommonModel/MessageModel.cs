using API.Common;
using System.Net;

namespace API.Model
{
    public class MessageModel
    {
        public string Message { get; set; }
        public string Type { get; set; } = Enumeration.MessageType.Success;
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
    } 
}
