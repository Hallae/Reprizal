using System.ComponentModel.DataAnnotations;

namespace myApi
{
    public class Application
    {
        [Key] // This attribute specifies that the Id property is the primary key
        public Guid id { get; set; } = "9c53ea53-a88d-4367-ad8a-281738690412";// Make the property settable and initialize with a new GUID
        public string author { get; set; }
        public string activity { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string outline { get; set; }


        public bool IsSubmitted { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}

