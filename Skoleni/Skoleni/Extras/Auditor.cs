using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Skoleni.Extras
{
    public class Auditor
    {
        private readonly DbChangeTracker _changeTracker;

        public Auditor(DbChangeTracker changeTracker)
        {
            _changeTracker = changeTracker;
        }

        public List<AuditItem> AuditChanges(bool detectChangesEnabled = true)
        {
            DetectChanges(detectChangesEnabled);

            if (!_changeTracker.HasChanges())
            {
                return new List<AuditItem>(0);
            }

            var auditItems = new List<AuditItem>();
            var entries = _changeTracker.Entries().ToList();
            foreach (var dbEntityEntry in entries)
            {
                var properties = dbEntityEntry.CurrentValues.PropertyNames.ToList();

                Dictionary<string, object> currentValues = new Dictionary<string, object>();
                Dictionary<string, object> originalValues = new Dictionary<string, object>();
                foreach (var property in properties)
                {
                    currentValues.Add(property, dbEntityEntry.CurrentValues.GetValue<object>(property));
                    originalValues.Add(property, dbEntityEntry.OriginalValues.GetValue<object>(property));
                }

                auditItems.Add(new AuditItem
                {
                    ObjectType = dbEntityEntry.Entity.GetType().Name.Split('_')[0],
                    Operation = dbEntityEntry.State.ToString(),
                    Original = originalValues,
                    Current = currentValues
                });
            }

            return auditItems;
        }

        private void DetectChanges(bool detectChanges)
        {
            if (detectChanges)
            {
                _changeTracker.DetectChanges();
            }
        }
    }
}