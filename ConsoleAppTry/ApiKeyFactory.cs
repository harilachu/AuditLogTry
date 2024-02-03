using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTry
{
    internal class ApiKeyFactory
    {
        const string ApiKeyName = "APIKey_";

        internal static List<ChurnApiKey> CreateApiKeys()
        {
            int i = 1;
            return new List<ChurnApiKey> { 
                new ChurnApiKey(){ Id = 1, Name=ApiKeyName+i++, ApiKey= Guid.NewGuid(), IsActive=true },
                new ChurnApiKey(){ Id = 1, Name=ApiKeyName+i++, ApiKey= Guid.NewGuid(), IsActive=true },
                new ChurnApiKey(){ Id = 1, Name=ApiKeyName+i++, ApiKey= Guid.NewGuid(), IsActive=true },
                new ChurnApiKey(){ Id = 1, Name=ApiKeyName+i++, ApiKey= Guid.NewGuid(), IsActive=true },

            };
        }
    }
}
