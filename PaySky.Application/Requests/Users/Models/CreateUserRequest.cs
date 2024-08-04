﻿using PaySky.Domain.Enums;

namespace PaySky.Application.Requests.Users.Models;

public class CreateUserRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public UserType UserType { get; set; }
}