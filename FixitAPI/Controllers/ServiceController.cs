using FixitAPI.Data;
using FixitAPI.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServiceController(AppDbContext context)
    {
        _context = context;
    }


    [HttpPost("SaveEmployeeDetails")]
    public IActionResult SaveEmployeeDetails([FromBody] EmployeeDto employeeDto)
    {
        if (ModelState.IsValid)
        {           
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Age = employeeDto.Age ?? 0,  
                Gender = employeeDto.Gender,
                Email = employeeDto.Email,
                Address = employeeDto.Address,
                City = employeeDto.City,
                State = employeeDto.State,
                PhoneNumber = employeeDto.PhoneNumber,
                Role = string.Join(",", employeeDto.Roles),
                Experience = employeeDto.Experience?.ToString() ?? "0"  
            };
            
            _context.Employees.Add(employee);
            _context.SaveChanges();
            
            foreach (var roleName in employeeDto.Roles)
            {
                var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
                if (role != null)
                {
                    var servicesForRole = _context.Services.Where(s => s.RoleId == role.Id).ToList();
                    foreach (var service in servicesForRole)
                    {
                        var employeeService = new EmployeeService
                        {
                            EmployeeId = employee.Id,
                            ServiceId = service.Id
                        };

                        _context.EmployeeServices.Add(employeeService);
                    }
                }
            }
            
            _context.SaveChanges();

            return Ok(new { message = "Employee details saved successfully with associated services!" });
        }

        return BadRequest(ModelState);
    }




    [HttpPut("UpdateEmployeeDetails")]
    public IActionResult UpdateEmployeeDetails(int id, [FromBody] EmployeeDto employeeDto)
    {
        if (ModelState.IsValid)
        {           
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }
            employee.FirstName = employeeDto.FirstName ?? employee.FirstName;
            employee.LastName = employeeDto.LastName ?? employee.LastName;
            employee.Age = employeeDto.Age ?? employee.Age;  
            employee.Gender = employeeDto.Gender ?? employee.Gender;
            employee.Email = employeeDto.Email ?? employee.Email;
            employee.Address = employeeDto.Address ?? employee.Address;
            employee.City = employeeDto.City ?? employee.City;
            employee.State = employeeDto.State ?? employee.State;
            employee.PhoneNumber = employeeDto.PhoneNumber ?? employee.PhoneNumber;
            employee.Role = string.Join(",", employeeDto.Roles);
            employee.Experience = employeeDto.Experience.HasValue ? employeeDto.Experience.Value.ToString() : employee.Experience;


            var existingEmployeeServices = _context.EmployeeServices.Where(es => es.EmployeeId == employee.Id).ToList();
            _context.EmployeeServices.RemoveRange(existingEmployeeServices);

            
            if (employeeDto.Roles != null)
            {
                foreach (var roleName in employeeDto.Roles)
                {
                    var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        var servicesForRole = _context.Services.Where(s => s.RoleId == role.Id).ToList();
                        foreach (var service in servicesForRole)
                        {
                            var employeeService = new EmployeeService
                            {
                                EmployeeId = employee.Id,
                                ServiceId = service.Id
                            };
                            _context.EmployeeServices.Add(employeeService);
                        }
                    }
                }
            }

           
            _context.SaveChanges();

            return Ok(new { message = "Employee details updated successfully!" });
        }

        return BadRequest(ModelState);
    }




    [HttpGet("GetServicesByRole/{roleId}")]
    public IActionResult GetServicesByRole(int roleId)
    {
        var services = _context.Services
            .Where(s => s.RoleId == roleId)
            .Select(s => new
            {
                s.Id,
                s.ServiceName
            }).ToList();

        if (!services.Any())
        {
            return NotFound("No services found for this role.");
        }

        return Ok(services);
    }


    [HttpPost("SearchEmployees")]
    public IActionResult SearchEmployees([FromBody] SearchEmployeeDto searchDto)
    {
        var matchingEmployees = _context.Employees
     .Select(e => new
     {
         e.FirstName,
         e.LastName,
         e.Age,
         e.Gender,
         e.Email,
         e.Address,
         e.City,
         e.State,
         e.PhoneNumber,
         Role = e.Role ?? "No Role Assigned"  
     })
     .ToList();

        if (!matchingEmployees.Any())
        {
            return NotFound("No servies match the provided criteria.");
        }

        return Ok(matchingEmployees);
    }

    [HttpPost("SaveRoles")]
    public IActionResult SaveRoles([FromBody] RoleServiceDto roleServiceDto)
    {
        
        var existingRole = _context.Roles.FirstOrDefault(r => r.RoleName == roleServiceDto.RoleName);

        if (existingRole != null)
        {
            return BadRequest("Role already exists.");
        }
       
        var newRole = new Role
        {
            RoleName = roleServiceDto.RoleName
        };
        
        _context.Roles.Add(newRole);
        _context.SaveChanges();  
        var roleId = newRole.Id;        
        foreach (var serviceName in roleServiceDto.ServiceNames)
        {
            var service = new Service
            {
                ServiceName = serviceName,
                RoleId = roleId  
            };
            _context.Services.Add(service);
        }
        _context.SaveChanges();

        return Ok(new { message = "Role and services saved successfully!" });
    }

}

