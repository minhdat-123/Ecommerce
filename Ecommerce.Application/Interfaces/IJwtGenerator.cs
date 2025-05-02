using Ecommerce.Domain.Entities;
using System.Collections.Generic;

namespace Ecommerce.Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
} 