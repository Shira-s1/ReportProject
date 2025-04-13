//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ReportProject.Core.Entities;
//using ReportProject.Core.Interfaces;
//using ReportProject.Data;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ReportProject.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PasswordFixController : ControllerBase
//    {
//        private readonly DataContext _dataContext;

//        public PasswordFixController(DataContext dataContext)
//        {
//            _dataContext = dataContext;
//        }

//        [HttpGet("fix")] // נקודת קצה חד פעמית לתיקון סיסמאות
//        public async Task<IActionResult> FixExistingPasswords()
//        {
//            var employeesToUpdate = _dataContext.empList.Where(e => !e.Password.StartsWith("$2a$")).ToList(); // בדיקה פשוטה אם הסיסמה לא נראית כמו BCrypt Hash

//            foreach (var employee in employeesToUpdate)
//            {
//                employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
//            }

//            await _dataContext.SaveChangesAsync();
//            return Ok("Existing passwords updated.");
//        }
//    }
//}