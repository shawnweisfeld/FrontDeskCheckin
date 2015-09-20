using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FrontDeskCheckinWeb.Data;

namespace FrontDeskCheckinWeb.Controllers
{
    public class TerminalsController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Terminals
        public async Task<ActionResult> Index(string building)
        {
            return View(await db.Terminals.Where(x => x.Building == building).ToListAsync());
        }

        // GET: Terminals/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal terminal = await db.Terminals.FindAsync(id);
            if (terminal == null)
            {
                return HttpNotFound();
            }
            return View(terminal);
        }

        // GET: Terminals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Terminals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Key,SiteName,Building")] Terminal terminal)
        {
            if (ModelState.IsValid)
            {
                db.Terminals.Add(terminal);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(terminal);
        }

        // GET: Terminals/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal terminal = await db.Terminals.FindAsync(id);
            if (terminal == null)
            {
                return HttpNotFound();
            }
            return View(terminal);
        }

        // POST: Terminals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Key,SiteName,Building")] Terminal terminal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(terminal).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { building = terminal.Building });
            }
            return View(terminal);
        }

        // GET: Terminals/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal terminal = await db.Terminals.FindAsync(id);
            if (terminal == null)
            {
                return HttpNotFound();
            }
            return View(terminal);
        }

        // POST: Terminals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Terminal terminal = await db.Terminals.FindAsync(id);
            db.Terminals.Remove(terminal);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
