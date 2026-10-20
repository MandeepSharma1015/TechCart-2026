using Core.Entity;
using Core.UseCases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository(ApplicationDbContext _context) : IProductRepository
    {
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
          return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
           return await _context.Products.FindAsync(id);

        }

        public bool ProductExists(int id)
        {
           return _context.Products.Any(x => x.Id == id);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> SaveChangesAsync()
        {
          return  await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
