using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskCheckinWeb.ViewModels.Home
{
    public class Index
    {
        public List<BuildingDetail> Buildings { get; set; }
    }

    public class BuildingDetail
    {
        public string Name { get; set; }
        public int TotalVisitors { get; set; }
        public double AverageDailyVisitors { get; set; }
        public double AverageMonthlyVisitors { get; set; }
        public int Terminals { get; set; }
        public DateTime? FirstVisitor { get; set; }
        public DateTime? LastVisitor { get; set; }
    }
}
