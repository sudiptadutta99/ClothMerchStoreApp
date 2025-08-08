using System.Linq;
using System.Web.Mvc;
using ClothMerchStoreApp.Models;

namespace ClothMerchStoreApp.Controllers
{
    public class WishlistController : Controller
    {
        ClothMerchStoreDBEntities db = new ClothMerchStoreDBEntities();

        // GET: Wishlist
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserID"];
            var wishlistItems = db.WishLists.Where(w => w.UserID == userId).ToList();
            return View(wishlistItems);
        }

        // GET: Wishlist/Add?productId=5
        public ActionResult Add(int productId)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserID"];

            var existing = db.WishLists.FirstOrDefault(w => w.UserID == userId && w.ProductID == productId);
            if (existing == null)
            {
                WishList w = new WishList
                {
                    UserID = userId,
                    ProductID = productId
                };
                db.WishLists.Add(w);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Wishlist/Remove/5
        

        public ActionResult Remove(int id)
        {
            var item = db.WishLists.Find(id);
            if (item != null)
            {
                db.WishLists.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult MoveToCart(int productId)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserID"];

            // Check if product already in cart
            var existing = db.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);
            if (existing == null)
            {
                Cart cartItem = new Cart
                {
                    UserID = userId,
                    ProductID = productId,
                    Quantity = 1
                };
                db.Carts.Add(cartItem);
                db.SaveChanges();
            }

            // Remove from wishlist after moving to cart
            var wishItem = db.WishLists.FirstOrDefault(w => w.UserID == userId && w.ProductID == productId);
            if (wishItem != null)
            {
                db.WishLists.Remove(wishItem);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Cart");
        }

    }
}
