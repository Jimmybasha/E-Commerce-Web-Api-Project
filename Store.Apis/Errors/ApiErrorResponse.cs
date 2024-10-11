namespace Store.Apis.Errors
{
    public class ApiErrorResponse
    {

        public int StatusCode { get; set; }

        public string? ErrorMessage { get; set; }

        public ApiErrorResponse(int statusCode, string? errorMessage = null)
        {
            StatusCode = statusCode;
            //To Generate a default message for every error
            ErrorMessage = errorMessage ?? GetDefaultMessageForStatusCode(statusCode);

             

        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "bad request occured!",
                401 => "You are not authorized!",
                404 =>"Resource isn't found",
                500 =>"Server Error",
                _=> null,

            };





            return message;
        }



    }
}
