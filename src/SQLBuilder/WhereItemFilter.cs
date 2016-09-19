using System.Collections.Generic;

namespace SQLBuilder
{
    public class WhereItemFilter
    {
        public string Condition { get; set; }
        public string Column { get; set; }
        public string Operation { get; set; }
        public List<object> Values { get; set; } = new List<object>();
    }
}
