using System.Net;

namespace PaySky.Shared.Exceptions;

public class UnauthorizedException(string message) : CustomException(message, null, HttpStatusCode.Unauthorized);