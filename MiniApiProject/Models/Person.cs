using System.ComponentModel.DataAnnotations;

namespace MiniApiProject.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Phone> Phones { get; set; }

        public List<Interest_Person> Interest_Persons { get; set; }

        public List<Link_Person> Link_Persons { get; set; }
    }
}
