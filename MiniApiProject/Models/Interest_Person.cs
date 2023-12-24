namespace MiniApiProject.Models
{
    public class Interest_Person
    {
        public int PersonId { get; set; }

        public Person Person { get; set; }
        public int InterestId { get; set; }

        public Interest Interest { get; set; }
    }
}
