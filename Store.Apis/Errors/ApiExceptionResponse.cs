namespace Store.Apis.Errors
{
    public class ApiExceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(int statusCode, string? details=null, string? errorMessage = null) : base(statusCode, errorMessage)
        {
            Details = details;
        }
    }
}
