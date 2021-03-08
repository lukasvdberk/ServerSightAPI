namespace ServerSightAPI.Responses
{
    public class FieldError
    {
        public string Field { get; set; }
        public string Error { get; set; }

        public FieldError(string field, string error)
        {
            Field = field;
            Error = error;
        }
    }
}