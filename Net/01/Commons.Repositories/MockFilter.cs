using System;
using DynamicExpresso;
using System.Collections.Generic;
using Commons.Repository;

namespace Commons.Repositories
{
    class MockFilter 
    {
        public MockFilter()
        {
            Conditions = new List<MockFilter>();
        }
        public List<MockFilter> Conditions { get; set; }
        public Lambda Lambda { get; set; }
        public object Value { get; internal set; }
        public FilterType Type { get; set; }

        public bool Match<T>(T item)
        {
            if (Type != FilterType.And && Type != FilterType.Or)
            {
                return (bool)Lambda.Invoke(item, Value);
            }
            var someVerified = false;
            foreach (var condition in Conditions)
            {
                var conditionResult = condition.Match(item);
                if (Type == FilterType.And && !conditionResult)
                {
                    return false;
                }
                someVerified = someVerified || conditionResult;
            }
            return someVerified;
        }
    }
}
