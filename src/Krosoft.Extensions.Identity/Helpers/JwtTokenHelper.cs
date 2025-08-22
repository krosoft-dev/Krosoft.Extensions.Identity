using System.IdentityModel.Tokens.Jwt;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Identity.Helpers;

public static class JwtTokenHelper
{
    public static Result<bool> IsTokenExpired(string? jwtToken)
    {
        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            return Result<bool>.Failure(new KrosoftTechnicalException("JWT Token non renseigné."));
        }

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(jwtToken))
        {
            return Result<bool>.Failure(new KrosoftTechnicalException("JWT Token illisible."));
        }

        var token = handler.ReadJwtToken(jwtToken);

        // La date d'expiration du token est en UTC.
        var expiration = token.ValidTo;

        var isExpired = expiration <= DateTimeOffset.UtcNow;

        if (isExpired)
        {
            return Result<bool>.Failure(new KrosoftTechnicalException($"Le JWT token a expiré au {expiration}."));
        }

        return Result<bool>.Success(false);
    }
}