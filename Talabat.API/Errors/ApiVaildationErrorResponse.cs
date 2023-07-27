namespace Talabat.API.Errors
{
    public class ApiVaildationErrorResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiVaildationErrorResponse():base(400)
        {
            Errors = new List<string>();
        }

    }
}
