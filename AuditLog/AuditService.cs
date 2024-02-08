using Audit.Core;
using Audit.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleAppTry
{
    internal class AuditService : IAuditService, IDisposable    
    {
        private bool disposedValue;

        public Guid TenantId { get; set; }

        public AuditScope? AuditScope { get; set; }

        public AuditService()
        {
            TenantId = Guid.NewGuid(); //Get Tenant ID from Tenant context accessor

            Audit.Core.Configuration.JsonSettings = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
            };
        }

        public void CreateAuditScope<TEntity>(string eventType, TEntity trackingObject, Status status)
        {
            if (trackingObject == null) return;

            var dataProvider = new DynamicDataProvider();
            // Attach an action for insert
            dataProvider.AttachOnInsert(ev => { Console.WriteLine(ev.ToJson().ToString());
                Console.WriteLine("_________________________________________________");
            }); //Use this to publish a message to Service bus when audit data is inserted

            this.AuditScope = AuditScope.Create(new AuditScopeOptions()
            {
                DataProvider = dataProvider,
                EventType = eventType,
                TargetGetter = () => trackingObject,
                CreationPolicy = EventCreationPolicy.Manual,
                ExtraFields = new { 
                    TenantId = TenantId.ToString(),
                    Status = status.ToString() }
            });
        }

        public void SaveAudit()
        {
            if(this.AuditScope!=null)
            {
                this.AuditScope.Save();
                this.AuditScope.Discard();
            }
        }

        public void DiscardAudit()
        {
            if (this.AuditScope != null)
            {
                this.AuditScope.Discard();
            }
        }

        public void DisposeAudit()
        {
            if (this.AuditScope != null)
            {
                this.AuditScope.Dispose();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    this.DisposeAudit();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AuditService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
