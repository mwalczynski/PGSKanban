using System.Net;

namespace PgsKanban.BusinessLogic.Responses
{
    public class QueryResponse<T>
    {
        public T ResponseDto { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
