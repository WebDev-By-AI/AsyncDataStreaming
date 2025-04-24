using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StreamEmployees.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyCorsPolicy")]

    public class EmployeesController : ControllerBase
    {
        [HttpGet("stream")]
        public async Task StreamEmployees()
        {
            Response.ContentType = "text/event-stream"; // Server-Sent Events (SSE)

            var employees = GenerateEmployees();

            await using var writer = new StreamWriter(Response.Body);
            foreach (var employee in employees)
            {
                var json = JsonSerializer.Serialize(employee);
                Console.WriteLine($"Sending: {json}"); // Log to console

                await writer.WriteLineAsync($"data: {json}");
                await writer.FlushAsync();
                await Task.Delay(500); // Simulate delay between records
            }
        }

        private IEnumerable<Employee> GenerateEmployees()
        {
            for (int i = 1; i <= 100; i++)
            {
                yield return new Employee
                {
                    Id = i,
                    Name = $"Employee {i}",
                    Email = $"employee{i}@example.com",
                    Status = (i % 2 == 0) ? "Active" : "Inactive"
                };
            }
        }
    }


    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }


}
