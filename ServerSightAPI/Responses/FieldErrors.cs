using System.Collections.Generic;

namespace ServerSightAPI.Responses
{
    public class FieldErrors
    {
        public List<FieldError> Errors { get; set; }

        public FieldErrors(List<FieldError>fieldErrors)
        {
            Errors = fieldErrors;
        }
    }
}