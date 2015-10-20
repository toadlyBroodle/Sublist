using System.Collections.Generic;
using System.Linq;

namespace Sublist.Providers.Container
{
    public class ProviderContainer
    {
        private static readonly Dictionary<string, object> Providers = new Dictionary<string, object>();

        protected ProviderContainer()
        {
            // The provider can only be instantiated by itself or its inheritants
        }

        public static T GetInstance<T>()
        {
            var interfaceName = typeof(T).FullName;
            var entry = Providers.FirstOrDefault(d => d.Key == interfaceName);
            if (entry.Value == null)
            {
                throw new KeyNotFoundException("Provider is not instantiated.");
            }
            return (T)entry.Value;
        }

        public static void Register<T>(T implementation)
        {
            var interfaceName = typeof(T).FullName;
            Providers.Add(interfaceName, implementation);
        }
    }
}