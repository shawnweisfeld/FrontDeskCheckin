using FrontDeskCheckinWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrontDeskCheckinWeb.ViewModels.Home;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FrontDeskCheckinWeb.Controllers
{
    public class HomeController : Controller
    {
        private AppContext db = new AppContext();

        public async Task<ActionResult> Index()
        {
            var vm = new Index();

            vm.Buildings = await db.Terminals
                .GroupBy(x => x.Building)
                .Select(x => new BuildingDetail()
                {
                    Name = x.Key,
                    TotalVisitors = x.SelectMany(y => y.Visitors).Count(),
                    Terminals = x.Count(),
                    FirstVisitor = x.SelectMany(y => y.Visitors).Min(y => y.ArrivedAt),
                    LastVisitor = x.SelectMany(y => y.Visitors).Max(y => y.ArrivedAt),
                })
                .ToListAsync();

            foreach (var building in vm.Buildings)
            {
                if (building.FirstVisitor.HasValue && building.LastVisitor.HasValue)
                {
                    var buildingOpen = building.LastVisitor.Value.Subtract(building.FirstVisitor.Value);
                    building.AverageDailyVisitors = building.TotalVisitors / buildingOpen.TotalDays;
                    building.AverageMonthlyVisitors = building.TotalVisitors / (buildingOpen.TotalDays / 30);
                }
            }

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}