using System.ComponentModel.DataAnnotations;

namespace myApi
{
    public class Application
    {

        [Key]
        [Required]// This attribute specifies that the Id property is the primary key
        public Guid id { get; set; } = Guid.NewGuid();
        // Make the property settable and initialize with a new GUID
        public Guid author { get; set; } = Guid.NewGuid();


        [Required]
        public string activity { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public string outline { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

    }
}

