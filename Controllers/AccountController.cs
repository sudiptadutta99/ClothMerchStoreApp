using System.Linq;
using System.Web.Mvc;
using ClothMerchStoreApp.Models;

namespace ClothMerchStoreApp.Controllers
{
    public class AccountController : Controller
    {
        ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities();

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existing == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    ViewBag.Message = "Registration successful!";
                    return RedirectToAction("Login");
                }
                ViewBag.Message = "Email already exists!";
            }
            return View(user);
        }

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["FullName"] = user.FullName;
                return RedirectToAction("Index", "Products");
            }
            ViewBag.Message = "Invalid credentials!";
            return View();
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult MyOrders()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int userId = (int)Session["UserID"];
            var orders = db.Orders.Where(o => o.UserID == userId).ToList();
            return View(orders);
        }

    }
}
