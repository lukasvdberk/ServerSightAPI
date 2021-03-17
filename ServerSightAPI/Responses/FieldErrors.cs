using System.Collections.Generic;

namespace ServerSightAPI.Responses
{
    public class FieldErrors
    {
        public FieldErrors(List<FieldError> fieldErrors)
        {
            Errors = fieldErrors;
        }

        public List<FieldError> Errors { get; set; }
    }
}