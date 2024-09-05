using Microsoft.AspNetCore.Mvc;
using NetProductApp.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore; // namespace/library for DB

namespace NetProductApp.Controllers
{
    public class ProductsController : Controller
    {
        //static List<Product> products = new List<Product>();
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context){
            _context = context;
        }

        private bool IsAdmin(){
            return HttpContext.Session.GetString("UserRole") == "admin";
        }

        public async Task<IActionResult> Index()
        {
            // Joining the Products and Stock tables where there is stock available
            var productsInStock = await _context.Products
                                                .Join(_context.stock,
                                                    p => p.id,        // joinging on id
                                                    s => s.id,
                                                    (p, s) => new { Product = p, Stock = s }) // Select both Product and Stock
                                                .Where(ps => ps.Stock.num_instock > 0)  // Filter where num_instock is greater than 0
                                                .Select(ps => ps.Product)  // Select the Product to return in the result
                                                .ToListAsync();

            return View(productsInStock);
        }

        // only admins authorised to create new products
        public IActionResult Create()
        {
            if(IsAdmin()) return View();
            else return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if(imageFile != null && imageFile.Length > 0){
                //Console.WriteLine("nut null!!!");
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(imageFile.FileName); // Generate a unique file name
                var filePath = Path.Combine("wwwroot/images/products", fileName); // Define the path to save the file

                // save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create)){
                    imageFile.CopyTo(stream);
                }

                product.image_path = "/images/products/"+fileName;
            }
            else{
                product.image_path = "/images/noimg.png";
                //product.image_path = "/images/products/shoes.png";
            }

            //products.Add(product);
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /*
        public IActionResult Edit(int id)
        {
            Console.WriteLine("edit id");
            Product product = products.Find(p => p.id == id);
            return View(product);
        }
        */

        public async Task<IActionResult> Edit(int id)
        {
            if(IsAdmin()){
                Console.WriteLine("edit id");
                var product = await _context.Products.FindAsync(id);
                if(product == null) return NotFound();

                return View(product);
            }
            else return Unauthorized();
        }

        /*
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile imageFile)
        {
            Console.WriteLine("edit product");
            var existingProduct = products.Find(p => p.id == product.id);

            if(existingProduct != null){
                existingProduct.name = product.name;
                existingProduct.price = product.price;

                if(imageFile != null && imageFile.Length > 0){
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(imageFile.FileName); // Generate a unique file name
                    var filePath = Path.Combine("wwwroot/images/products", fileName); // Define the path to save the file

                    // save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create)){
                        imageFile.CopyTo(stream);
                    }

                    product.image_path = "/images/products/"+fileName;
                }
            }

            return RedirectToAction("Index");
        }
        */

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        {
            Console.WriteLine("edit product");
            var existingProduct = await _context.Products.FindAsync(product.id);
            if(existingProduct == null){ Console.WriteLine("tried edit"); return NotFound();}

            existingProduct.name = product.name;
            existingProduct.price = product.price;
            Console.WriteLine("name and price DONE");

            if(imageFile != null && imageFile.Length > 0){
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(imageFile.FileName); // Generate a unique file name
                    var filePath = Path.Combine("wwwroot/images/products", fileName); // Define the path to save the file

                    // save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create)){
                        imageFile.CopyTo(stream);
                    }

                    existingProduct.image_path = "/images/products/"+fileName;
            }

            _context.Update(existingProduct);
            await _context.SaveChangesAsync();

            Console.WriteLine("DONE DONE DONE");

            return RedirectToAction("Index");
        }

        /*
        public IActionResult Delete(int id)
        {
            products.RemoveAt(products.FindIndex(p => p.id == id));
            return RedirectToAction("Index");
        }
        */

        public async Task<IActionResult> Delete(int id)
        {
            if(IsAdmin()){
                var product = await _context.Products.FindAsync(id);
                if(product == null) return NotFound();

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else return Unauthorized();
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            Console.WriteLine("it works hereeee");
            return RedirectToAction("Index");
        }

    }
}