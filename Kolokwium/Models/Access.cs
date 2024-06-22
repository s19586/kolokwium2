namespace Kolokwium.Models
{
    public class Access
    {
        public int IdUser { get; set; }
        public User User { get; set; }
        public int IdProject { get; set; }
        public Project Project { get; set; }
    }
}
