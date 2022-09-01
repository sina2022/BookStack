﻿using BookStack_DataAccess.Repositories.IRepository;
using BookStack_Models;
using BookStack_Models.ViewModels;
//using BookStack_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace BookStack_Web.Controllers;
[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");

        return View(productList);
    }

    public IActionResult Details(int productId)
    {
        ShoppingCart cartObj = new()
        {
            Count = 1,
            ProductId = productId,
            Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
        };

        return View(cartObj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart cartItem)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        cartItem.ApplicationUserId = claim.Value;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
            u => u.ApplicationUserId == claim.Value && u.ProductId == cartItem.ProductId);

        // Add product to cart if isn't found in DB Table "ShoppingCart"
        if (cartFromDb == null)
        {

            _unitOfWork.ShoppingCart.Add(cartItem);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32("SessionShoppingCart",
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
        }
        else
        {
            _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, cartItem.Count);
            _unitOfWork.Save();
        }


        return RedirectToAction(nameof(Index));
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
