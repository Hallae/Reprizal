using System.ComponentModel.DataAnnotations;

namespace myApi
{
    public class Application
    {

        [Key]
        [Required(ErrorMessage = " Guild is a required field")]// This attribute specifies that the Id property is the primary key
        public Guid id { get; set; } = Guid.NewGuid();
        
        public Guid author { get; set; } = Guid.NewGuid();


        [Required(ErrorMessage = "activity is a required field.")]
        public string activity { get; set; }

        [Required(ErrorMessage = "name is a required field.")]
        public string name { get; set; }
        public string description { get; set; }
        public string outline { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

    }
}

