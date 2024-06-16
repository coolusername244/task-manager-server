using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Interfaces
{
  public interface ITokenService
  {
    string CreateToken(AppUser user);
  }
}