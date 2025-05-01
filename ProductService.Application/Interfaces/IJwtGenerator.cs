using ProductService.Domain.Entities;
using System.Collections.Generic;

namespace ProductService.Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
} 
