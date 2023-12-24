using System.ComponentModel.DataAnnotations;

namespace MiniApiProject.Models
{
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }

        public Person Person { get; set; }
    }
}
