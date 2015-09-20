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

            vm.Buildings = await db.Visitors
                .GroupBy(x => x.Terminal.Building)
                .Select(x => new BuildingDetail()
                {
                    Name = x.Key,
                    TotalVisitors = x.Count(),
                    FirstVisitor = x.Min(y => y.ArrivedAt),
                    LastVisitor = x.Max(y => y.ArrivedAt)
                })
                .ToListAsync();

            foreach (var building in vm.Buildings)
            {
                var buildingOpen = building.LastVisitor.Subtract(building.FirstVisitor);
                building.AverageDailyVisitors = building.TotalVisitors / buildingOpen.TotalDays;
                building.AverageMonthlyVisitors = building.TotalVisitors / (buildingOpen.TotalDays / 30);
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