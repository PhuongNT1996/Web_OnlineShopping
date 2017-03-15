using Model.DAL;
using Model.Models;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult ViewCart()
        {
            return View();
        }

        public ActionResult Add(FormCollection f)
        {
            string quantityStr = f["myNumber"];
            Int32 quantity = Convert.ToInt32(quantityStr);

            string productIdStr = f["productId"];
            int productId = int.Parse(productIdStr);

            ProductDAL productDal = new ProductDAL();
            Product product = productDal.getProductById(productId);

            if(quantity > product.Products_Available)
            {
                Session["ADD_ERROR"] = "This product has only " + product.Products_Available + " items";
                return RedirectToAction("ProductDetails", "Product", new { productIDStr = @product.Product_ID + "" });
            }

            if (Session["USER"] != null)
            {
                User_Account userAccount = (User_Account)Session["USER"];
                ICollection<Product_Cart> productCarts = userAccount.Product_Cart;
                Product_CartDAL productCartDal = new Product_CartDAL();

                bool isContain = false;
                foreach (var item in productCarts)
                {
                    if (item.Product_ID == productId)
                    {
                        isContain = true;
                        if(item.Quantity + quantity > product.Products_Available)
                        {
                            Session["ADD_ERROR"] = "This product has only " + (product.Products_Available - item.Quantity) + " items";
                            return RedirectToAction("ProductDetails", "Product", new { productIDStr = @product.Product_ID + "" });
                        }
                        item.Quantity += quantity;
                        productCartDal.updateQuantity(item);
                        break;
                    }
                }

                if (!isContain)
                {
                    Product_Cart productCart = new Product_Cart()
                    {
                        Email = userAccount.Email,
                        Quantity = quantity,
                        Product_ID = productId,
                        Product = product,
                    };
                    productCarts.Add(productCart);
                    productCartDal.addProductCart(productCart);
                }
            }
            else
            {
                List<Product_Cart> myCart = new List<Product_Cart>();
                if (Session["CART"] != null)
                {
                    myCart = (List<Product_Cart>)Session["CART"];
                }

                Product_Cart productCart = null;
                foreach (var item in myCart)
                {
                    if (productIdStr.Equals(item.Product_ID + ""))
                    {
                        productCart = item;
                        break;
                    }
                }

                if (productCart != null)
                {
                    if (productCart.Quantity + quantity > product.Products_Available)
                    {
                        Session["ADD_ERROR"] = "This product has only " + (product.Products_Available - productCart.Quantity) + " items";
                        return RedirectToAction("ProductDetails", "Product", new { productIDStr = @product.Product_ID + "" });
                    }
                    productCart.Quantity += quantity;
                }
                else
                {
                    productCart = new Product_Cart()
                    {
                        Quantity = quantity,
                        Product_ID = productId,
                        Product = product,
                    };
                    myCart.Add(productCart);
                }
                Session["CART"] = myCart;
            }
            return RedirectToAction("ViewCart", "Cart");
        }

        public ActionResult Delete(FormCollection f)
        {
            string productIdStr = f["productId"];
            int productId;
            if (int.TryParse(productIdStr, out productId))
            {
                if (Session["USER"] != null)
                {
                    User_Account userAccount = (User_Account)Session["USER"];
                    ICollection<Product_Cart> productCarts = userAccount.Product_Cart;

                    Product_CartDAL productCartDal = new Product_CartDAL();
                    productCartDal.deleteRecord(productId, userAccount.Email);

                    foreach (Product_Cart item in productCarts)
                    {
                        if(item.Product_ID == productId)
                        {
                            productCarts.Remove(item);
                            break;
                        }
                    }
                }
                else if (Session["CART"] != null)
                {
                    List<Product_Cart> myCart = (List<Product_Cart>)Session["CART"];
                    foreach (Product_Cart item in myCart)
                    {
                        if(item.Product_ID == productId)
                        {
                            myCart.Remove(item);
                            break;
                        }
                    }
                }

            }
            return RedirectToAction("ViewCart", "Cart");
        }
    }
}