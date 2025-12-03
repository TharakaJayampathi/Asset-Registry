namespace test_backend.Models
{
    public class HttpResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string? Message { get; set; }
    }
}
