using System;

namespace Planner.UI.Model
{
    public class Day
    {
        public DateTime Date { get; set; }
        public bool IsTargetMonth { get; set; }
        public bool IsToday { get; set; }
    }
}
