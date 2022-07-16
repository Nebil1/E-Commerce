using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);   
            if(objFromDb != null)
            {
                objFromDb.ProductName = obj.ProductName;
                objFromDb.Producer = obj.Producer;
                objFromDb.ProductId = obj.ProductId;
                objFromDb.Price = obj.Price;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ShippingTypeId = obj.ShippingTypeId;  
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.ListPrice100 = obj.ListPrice100;

            if(obj.Image != null)
             {
                objFromDb.Image = obj.Image;
             } 

            }
        }
    }
}
