using System.Net;
using PaySky.Shared.Exceptions;

namespace PaySky.Domain.Exceptions;

public class DomainException(string message) : CustomException(message, new List<string>(), HttpStatusCode.BadRequest);