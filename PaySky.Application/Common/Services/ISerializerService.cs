﻿using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Common.Services;

public interface ISerializerService : ISingletonService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}