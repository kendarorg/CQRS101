using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Repository
{
    public enum FilterType
    {
        And = 10,
        Or = 11,
        Equal = 0,
        Greater = 1,
        Lower = 2,
        GreaterEqual = 3,
        LowerEqual = 4,
        Contains = 5
    }
    public class Filter 
    {
        public Filter()
        {
            Type = FilterType.Equal;
            Conditions = new List<Filter>();
        }
        public FilterType Type { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public List<Filter> Conditions { get; set; }

    }
}
