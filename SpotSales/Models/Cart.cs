using System;
namespace SpotSales.Models
{
	public class Cart
	{
        public int ShopperID { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Subtotal { get; set; }
        public int OrderPlaced { get; set; }
        public decimal Discount { get; set; }
    }
}

