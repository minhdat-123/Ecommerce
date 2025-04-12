using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.Extensions.Configuration; // Required for reading config
using Microsoft.IdentityModel.Tokens; // Required for SecurityKey, SigningCredentials
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt; // Required for JwtSecurityTokenHandler
using System.Security.Claims; // Required for Claim
using System.Text; // Required for Encoding

namespace Ecommerce.Infrastructure.Services;

public class JwtGenerator : IJwtGenerator
{
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;

    // Inject IConfiguration to read JWT settings
    public JwtGenerator(IConfiguration config)
    {
        var secretKey = config["Jwt:Key"];
        _issuer = config["Jwt:Issuer"];
        _audience = config["Jwt:Audience"];

        if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(_issuer) || string.IsNullOrEmpty(_audience))
        {
            throw new ArgumentNullException("JWT Key, Issuer or Audience is missing in configuration for JwtGenerator.");
        }
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }

    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id), // Unique User ID
            new Claim(JwtRegisteredClaimNames.Email, user.Email), // User Email
            // Add other claims as needed (e.g., name)
            // new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName ?? string.Empty)
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Token expiry (e.g., 1 hour)
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
} 