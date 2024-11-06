using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IJwtService jwtService, IAuthService authService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _authService = authService;
    }

    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        return Ok(_authService.GetProfile());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _userService.RegisterAsync(request.Email, request.Password, request.UserName);
            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.AuthenticateAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized(new { error = "Invalid email or password" });

        var token = _authService.GenerateJwtToken(user);
        return Ok(new { accessToken =token });
    }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }

}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
