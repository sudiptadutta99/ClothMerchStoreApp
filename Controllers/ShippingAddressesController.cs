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
    public class ShippingAddressesController : Controller
    {
        private ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities();

        // GET: ShippingAddresses
        public async Task<ActionResult> Index()
        {
            var shippingAddresses = db.ShippingAddresses.Include(s => s.User);
            return View(await shippingAddresses.ToListAsync());
        }

        // GET: ShippingAddresses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAddress shippingAddress = await db.ShippingAddresses.FindAsync(id);
            if (shippingAddress == null)
            {
                return HttpNotFound();
            }
            return View(shippingAddress);
        }

        // GET: ShippingAddresses/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");
            return View();
        }

        // POST: ShippingAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AddressID,UserID,AddressLine,City,State,PostalCode,Country")] ShippingAddress shippingAddress)
        {
            if (ModelState.IsValid)
            {
                db.ShippingAddresses.Add(shippingAddress);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", shippingAddress.UserID);
            return View(shippingAddress);
        }

        // GET: ShippingAddresses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAddress shippingAddress = await db.ShippingAddresses.FindAsync(id);
            if (shippingAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", shippingAddress.UserID);
            return View(shippingAddress);
        }

        // POST: ShippingAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AddressID,UserID,AddressLine,City,State,PostalCode,Country")] ShippingAddress shippingAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingAddress).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", shippingAddress.UserID);
            return View(shippingAddress);
        }

        // GET: ShippingAddresses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingAddress shippingAddress = await db.ShippingAddresses.FindAsync(id);
            if (shippingAddress == null)
            {
                return HttpNotFound();
            }
            return View(shippingAddress);
        }

        // POST: ShippingAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ShippingAddress shippingAddress = await db.ShippingAddresses.FindAsync(id);
            db.ShippingAddresses.Remove(shippingAddress);
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
