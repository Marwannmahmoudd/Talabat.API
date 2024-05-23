namespace Talabat.API.Errors
{
    public class ValidationErrorsResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationErrorsResponse() :base(400)
        {
            Errors = new List<string>();
        }
    }
}
