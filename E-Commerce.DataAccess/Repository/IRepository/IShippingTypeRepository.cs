using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository.IRepository
{
    public interface IShippingTypeRepository : IRepository<ShippingType>
    {
        void Update(ShippingType obj); 
    }
} 
