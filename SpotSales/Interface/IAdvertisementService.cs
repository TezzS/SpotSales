using System;
using System.Collections.Generic;
using SpotSales.Models;

namespace SpotSales.Interface
{
	public interface IAdvertisementService
	{
		IEnumerable<Advertisement> GetAllAd();

		Advertisement GetAdById();

		void AddCart();
		void ChangeName();
	}
}

