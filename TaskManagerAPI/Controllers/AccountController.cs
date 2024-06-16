using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly DataContext _context;

    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
      _tokenService = tokenService;
      _context = context;
    }

    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {

      if (await CheckUserExists(registerDto.Username)) return BadRequest("Username is taken");

      using var hmac = new HMACSHA512();

      var user = new AppUser
      {
        Username = registerDto.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        PasswordSalt = hmac.Key
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return new UserDto
      {
        Username = user.Username,
        Token = _tokenService.CreateToken(user)

      };
    }

    [HttpPost("login")] // api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == loginDto.Username);

      if (user == null) return Unauthorized("Invalid username");

      using var hmac = new HMACSHA512(user.PasswordSalt);

      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

      for (int i = 0; i < computedHash.Length; i++)
      {
        if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
      }

      return new UserDto
      {
        Username = user.Username,
        Token = _tokenService.CreateToken(user)
      };
    }

    private async Task<bool> CheckUserExists(string username)
    {
      return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
    }
  }
}