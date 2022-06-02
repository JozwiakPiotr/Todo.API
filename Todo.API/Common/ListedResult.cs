using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.API.Queries;

namespace Todo.API.Common
{
    public class ListedResult<T>
    {
        public ListedResult(List<T> objects, TimePeriod timePeriod, DateTime poitingDate)
        {
            Objects = objects;
            TimePeriod = timePeriod;
            PoitingDate = poitingDate.Date;
        }

        public TimePeriod TimePeriod { get; set; }
        public DateTime PoitingDate { get; set; }
        public List<T> Objects { get; set; }
    }
}