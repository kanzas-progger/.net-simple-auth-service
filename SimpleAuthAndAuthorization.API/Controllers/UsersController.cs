using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthAndAuthorization.API.Contracts;
using SimpleAuthAndAuthorization.API.Contracts.Responses;
using SimpleAuthAndAuthorization.Core.Abstractions;
using SimpleAuthAndAuthorization.Core.Models;

namespace SimpleAuthAndAuthorization.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserRequest request)
    {
        if (request.password != request.confirmPassword)
        {
            return BadRequest("Passwords do not match");
        }

        string token = await _usersService.Register(request.login, request.password,
            request.firstname, request.surname, request.age, request.role);
        
        //Add token to coockies
        Response.Cookies.Append("coockies", token);
        
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest request)
    {
        string token = await _usersService.Login(request.login, request.password);
        
        //Add token to coockies
        Response.Cookies.Append("coockies", token);
        
        return Ok(token);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        Response.Cookies.Delete("coockies");
        return Ok("Logged out");
    }
    
    [HttpGet("allUsers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserResponse>>> GetAll()
    {
        var users = await _usersService.GetAllUsers();
        var response = users.Select(u => new UserResponse(u.Id, u.Login, u.Firstname, u.Surname, 
            u.Fathername, u.Age, u.Email, u.Phone));
        
        return Ok(response);
    }

    [HttpGet("{login}")]
    [Authorize(Roles = "Owner,Sitter,Admin")]
    public async Task<ActionResult<GetUserProfileResponse>> GetProfile(string login)
    {
        var user = await _usersService.GetUserByLogin(login);
        if (user == null)
            return NotFound("User not found");
        GetUserProfileResponse response = new GetUserProfileResponse(user.Login, user.Firstname, user.Surname,
            user.Fathername, user.Age, user.Email, user.Phone);
        
        return Ok(response);
    }

    [HttpPost("addAdminRole")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddAdminRoleToUser(Guid id)
    {
        return Ok(await _usersService.AddAdminRoleToUser(id));
    }
    
    [HttpPut("profile/edit")]
    [Authorize(Roles = "Owner,Sitter")]
    public async Task<ActionResult<Guid>> UpdateProfile([FromBody] UpdateUserRequest request)
    {
        var userId = GetUserIdFromToken();
        var response = await _usersService.UpdateUserProfile(userId, request.firstname,
            request.surname, request.fathername, request.email, request.phone);
        
        return Ok(response);
    }

    [HttpPut("profile/security/edit")]
    [Authorize(Roles = "Owner,Sitter,Admin")]
    public async Task<ActionResult> ChangePassword([FromBody] UpdateUserPasswordRequest request)
    {
        if (request.password != request.confirmPassword)
            return BadRequest("Passwords do not match");
        var userId = GetUserIdFromToken();
        await _usersService.ChangeUserPassword(userId, request.password);

        return Ok();
    }

    [HttpDelete("deleteUser/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        return Ok(await _usersService.DeleteUser(id));
    }

    private Guid GetUserIdFromToken()
    {
        var userIdFromToken = User.FindFirst("userId-")?.Value;
        if (userIdFromToken == null || !Guid.TryParse(userIdFromToken, out Guid userId))
            throw new UnauthorizedAccessException("Invalid response, token not found");

        return userId;
    }
    
}