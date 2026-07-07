using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace _Project.Scripts.Core
{
    public class ServiceLocator
    {
        private static ServiceLocator instance;
        public static ServiceLocator Instance => instance ??= new ServiceLocator();

        private readonly Dictionary<Type, object> services;
        
        private ServiceLocator()
        {
            services = new Dictionary<Type, object>();
        }
        public void RegisterService<T>(T service)
        {
            var type = typeof(T);
            Assert.IsFalse(services.ContainsKey(type), 
                $"Service {type} already registered");
            services.Add(type, service);
        }
        public void UnregisterService<T>()
        {
            var type = typeof(T);
            Assert.IsTrue(services.ContainsKey(type),
                $"Service {type} is not registered");

            services.Remove(type);
        }
        

       
        public bool Contains<T>()
        {
            var type = typeof(T);
            return services.ContainsKey(type);
        }

        public T GetService<T>()
        {
            var type = typeof(T);
            if (!services.TryGetValue(type, out var service))
            {
                throw new Exception($"Service {type} not found");
            }
            return (T)service;
        }
        
    }
}