﻿namespace PaySky.Application.Requests.Authentication.Models;

public class AuthenticateResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}