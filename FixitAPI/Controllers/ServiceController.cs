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
                Age = employeeDto.Age,
                Gender = employeeDto.Gender,
                Email = employeeDto.Email,
                Address = employeeDto.Address,
                City = employeeDto.City,
                State = employeeDto.State,
                PhoneNumber = employeeDto.PhoneNumber,
                 Role = string.Join(",", employeeDto.Roles)
            };

         
            _context.Employees.Add(employee);
            _context.SaveChanges();


            foreach (var roleName in employeeDto.Roles)
            {

                var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
                if (role != null)
                {
              
                    foreach (var serviceId in employeeDto.ServiceIds)
                    {
                    
                        var service = _context.Services.FirstOrDefault(s => s.Id == serviceId && s.RoleId == role.Id);
                        if (service != null)
                        {
                            var employeeService = new EmployeeService
                            {
                                EmployeeId = employee.Id,
                                ServiceId = serviceId
                            };

                            _context.EmployeeServices.Add(employeeService);
                        }
                    }
                }
            }

      
            _context.SaveChanges();

            return Ok(new { message = "Employee details saved successfully!" });
        }

        return BadRequest(ModelState);
    }


    [HttpPut("UpdateEmployeeDetails")]
    public IActionResult UpdateEmployeeDetails(int id, [FromBody] EmployeeDto employeeDto)
    {
        if (ModelState.IsValid)
        {
            // Find the existing employee record by Id
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

     
            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Age = employeeDto.Age;
            employee.Gender = employeeDto.Gender;
            employee.Email = employeeDto.Email;
            employee.Address = employeeDto.Address;
            employee.City = employeeDto.City;
            employee.State = employeeDto.State;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.Role = string.Join(",", employeeDto.Roles);

     
            var existingEmployeeServices = _context.EmployeeServices.Where(es => es.EmployeeId == employee.Id).ToList();
            _context.EmployeeServices.RemoveRange(existingEmployeeServices);

            foreach (var roleName in employeeDto.Roles)
            {
                var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
                if (role != null)
                {
                    foreach (var serviceId in employeeDto.ServiceIds)
                    {
                        var service = _context.Services.FirstOrDefault(s => s.Id == serviceId && s.RoleId == role.Id);
                        if (service != null)
                        {
                            var employeeService = new EmployeeService
                            {
                                EmployeeId = employee.Id,
                                ServiceId = serviceId
                            };
                            _context.EmployeeServices.Add(employeeService);
                        }
                    }
                }
            }

            // Save the updated employee and services
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
}

