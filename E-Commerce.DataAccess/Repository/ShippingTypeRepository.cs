using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository
{
    public class ShippingTypeRepository : Repository<ShippingType>, IShippingTypeRepository
    {
        private ApplicationDbContext _db;

        public ShippingTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShippingType obj)
        {
            _db.ShippingTypes.Update(obj);
        }
    }
}
