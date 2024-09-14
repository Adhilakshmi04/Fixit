namespace FixitAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string TaskName { get; set; }
        public Role Role { get; set; }
    }
}
