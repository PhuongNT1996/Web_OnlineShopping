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
            if (Session["USER"] != null)
            {
                ProductDAL productDal = new ProductDAL();
                User_Account userAccount = (User_Account)Session["USER"];
                ICollection<Product_Cart> productCarts = userAccount.Product_Cart;

                string quantityStr = f["myNumber"];
                Int32 quantity = Convert.ToInt32(quantityStr);

                string productIdStr = f["productId"];
                int productId = int.Parse(productIdStr);

                Product_CartDAL productCartDal = new Product_CartDAL();

                bool isContain = false;
                foreach (var item in productCarts)
                {
                 if(item.Product_ID == productId)
                    {
                        isContain = true;
                        item.Quantity += quantity;
                        productCartDal.updateQuantity(item);
                        break;
                    }
                }

                if (!isContain)
                {
                    
                    Product product = productDal.getProductById(productId);
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
                string quantityStr = f["myNumber"];
                Int32 quantity = Convert.ToInt32(quantityStr);

                string productIdStr = f["productId"];

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
                    productCart.Quantity += quantity;
                }
                else
                {
                    int productId = int.Parse(productIdStr);
                    ProductDAL productDal = new ProductDAL();
                    Product product = productDal.getProductById(productId);

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
            return View();
        }
    }
}