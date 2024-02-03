using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTry
{
    public class ChurnApiKey
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid ApiKey { get; set; }

        public string ApiKeyHash { get; set; }

        public bool IsActive { get; set; }
    }
}
