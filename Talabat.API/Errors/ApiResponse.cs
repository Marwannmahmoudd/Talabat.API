namespace Talabat.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode,string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request , You Have Made",
                401 => "Authorized , you are not",
                404 => "Resource was not Found",
                500 => "Errors are the path to dark side , Errors lead to anger , Anger head to career change",
                _ => null
            }; 
        }
    }
}
