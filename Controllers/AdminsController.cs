using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothMerchStoreApp.Models;

namespace ClothMerchStoreApp.Controllers
{
    public class AdminsController : Controller
    {
        private ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities();

        // GET: Admins
        public async Task<ActionResult> Index()
        {
            return View(await db.Admins.ToListAsync());
        }

        // GET: Admins/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = await db.Admins.FindAsync(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AdminID,Username,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = await db.Admins.FindAsync(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdminID,Username,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = await db.Admins.FindAsync(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Admin admin = await db.Admins.FindAsync(id);
            db.Admins.Remove(admin);
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities())
            {
                var admin = db.Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
                if (admin != null)
                {
                    Session["AdminID"] = admin.AdminID;
                    Session["AdminUser"] = admin.Username;
                    return RedirectToAction("Dashboard");
                }
                ViewBag.Message = "Invalid credentials!";
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            if (Session["AdminID"] == null)
                return RedirectToAction("Login");

            using (ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities())
            {
                ViewBag.TotalProducts = db.Products.Count();
                ViewBag.TotalOrders = db.Orders.Count();
                ViewBag.TotalUsers = db.Users.Count();
                return View();
            }
        }

        public ActionResult Orders()
        {
            if (Session["AdminID"] == null)
                return RedirectToAction("Login");

            using (ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities())
            {
                var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
                return View(orders);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }



    }
}
