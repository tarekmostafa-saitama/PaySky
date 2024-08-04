using System.Net;

namespace PaySky.Shared.Exceptions;

public class NotFoundException(string message) : CustomException(message, null, HttpStatusCode.NotFound);