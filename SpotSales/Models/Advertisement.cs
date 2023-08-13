using System;
namespace SpotSales.Models
{
	public class Advertisement
	{

		public int Id { get; set; }
		public string Title { get; set; }
		public string Desc { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
	}
}

