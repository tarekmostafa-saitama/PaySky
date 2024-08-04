using System.Net;

namespace PaySky.Shared.Exceptions;

public class ForbiddenException(string message) : CustomException(message, null, HttpStatusCode.Forbidden);