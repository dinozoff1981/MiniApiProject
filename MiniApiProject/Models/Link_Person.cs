namespace MiniApiProject.Models
{
    public class Link_Person
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int LinkId { get; set; }

        public Link Link { get; set; }
    }
}
