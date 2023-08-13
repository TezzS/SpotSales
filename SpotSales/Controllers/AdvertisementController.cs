using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SpotSales.Interface;
using SpotSales.Models;
using SpotSales.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpotSales.Controllers
{
    public class AdvertisementController : Controller
    {
        IAdvertisementService adService;

        public AdvertisementController(IAdvertisementService _adService)
        {
            adService = _adService;
        }
        // GET: /<controller>/
        public ActionResult Index()
        {
            IEnumerable<Advertisement> ad = adService.GetAllAd();
            return View(ad);
        }
        [HttpPost]
        public IActionResult AddCart()
        {
            // Call the PlaceOrder() method from your CartService
            adService.AddCart();

            // You can return a view, redirect, or perform any other action here
            return RedirectToAction("Index", "Cart"); // Redirect to OrderConfirmation action in HomeController
        }
        [HttpPost]
        public IActionResult ChangeName()
        {
            // Call the PlaceOrder() method from your CartService
            adService.ChangeName();

            // You can return a view, redirect, or perform any other action here
            return RedirectToAction("Index", "Advertisement"); // Redirect to OrderConfirmation action in HomeController
        }
    }
}

