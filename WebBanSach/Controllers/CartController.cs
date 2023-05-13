using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach.Application.Interfaces;
using WebBanSach.Application.ViewModels.Product;
using WebBanSach.Data.Enums;
using WebBanSach.Extensions;
using WebBanSach.Models;
using WebBanSach.Services;
using WebBanSach.Utilities.Constant;

namespace WebBanSach.Controllers
{
    public class CartController : Controller
    {
        IProductService _productService;
        IBillService _billService;
        IViewRenderService _viewRenderService;
        IConfiguration _configuration;
        IEmailSender _emailSender;
        IProductQuantityService _productQuantityService;

        public CartController(IProductService productService,
            IViewRenderService viewRenderService, IEmailSender emailSender,
            IConfiguration configuration, IBillService billService, 
            IProductQuantityService productQuantityService)
        {
            _productService = productService;
            _billService = billService;
            _viewRenderService = viewRenderService;
            _configuration = configuration;
            _emailSender = emailSender;
            _productQuantityService = productQuantityService;
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("checkout.html", Name = "Checkout")]
        [HttpGet]
        //[Authorize]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

            model.Carts = session;
            return View(model);
        }

        [Route("checkout.html", Name = "Checkout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

            if (ModelState.IsValid)
            {
                if (session != null)
                {
                    var details = new List<BillDetailViewModel>();
                    foreach (var item in session)
                    {
                        details.Add(new BillDetailViewModel()
                        {
                            //Product = item.Product,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id,
                        });
                    }
                    var billViewModel = new BillViewModel()
                    {
                        CustomerMobile = model.CustomerMobile,
                        BillStatus = BillStatus.New,
                        CustomerAddress = model.CustomerAddress,
                        CustomerName = model.CustomerName,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details
                    };
                    if (User.Identity.IsAuthenticated == true)
                    {
                        billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
                    }
                    _billService.Create(billViewModel);
                    try
                    {

                        _billService.Save();

                        //var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
                        //Send mail
                        //await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from TechBook Shop", content);
                        ViewData["Success"] = true;
                    }
                    catch (Exception ex)
                    {
                        ViewData["Success"] = false;
                        ModelState.AddModelError("", ex.Message);
                    }

                }
            }
            //model.Carts = session;
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return View(model);
        }

        #region AJAX Request
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            //Get product detail
            var product = _productService.GetById(productId);
            int product_quantity = _productService.GetQuantities(productId).Where(x => x.Quantity != 0).FirstOrDefault().Quantity;


            if(quantity <= product_quantity && quantity > 0)
			{
                int q = product_quantity - quantity;
                //Get session with item list from cart
                var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
                if (session != null)
                {
                    //Convert string to list object
                    bool hasChanged = false;

                    //Check exist with item product id
                    if (session.Any(x => x.Product.Id == productId))
                    {

                        foreach (var item in session)
                        {
                            //Update quantity for product if match product id
                            if (item.Product.Id == productId)
                            {
                                item.Quantity += quantity;
                                item.Price = product.PromotionPrice ?? product.Price;

                                hasChanged = true;
                            }
                        }
                        
                    }
                    else
                    {
                        session.Add(new ShoppingCartViewModel()
                        {
                            Product = product,
                            Quantity = quantity,
                            Price = product.PromotionPrice ?? product.Price
                        });
                        hasChanged = true;
                    }
                    //Update back to cart
                    if (hasChanged)
                    {
                        HttpContext.Session.Set(CommonConstants.CartSession, session);
                    }
                }
                else
                {
                    //Add new cart
                    var cart = new List<ShoppingCartViewModel>();
                    cart.Add(new ShoppingCartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Price = product.PromotionPrice ?? product.Price
                    });
                    HttpContext.Session.Set(CommonConstants.CartSession, cart);
                }
                _productQuantityService.Update(productId, q);
                _productQuantityService.SaveChanges();
                return new OkObjectResult(productId);
            }
			else
			{
                ViewData["Success"] = false;
                return Redirect("/");
			}
        }

        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            int product_quantity = _productService.GetQuantities(productId).Where(x => x.Quantity != 0).FirstOrDefault().Quantity;
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        int q = product_quantity + item.Quantity;
                        _productQuantityService.Update(productId, q);
                        _productQuantityService.SaveChanges();
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productService.GetById(productId);
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }
        #endregion
    }
}
