using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }

        IShippingTypeRepository ShippingType { get; }

        IProductRepository Product { get; }

        ICompanyRepository Company { get; }

        IApplicationUserRepository ApplicationUser { get; } 

        IShoppingCartRepository ShoppingCart { get; }

        IOrderDetailRepository OrderDetail { get; } 

        IOrderTypeRepository OrderType { get; }

        void Save();
    }
}
