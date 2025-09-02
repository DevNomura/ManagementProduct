using ManagementProduct.Models;
using ManagementProduct.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ManagementProduct.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _products = new List<Product>();
        private static ProductRepository _repo = new ProductRepository();
        public IActionResult Index()
        {
            return View(_products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length < 3 || product.Name.Length > 100)
                ModelState.AddModelError("Name", "O Nome do produto deve ter entre 3 e 100 caracteres");

            if (product.Price <= 0)
                ModelState.AddModelError("Price", "O Preço do produto deve ser maior que zero");

            if (product.Stock <= 0)
                ModelState.AddModelError("Stock", "O Estoque do produto não pode ser negativo");

            if (!ModelState.IsValid)
                return View(product);

            product.IdProduct = _products.Select(p => p.IdProduct).DefaultIfEmpty(0).Max() + 1;
            _products.Add(product);

            return RedirectToAction("Index");
        
        }
        public IActionResult Edit(int id)
        {
            var product = _products.FirstOrDefault(p => p.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.IdProduct == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length < 3 || product.Name.Length > 100)
                ModelState.AddModelError("Name", "O Nome do produto deve ter entre 3 e 100 caracteres");

            if (product.Price <= 0)
                ModelState.AddModelError("Price", "O Preço do produto deve ser maior que zero");

            if (product.Stock < 0)
                ModelState.AddModelError("Stock", "O Estoque do produto não pode ser negativo");


            if (existingProduct.IsActive && product.Stock == 0)
                ModelState.AddModelError("Stock", "Produto ativo não pode ter estoque zero.");

            if (!ModelState.IsValid)
                return View(product);


            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Category = product.Category;

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _products.FirstOrDefault(p => p.IdProduct == id);
                if (product == null)
                {
                    return NotFound();
                }
                _products.Remove(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult Search(int? id)
        {
            List<Product> products;

            if (id == null)
            {
                products = _products; 
            }
            else
            {
                var product = _products.FirstOrDefault(p => p.IdProduct == id.Value);
                products = product == null ? new List<Product>() : new List<Product> { product };
            }

            return View(products);
        }
        public IActionResult ByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return View("Search", _products);
            }

            var products = _products
                .Where(p => p.Category != null &&
                            p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View("Search", products);
        }



    }
}
