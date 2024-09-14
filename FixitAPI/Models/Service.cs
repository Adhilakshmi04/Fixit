namespace FixitAPI.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ServiceName { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public ICollection<EmployeeService> EmployeeServices { get; set; }
    }

}
