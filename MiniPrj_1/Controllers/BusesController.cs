using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiniPrj_1.Models;

namespace MiniPrj_1.Controllers
{
    public class BusesController : Controller
    {
        private NavetteDBEntities db = new NavetteDBEntities();

        // GET: Buses
        public async Task<ActionResult> Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            var buses = db.Buses.Include(b => b.Societe).Include(b => b.Trajet);
            return View(await buses.ToListAsync());
        }

        // GET: Buses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = await db.Buses.FindAsync(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // GET: Buses/Create
        public ActionResult Create()
        {
            string help = "";
            List<SelectListItem> trajets = new List<SelectListItem>();
            ViewBag.UsrSession = Session["UsrSession"];
            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
            foreach(var t  in db.Trajets)
            {
                help = t.aller + " => " + t.retour;
                trajets.Add(new  SelectListItem{Text=help,Value= t.id.ToString() });
            }
            ViewBag.idTrajet = trajets;
            return View();
        }

        // POST: Buses/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,typeDeVehicule,description_,idSociete,idTrajet")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Buses.Add(bus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial", bus.idSociete);
            ViewBag.idTrajet = new SelectList(db.Trajets, "id", "aller", bus.idTrajet);
            return View(bus);
        }

        // GET: Buses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = await db.Buses.FindAsync(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial", bus.idSociete);
            string help = "";
            List<SelectListItem> trajets = new List<SelectListItem>();
            ViewBag.UsrSession = Session["UsrSession"];
            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
            foreach (var t in db.Trajets)
            {
                help = t.aller + " => " + t.retour;
                trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
            }
            ViewBag.idTrajet = trajets;
            return View(bus);
        }

        // POST: Buses/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,typeDeVehicule,description_,idSociete,idTrajet")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial", bus.idSociete);
            string help = "";
            List<SelectListItem> trajets = new List<SelectListItem>();
            ViewBag.UsrSession = Session["UsrSession"];
            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
            foreach (var t in db.Trajets)
            {
                help = t.aller + " => " + t.retour;
                trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
            }
            ViewBag.idTrajet = trajets;
            return View(bus);
        }

        // GET: Buses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = await db.Buses.FindAsync(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // POST: Buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Bus bus = await db.Buses.FindAsync(id);
            db.Buses.Remove(bus);
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
