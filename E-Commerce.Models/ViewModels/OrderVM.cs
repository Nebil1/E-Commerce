using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models.ViewModels
{
	public class OrderVM
	{
		public OrderType OrderType { get; set; }

		public IEnumerable<OrderDetail> OrderDetail { get; set; }
	}
}
