using FrontDeskCheckinWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlTypes;

namespace FrontDeskCheckinWeb.Controllers
{
    public class ApiController : Controller
    {
        private AppContext db = new AppContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public async Task<ActionResult> AddVisitor(Visitor visitor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    visitor.DepartedAt = SqlDateTime.MinValue.Value;
                    visitor.Terminal = await db.Terminals.FindAsync(visitor.Terminal.Id);
                    db.Visitors.Add(visitor);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<ActionResult> CheckoutVisitor(Visitor visitor)
        {
            try
            {
                var tmp = await db.Visitors.FirstOrDefaultAsync(x => x.Id == visitor.Id);
                if (tmp != null)
                {
                    tmp.DepartedAt = visitor.DepartedAt;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<ActionResult> GetVisitors(string id)
        {
            var today = DateTime.Now.Date;
            var sqlMinDate = SqlDateTime.MinValue.Value;
            try
            {
                var result = new JsonResult();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                result.Data = await db.Visitors
                    .Where(x => x.Terminal.Key == id)
                    .Where(x => x.ArrivedAt > today)
                    .Where (x => x.DepartedAt == sqlMinDate)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        [HttpGet]
        public async Task<ActionResult> GetTerminal(string id)
        {
            try
            {
                var terminal = await db.Terminals.FirstOrDefaultAsync(x => x.Key == id);

                if (terminal == null)
                {
                    terminal = new Terminal()
                    {
                        Key = id,
                        Building = "TBD",
                        SiteName = "TBD"
                    };
                    db.Terminals.Add(terminal);
                    await db.SaveChangesAsync();
                }


                var result = new JsonResult();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                result.Data = terminal;

                return result;
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

    }


}