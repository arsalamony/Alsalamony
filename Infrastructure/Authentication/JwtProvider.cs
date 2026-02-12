using Alsalamony.Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace Alsalamony.Infrastructure.Authentication;
public class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _jwtOptions;
	public JwtProvider(IOptions<JwtOptions> jwtOptions)
	{
		_jwtOptions = jwtOptions.Value;
	}

	public (string token, int expiresIn) GenerateToken(User user)
	{

        var claims = new[]
		{
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.key));

		var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

		var expirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes);

		var token = new JwtSecurityToken(
			issuer: _jwtOptions.Issuer,
			audience: _jwtOptions.Audience,
			claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
			signingCredentials: signingCredentials
		);

		return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _jwtOptions.ExpiryMinutes * 60);
	}

	public int? ValidateToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.key));

		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				IssuerSigningKey = symmetricSecurityKey,
				ValidateIssuerSigningKey = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero

			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;

			var t = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;


            return t is not null? Convert.ToInt32(t):null;
		}
		catch
		{
			return null;
		}

	}
}