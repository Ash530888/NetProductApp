using Microsoft.AspNetCore.Mvc;
using NetProductApp.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NetProductApp.Controllers
{
    public class CartController : Controller{
        static Dictionary<Product, int> Cart = new Dictionary<Product, int>();
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context){
            _context = context;
        }

        public IActionResult Index()
        {
            //var products = _context.Products.Where(p => Cart.Keys.Contains(p.id)).ToList();
            return View(Cart);
        }

        /*
        public async Task<IActionResult> Index(int id)
        {
            if(IsShopper()){
                Product product = await _context.Products.FindAsync(id);
                if(product == null) return NotFound();

                products.Add(product, 1);

                return View(products);
            }
            else return Unauthorized();
        }
        */

        public IActionResult Checkout()
        {
            if(IsShopper()){
                //var products = _context.Products.Where(p => Cart.Keys.Contains(p.id)).ToList();

                return View(Cart);
            }
            else return Unauthorized();
        }

        public IActionResult Delete(int id){
            Product product = FindProductInCart(id);
            Cart.Remove(product);
            return RedirectToAction("Index", "Cart");
        }

        private Product FindProductInCart(int id){
            foreach(var product in Cart){
                if(product.Key.id == id) return product.Key;
            }
            return null;
        }

        public async Task<IActionResult> AddToCart(int id){
            //var product = _context.Products.SingleOrDefault(p => p.id == id);
            var product = FindProductInCart(id);
            var max = _context.stock.SingleOrDefault(stock => stock.id == id).num_instock;

            if(product != null){
                if(max - Cart[product] > 0) Cart[product] += 1;
                else TempData["MaxStockReached"] = "Stock Limit Reached For Product: "+product.name;
            }
            else if (max > 0){
                product = _context.Products.SingleOrDefault(p => p.id == id);
                Cart[product] = 1;
                
                //await UpdateStock(id);
            }

            return RedirectToAction("Index", "Cart");
            
        }

        

        private async Task UpdateStock(){
            foreach(var product in Cart){
                var product_in_stock = _context.stock.SingleOrDefault(stock => stock.id == product.Key.id);
                product_in_stock.num_instock -= product.Value;
            }
            await _context.SaveChangesAsync();
        }

        

        public async Task<IActionResult> UpdateQuantity(string action, int productId){

            //Console.WriteLine("UpdateQuantity");
            
            //var dbproduct = _context.Products.SingleOrDefault(product => product.id == productId);
            var product = FindProductInCart(productId);
            var max = _context.stock.SingleOrDefault(stock => stock.id == productId).num_instock;

            
            if(action == "increase"){
                if(Cart[product] < max) Cart[product] += 1;
                else TempData["MaxStockReached"] = "Stock Limit Reached For Product: "+product.name;
            }
            else if(action == "decrease"){
                if(Cart[product] > 0) Cart[product] -= 1;
            }

            //await UpdateStock(productId, Cart[product]);
            

            return RedirectToAction("Index", "Cart");

        }

        public async Task<IActionResult> UpdateQuantityManualInput(int id, int quantity){
            //var product = _context.Products.SingleOrDefault(product => product.id == productId);
            Console.WriteLine("UpdateQuantityManualInput "+id+","+quantity);
            var product = FindProductInCart(id);
            var max = _context.stock.SingleOrDefault(stock => stock.id == id).num_instock;

            if(0 < quantity && quantity < max) Cart[product] = quantity;
            else{
                Cart[product] = max;
                TempData["InvalidQuantity"] = "Invalid Quantity Entered For Product: "+product.name;
            }

            //await UpdateStock(productId, quantity);

            return RedirectToAction("Index", "Cart");

        }

        public async Task<IActionResult> SuccessfulPayment(){
            Console.WriteLine("SuccessfulPayment");
            TempData["SuccessfulPayment"] = "Payment Successful. Order Confirmed.";
            await UpdateStock();
            Cart.Clear();
            return RedirectToAction("Index", "Products");
        }

        private bool IsShopper(){
            return HttpContext.Session.GetString("UserRole") == "shopper";
        }
    }

    
}