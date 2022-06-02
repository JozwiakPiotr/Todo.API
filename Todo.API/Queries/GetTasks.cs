using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.API.Queries
{
    public class GetTasks
    {
        public Guid UserId { get; set; }
        public List<string> Categories { get; set; } = new();
        public DateTime PoitingDate { get; set; }
        public TimePeriod TimePeriod { get; set; }
    }

    public enum TimePeriod
    {
        Month
    }
}