namespace ServerSightAPI.Responses
{
    public class FieldError
    {
        public FieldError(string field, string error)
        {
            Field = field;
            Error = error;
        }

        public string Field { get; set; }
        public string Error { get; set; }
    }
}