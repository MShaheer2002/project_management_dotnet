using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.User;
using ProjectManagementBackend.Application.Dto.auth;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtService jwtService;

        public AuthController(IUserRepository userRepository, IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "Email not found"));
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return NotFound(new ApiResponse<Object>(false, "", "Invalid credentials"));

            }

            var token = jwtService.GenerateToken(user.Id, user.Email);

            var loginUser = new LoginResponseDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                JoinedAt = user.CreatedAt.ToString(),
                AvatarUrl = user.AvatarUrl,
            };

            var data = new
            {
                Token = token,
                user = loginUser,
            };

            return Ok(new ApiResponse<Object>(true, data, "Successful Login"));
        }

        [HttpPost("Register-with-email")]
        public async Task<IActionResult> RegisterWithEmail(CreateUserRequestDto createUserDto)
        {
            var hasedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var newUser = new User(createUserDto.FirstName, createUserDto.LastName, createUserDto.Email, createUserDto.UserName, hasedPassword, createUserDto.AvatarUrl);

            await userRepository.CreateUserAsync(newUser);

            var user = new CreateUserResponseDto
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName ?? "",
                UserName = newUser.UserName,
                Email = newUser.Email,
                AvatarUrl = newUser.AvatarUrl ?? "",

            };

            var response = new ApiResponse<Object>(true, user, "new user created");

            return Ok(response);
        }

    }
}