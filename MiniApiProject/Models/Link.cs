using System.ComponentModel.DataAnnotations;

namespace MiniApiProject.Models
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }
        public string? Url { get; set; }


        public List<Link_Person> Link_Persons { get; set; }
        public List<Link_Interest> Link_Interests { get; set; }
    }
}
