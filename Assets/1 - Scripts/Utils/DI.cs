using System;
using System.Collections.Generic;

namespace DependencyInjection
{
    public static class DI
    {
        private static Dictionary<Type, object> injections;

        private static Dictionary<Type, object> Injections =>
            injections is null 
            ? injections = new Dictionary<Type, object>() 
            : injections;

        public static void Add<T>(T obj)
        {
            if (!Injections.TryAdd(obj.GetType(), obj))
            {
                throw new Exception($"The object of type {typeof(T)} have been already added to the DI container");
            }
        }

        public static T Get<T>()
        {
            var hasObject = Injections.TryGetValue(typeof(T), out object retVal);

            if (!hasObject)
            {
                throw new Exception($"The object of type {typeof(T)} have not been added to the DI container");
            }

            return (T)retVal; 
        }

        public static bool Has<T>()
        {
            return Injections.ContainsKey(typeof(T));
        }
    }
}