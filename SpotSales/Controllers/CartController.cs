using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotSales.Interface;
using SpotSales.Models;
using SpotSales.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpotSales.Controllers
{
    public class CartController : Controller
    {
        ICartService crtService;

        public CartController(ICartService _crtService)
        {
            crtService = _crtService;
        }
        // GET: /<controller>/
        public ActionResult Index()
        {
            IEnumerable<Cart> crt = crtService.GetCart();
            return View(crt);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            // Call the PlaceOrder() method from your CartService
            crtService.PlaceOrder();

            // You can return a view, redirect, or perform any other action here
            return RedirectToAction("Index", "Advertisement"); // Redirect to OrderConfirmation action in HomeController
        }
    }
}

