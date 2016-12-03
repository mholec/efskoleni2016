using System.Collections.Generic;

namespace Skoleni.Extras
{
    public class AuditItem
    {
        public string Operation { get; set; }
        public string ObjectType { get; set; }
        public Dictionary<string, object> Original { get; set; }
        public Dictionary<string, object> Current { get; set; }
    }
}