using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.User;
using project_management_backend.Infrastructure.Persistence;

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

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDto createUserDto)
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

            var response = new ApiResponse<Object>(true,  user , "new user created");

            return Ok(response);

        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(Guid Id, [FromBody] UpdateRequestDto updateRequestDto)
        {

            var updatedUser = await userRepository.UpdateUserAsync(Id, updateRequestDto);

            if (updatedUser == null)
            {
                return NotFound(new ApiResponse<object>(false, null, "User not found"));
            }

            var user = new UpdateResponseDto
            {
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                UserName = updatedUser.UserName,
                AvatarUrl = updatedUser.AvatarUrl

            };

            var responseUser = new ApiResponse<Object>(true,  user , "Data updated");

            return Ok(responseUser);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var deletedUser = await userRepository.DeleteUserAsync(Id);

            if (deletedUser == null)
                return  NotFound(new ApiResponse<object>(false, null, "User not found"));


            var user = new DeleteUserDto
            {
                FirstName = deletedUser.FirstName,
                LastName = deletedUser.LastName,
                UserName = deletedUser.UserName,
                AvatarUrl = deletedUser.AvatarUrl
            };

            var response = new ApiResponse<Object>(true, user , "User deleted");
            return Ok(response);

        }
    }
}