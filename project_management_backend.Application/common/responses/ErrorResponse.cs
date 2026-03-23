namespace project_management_backend.Application.common.responses
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Errors { get; set; }
    }
}