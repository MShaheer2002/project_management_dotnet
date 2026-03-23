using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.Organization;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Users;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateUser(CreateUserRequestDto createUserDto)
        // {
        //     var hasedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        //     var newUser = new User(createUserDto.FirstName, createUserDto.LastName, createUserDto.Email, createUserDto.UserName, hasedPassword, createUserDto.AvatarUrl);

        //     await userRepository.CreateUserAsync(newUser);

        //     var user = new CreateUserResponseDto
        //     {
        //         Id = newUser.Id,
        //         FirstName = newUser.FirstName,
        //         LastName = newUser.LastName ?? "",
        //         UserName = newUser.UserName,
        //         Email = newUser.Email,
        //         AvatarUrl = newUser.AvatarUrl ?? "",

        //     };

        //     var response = new ApiResponse<Object>(true, user, "new user created");

        //     return Ok(response);

        // }


        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(Guid Id, [FromBody] UpdateRequestDto updateRequestDto)
        {

            var updatedUser = await userRepository.UpdateUserAsync(Id, updateRequestDto);

            if (updatedUser == null)
            {
                return NotFound(new ApiResponse<object>(false, "", "User not found"));
            }

            var user = new UpdateResponseDto
            {
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                UserName = updatedUser.UserName,
                AvatarUrl = updatedUser.AvatarUrl

            };

            var responseUser = new ApiResponse<Object>(true, user, "Data updated");

            return Ok(responseUser);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var deletedUser = await userRepository.DeleteUserAsync(Id);

            if (deletedUser == null)
                return NotFound(new ApiResponse<object>(false, "", "User not found"));


            var user = new DeleteUserDto
            {
                FirstName = deletedUser.FirstName,
                LastName = deletedUser.LastName,
                UserName = deletedUser.UserName,
                AvatarUrl = deletedUser.AvatarUrl
            };

            var response = new ApiResponse<Object>(true, user, "User deleted");
            return Ok(response);

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var User = await userRepository.GetUserAsync(Id);
            if (User == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "User Not Found"));
            }

            var user = new GetUserResponseDto
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                UserName = User.UserName,
                AvatarUrl = User.AvatarUrl
            };

            return Ok(new ApiResponse<Object>(true, user, "user found"));
        }

        [HttpGet("by_email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var User = await userRepository.GetUserByEmailAsync(email);
            if (User == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "User Not Found"));
            }

            var user = new GetUserResponseDto
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                UserName = User.UserName,
                AvatarUrl = User.AvatarUrl
            };

            return Ok(new ApiResponse<Object>(true, user, "user found"));
        }

        [HttpGet("by_username")]
        public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
        {
            var User = await userRepository.GetUserByUserNameAsync(username);
            if (User == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "User Not Found"));
            }

            var user = new GetUserResponseDto
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                UserName = User.UserName,
                AvatarUrl = User.AvatarUrl
            };

            return Ok(new ApiResponse<Object>(true, user, "user found"));
        }

        [HttpGet("/IsUserInOrganization")]
        public async Task<IActionResult> IsUserInOrganization([FromQuery] Guid userId, [FromQuery] Guid organizationId)
        {
            var isTrue = await userRepository.IsUserInOrganizationAsync(userId, organizationId);
            return Ok(new ApiResponse<Object>(true, isTrue, "success"));
        }

        [HttpGet("/UsersInOrganization/{organizationId}")]
        public async Task<IActionResult> UsersInOrganization(Guid organizationId)
        {
            var members = await userRepository.GetOrganizationUsersAsync(organizationId);

            var response = members.Select(m => new GetOrganizationMemberResponseDto
            {
                Id = m.Id,
                OrganizationId = m.OrganizationId,
                UserId = m.UserId,
                Email = m.Email,
                Role = m.Role,
                IsAccepted = m.IsAccepted,
                InvitedAt = m.InvitedAt,
                JoinedAt = m.JoinedAt,
                User = m.User != null ? new GetUserResponseDto
                {
                    Id = m.User.Id,
                    FirstName = m.User.FirstName,
                    LastName = m.User.LastName,
                    UserName = m.User.UserName,
                    AvatarUrl = m.User.AvatarUrl
                } : null
            }).ToList();


            return Ok(new ApiResponse<Object>(true, response, "success"));
        }

        [HttpGet("/{Id}/active")]
        public async Task<IActionResult> SetUserActiveStatusAsync([FromQuery] Guid Id, [FromBody] bool isActive)
        {
            var isStatusActive = await userRepository.SetUserActiveStatusAsync(Id, isActive);
            if (isStatusActive == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "User Not Found"));

            }
            return Ok(new ApiResponse<Object>(true, isStatusActive, "success"));

        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] Guid organizationId, [FromQuery] string searchTerm)
        {
            var users = await userRepository.SearchOrganizationUsersAsync(organizationId, searchTerm);
            if (users == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "User Not Found"));

            }
            return Ok(new ApiResponse<Object>(true, users, "Successful"));
        }
    }
}