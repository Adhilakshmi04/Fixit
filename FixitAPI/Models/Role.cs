namespace FixitAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}

