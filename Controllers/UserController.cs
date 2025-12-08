using AssetRegistry.DTOs;
using AssetRegistry.DTOs.Users;
using AssetRegistry.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AssetRegistry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await (from us in _context.ApplicationUsers
                                   join ur in _context.UserRoles on us.Id equals ur.UserId
                                   join ro in _context.Roles on ur.RoleId equals ro.Id
                                   select new UserListDTO()
                                   {
                                       Id = us.Id,
                                       FirstName = us.FirstName,
                                       LastName = us.LastName,
                                       Email = us.Email,
                                       Nic = us.Nic,
                                       Address = us.Address,
                                       IsActive = us.IsActive,
                                       RoleId = ur.RoleId,
                                       RoleName = ro.Name
                                   }).ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] RegisterDTO model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || !new EmailAddressAttribute().IsValid(model.Email))
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDTO { Status = "Error", Message = "Invalid email address!" });
                }

                var userExists = await _userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDTO { Status = "Error", Message = "User already exists!" });
                }

                var _role = await _roleManager.FindByIdAsync(model.RoleId);
                if (_role == null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDTO { Status = "Error", Message = "Role Not Exist." });
                }

                ApplicationUser user = new()
                {
                    FirstName = model.Email,
                    LastName = model.LastName,
                    Nic = model.Nic,
                    Address = model.Address,
                    Email = model.Email,
                    UserName = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsActive = true,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, _role.Name);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                }
                return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] RegisterDTO model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) ||
                    !new EmailAddressAttribute().IsValid(model.Email))
                {
                    return BadRequest(new { message = "Invalid email address." });
                }

                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                user.Email = model.Email;
                user.UserName = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        message = "Update failed.",
                        errors = result.Errors.Select(e => e.Description)
                    });
                }

                return Ok(new { message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the user.",
                    error = ex.Message
                });
            }
        }
    }
}
