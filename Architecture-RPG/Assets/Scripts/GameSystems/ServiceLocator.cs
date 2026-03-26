using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();
    
    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }
    
    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out object service))
            return (T)service;
        throw new Exception($"Service {typeof(T).Name} not found.");
    }
    
    public static void Clear() => _services.Clear();
}
