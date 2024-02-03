using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTry
{
    internal interface IAuditService
    {
        void CreateAuditScope<TEntity>(string eventType, TEntity trackingObject, Status status);
        //void LogAudit(string eventType, Status message);
        void SaveAudit();
        void DiscardAudit();
        void DisposeAudit();
    }
}
