﻿namespace PaySky.Application.Requests.Authentication.Models;

public class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}