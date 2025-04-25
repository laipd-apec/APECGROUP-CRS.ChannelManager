
using CRS.ChannelManager.Library.EnumType;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseEntities
{
    public class AuditEntry
    {
        //public AuditEntry(EntityEntry entry)
        //{
        //    Entry = entry;
        //}

        public AuditEntry() { }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        //public EntityEntry Entry { get; }
        public string UserName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> Data { get; } = new Dictionary<string, object>();

        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public AuditEntity ToAudit()
        {
            var audit = new AuditEntity();
            audit.UserName = UserName;
            audit.Type = AuditType.ToString();
            audit.TableName = TableName;
            audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.Data = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(Data);
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            audit.CreatedDate = DateTime.Now;
            audit.CreatedBy = UserName;
            return audit;
        }
    }
}
