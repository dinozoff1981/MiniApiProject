namespace MiniApiProject.Models
{
    public class Link_Interest
    {
        public int LinkId { get; set; }
        public Link Link { get; set; }
        public int InterestId { get; set; }

        public Interest interest { get; set; }
    }
}
