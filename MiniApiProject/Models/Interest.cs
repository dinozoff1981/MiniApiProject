using System.ComponentModel.DataAnnotations;

namespace MiniApiProject.Models
{
    public class Interest
    {
        [Key]
        public int InterestId { get; set; }
        public string Titel { get; set; }

        public string Description { get; set; }




        public List<Interest_Person> Interest_Persons { get; set; }
        public List<Link_Interest> Link_Interests { get; set;}
    }
}
