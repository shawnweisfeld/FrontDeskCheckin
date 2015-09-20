using FrontDeskCheckinWeb.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlTypes;

namespace FrontDeskCheckinWeb.Controllers
{
    public class ExportController : Controller
    {
        private AppContext db = new AppContext();

        public async Task<ActionResult> Index()
        {
            var buildings = await db.Terminals
                .Select(x => x.Building)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            var vm = new ViewModels.Export.Index();

            vm.Buildings = buildings.Select(x => new SelectListItem()
            {
                Text = x,
                Value = x
            }).ToList();

            return View(vm);        
        }

        [HttpPost]
        public async Task<ActionResult> Excel(DateTime reportdate, string building)
        {
            string templatePath = Server.MapPath("~/App_Data/VMS_BulkVisitImport_v7_template.xlsx");
            var filename = string.Format("VMS_BulkVisitImport_{0}_{1:yyyy-MM-dd}.xlsx", building, reportdate);
            var row = 1;
            var col = 1;
            var startDate = reportdate.Date;
            var endDate = startDate.AddDays(1);
            var sqlMinDate = SqlDateTime.MinValue.Value;
            var visitors = await db.Visitors
                .Where(x => x.ArrivedAt > startDate)
                .Where(x => x.ArrivedAt < endDate)
                .Where(x => x.Terminal.Building == building)
                .OrderBy(x => x.ArrivedAt)
                .Include(x => x.Terminal)
                .ToListAsync();

            using (var ms = new MemoryStream())
            using (var package = new ExcelPackage(new FileInfo(templatePath)))
            {
                var sheet = package.Workbook.Worksheets.First();

                foreach (var visitor in visitors)
                {
                    row++;
                    col = 1;
                    sheet.Cells[row, col++].Value = visitor.FirstName;
                    sheet.Cells[row, col++].Value = visitor.LastName;
                    sheet.Cells[row, col++].Value = visitor.ArrivedAt.ToString("HH:mm");
                    sheet.Cells[row, col++].Value = visitor.ArrivedAt.ToShortDateString();

                    if (visitor.DepartedAt > visitor.ArrivedAt)
                    {
                        sheet.Cells[row, col++].Value = visitor.DepartedAt.ToString("HH:mm");
                        sheet.Cells[row, col++].Value = visitor.DepartedAt.ToShortDateString();
                    }
                    else
                    {
                        col += 2;
                    }

                    sheet.Cells[row, col++].Value = visitor.Sponsor;
                    sheet.Cells[row, col++].Value = visitor.Terminal.Building;
                    sheet.Cells[row, col++].Value = visitor.Company;
                }

                package.SaveAs(ms);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
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