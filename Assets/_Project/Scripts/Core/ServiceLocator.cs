using System;
using System.Collections.Generic;


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
           
            services.Add(type, service);
        }
        public void UnregisterService<T>()
        {
            var type = typeof(T);
            

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