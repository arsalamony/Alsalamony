using Domain.Entities;

namespace Alsalamony.Application.Common.Interfaces;
public interface IJwtProvider
{
	(string token, int expiresIn) GenerateToken(User user);
	int? ValidateToken(string token);

}
