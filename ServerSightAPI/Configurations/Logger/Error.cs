using Newtonsoft.Json;

namespace ServerSightAPI.Configurations.Logger
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}