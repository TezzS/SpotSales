using System;
using SpotSales.Models;
using System.Collections.Generic;

namespace SpotSales.Interface
{
	public interface ICartService
	{
        IEnumerable<Cart> GetCart();
        void PlaceOrder();
    }
}

