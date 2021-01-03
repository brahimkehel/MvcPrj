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
    public class UtilisateursController : Controller
    {
        private NavetteDBEntities db = new NavetteDBEntities();


        public ActionResult sedeconncter()
        {
            Session["UsrSession"] = null;
            return RedirectToAction("Login", "Utilisateurs");
        }
        //GET: Login
        public ActionResult Login(string error)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            
                return View();

        }

        public async Task<ActionResult> SeConnecter(Utilisateur model)
        {
                Utilisateur query = (from utilisateur in db.Utilisateurs where utilisateur.email == model.email && utilisateur.motDePasse == model.motDePasse select utilisateur).FirstOrDefault();
                if (query != null)
                {
                    Session["UsrSession"] = query;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login");
                }
        }

        // GET: Utilisateurs
        public async Task<ActionResult> Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            /*var utilisateurs = db.Utilisateurs.Include(u => u.Administrateur).Include(u => u.Client).Include(u => u.Societe);
            return View(await utilisateurs.ToListAsync());*/
            return View();
        }

        public async Task<ActionResult> ClientsList()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            var utilisateurs = from u in db.Utilisateurs where u.role_ == "client" select u;
            return View(await utilisateurs.ToListAsync());
        }

        public async Task<ActionResult> SocietesList()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            var utilisateurs = db.Utilisateurs.Where(d => d.role_=="societe");
            return View(await utilisateurs.ToListAsync());
        }

        // GET: Utilisateurs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.Administrateurs, "id", "id");
            ViewBag.id = new SelectList(db.Clients, "id", "id");
            ViewBag.id = new SelectList(db.Societes, "id", "id");
            return View();
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nom,prenom,email,motDePasse,telephone,role_")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Utilisateurs.Add(utilisateur);
                if (utilisateur.role_ == "client")
                {
                    db.Clients.Add(new Client {id= utilisateur.id });
                }
                else if(utilisateur.role_ == "societe")
                {
                    db.Societes.Add(new Societe { id = utilisateur.id });
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Login","Utilisateurs");
            }

            ViewBag.id = new SelectList(db.Administrateurs, "id", "id", utilisateur.id);
            ViewBag.id = new SelectList(db.Clients, "id", "id", utilisateur.id);
            ViewBag.id = new SelectList(db.Societes, "id", "id", utilisateur.id);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.Administrateurs, "id", "id", utilisateur.id);
            ViewBag.id = new SelectList(db.Clients, "id", "id", utilisateur.id);
            ViewBag.id = new SelectList(db.Societes, "id", "id", utilisateur.id);
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nom,prenom,email,motDePasse,telephone,role_")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "Utilisateurs");
            }
            ViewBag.id = new SelectList(db.Administrateurs, "id", "id", utilisateur.id);
            ViewBag.id = new SelectList(db.Clients, "id", "id", utilisateur.id);
            ViewBag.id = new SelectList(db.Societes, "id", "id", utilisateur.id);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur.role_ == "client")
            {
                Client c= await db.Clients.FindAsync(id);
                db.Clients.Remove(c);
                db.Utilisateurs.Remove(utilisateur);
                await db.SaveChangesAsync();
                return RedirectToAction("ClientsList", "Utilisateurs");
            }
            else if (utilisateur.role_ == "societe")
            {
                Societe s = await db.Societes.FindAsync(id);
                db.Societes.Remove(s);
                db.Utilisateurs.Remove(utilisateur);
                await db.SaveChangesAsync();
                return RedirectToAction("SocietesList", "Utilisateurs");
            }
            else
            {
                return HttpNotFound();
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
