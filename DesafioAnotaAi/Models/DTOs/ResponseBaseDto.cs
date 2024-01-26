using System.Net;

namespace DesafioAnotaAi.Models.DTOs;

public class ResponseBaseDto<T> where T : class
{
    public HttpStatusCode StatusCode { get; set; }
    public T Data { get; set; }
}
