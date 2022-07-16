using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;

            Category = new CategoryRepository(_db);
            ShippingType = new ShippingTypeRepository(_db);   
            Product = new ProductRepository(_db);   
            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);  
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderType = new OrderTypeRepository(_db);   
            OrderDetail = new OrderDetailRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }   

        public IShippingTypeRepository ShippingType { get; private set; }

        public IProductRepository Product { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }   

        public IApplicationUserRepository ApplicationUser { get; private set; }   

        public IOrderTypeRepository OrderType { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
