using ClothMerchStoreApp.Models;
using System;
using System.Linq;
using System.Web.Mvc;
//for all customer actions (add, remove, checkout)
namespace ClothMerchStoreApp.Controllers
{
    public class CartController : Controller
    {
        ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities();

        // View Cart
        // if (Session["UserID"] == null)
        //     return RedirectToAction("Login", "Account");

        // hardcode a test UserID temporarily

        public ActionResult Index()
        {
            //int UserID = 1;
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserID"];
            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();
            return View(cartItems);
        }

        // Add to Cart
        public ActionResult AddToCart(int id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserID"];

            var existing = db.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == id);
            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                Cart item = new Cart
                {
                    UserID = userId,
                    ProductID = id,
                    Quantity = 1
                };
                db.Carts.Add(item);
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }

        // Remove Item
        public ActionResult Remove(int id)
        {
            var cartItem = db.Carts.Find(id);
            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserID"];
            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();

            if (!cartItems.Any())
                return RedirectToAction("Index");

            decimal totalAmount = 0;
            foreach (var item in cartItems)
            {
                totalAmount += (item.Quantity.GetValueOrDefault() * (item.Product.Price ?? 0)); // ✅
            }

            ////cart count 
            //Session["CartCount"] = db.Carts.Where(c => c.UserID == currentUserID).Sum(c => c.Quantity);


            // Create Order
            Order order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Placed"
            };

            db.Orders.Add(order);
            db.SaveChanges();

            // Create OrderItems
            foreach (var item in cartItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                db.OrderItems.Add(orderItem);
            }

            // Clear Cart
            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();

            ViewBag.Message = "Order placed successfully!";
            return RedirectToAction("OrderSuccess");
        }

    }
}
