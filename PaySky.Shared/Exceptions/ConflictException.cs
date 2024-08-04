using System.Net;

namespace PaySky.Shared.Exceptions;

public class ConflictException(string message) : CustomException(message, null, HttpStatusCode.Conflict);