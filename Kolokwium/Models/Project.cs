namespace Kolokwium.Models
{
    public class Project
    {
        public int IdProject { get; set; }
        public string Name { get; set; }
        public int IdDefaultAssignee { get; set; }
        public User DefaultAssignee { get; set; }
    }
}
