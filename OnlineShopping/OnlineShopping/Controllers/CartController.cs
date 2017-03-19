using Model.DAL;
using Model.Models;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            if (quantity > product.Products_Available)
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
                        if (item.Quantity + quantity > product.Products_Available)
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
                        if (item.Product_ID == productId)
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
                        if (item.Product_ID == productId)
                        {
                            myCart.Remove(item);
                            break;
                        }
                    }
                }

            }
            return RedirectToAction("ViewCart", "Cart");
        }

        public ActionResult Checkout(FormCollection f)
        {
            User_Account userAccount = (User_Account)Session["USER"];
            DateTime today = DateTime.Today;
            string email = userAccount.Email;
            string name = f["txtName"];
            string address = f["txtAddress"];
            string province = f["province"];
            string district = f["district"];
            string town = f["town"];
            string orderPhone = f["txtNumber"];
            Order_Details order = new Order_Details
            {
                Email = email,
                Ordered_Date = today,
                Delivered = false,
                Cancelled_Order = false,
                Reason_Cancel = "",
                Order_Name = name,
                Exact_Address = address,
                Order_Phone = orderPhone,
                Province_ID = Int32.Parse(province),
                District_ID = Int32.Parse(district),
                Town_ID = Int32.Parse(town)
            };
            OrderDetailsDAL dal = new OrderDetailsDAL();
            int orderDetailsId = dal.addOrder(order);
            if (orderDetailsId >= 0)
            {
                ICollection<Product_Cart> productCarts = userAccount.Product_Cart;
                Product_CartDAL productCartDal = new Product_CartDAL();
                foreach (var item in productCarts)
                {
                    ProductOrderDetailsDAL podDal = new ProductOrderDetailsDAL();
                    Product_Order_Details pod = new Product_Order_Details()
                    {
                        Product_ID = item.Product_ID,
                        Order_ID = orderDetailsId,
                        Order_Quantity = (int)item.Quantity,
                        Price = item.Product.Price,
                        Discount_Percent = item.Product.DiscountPercent,
                    };

                    podDal.addProductOrderDetails(pod);

                    productCartDal.deleteRecord(item.Product_ID, userAccount.Email);
                }
                productCarts.Clear();
                Session.Remove("CART");
                return RedirectToAction("Index", "Congratulate");
            }

            return View();
        }

        public ActionResult Login(FormCollection f)
        {
            string email = f["txtEmail"];
            string password = f["txtPassword"];
            if(email == null && password == null)
            {
                return View();
            }
            AccountDAL accountDal = new AccountDAL();
            User_Account u_account = accountDal.checkLogin(email, password);
            if (u_account == null)
            {
                ViewBag.Message = "Your Email or Password is INCORRECT!!!";
            }
            else
            {
                Session["USER"] = u_account;

                Product_CartDAL productCartDal = new Product_CartDAL();
                List<Product_Cart> productCarts = productCartDal.getProductCartsByEmail(u_account.Email);
                foreach (var item in productCarts)
                {
                    u_account.Product_Cart.Add(item);
                }

                DateTime birthday = (DateTime)u_account.Birthday;
                Session["USER"] = u_account;
                Session["DATE"] = birthday.Day.ToString();
                Session["MONTH"] = birthday.Month.ToString();
                Session["YEAR"] = birthday.Year.ToString();
                Session["GENDER"] = u_account.Gender;

                addToCart();

                return RedirectToAction("ViewCart", "Cart");
            }
            return View();
        }

        public ActionResult Register(FormCollection f)
        {
            string email = f["txtEmail"];
            string password = f["txtPassword"];
            string fullName = f["txtFullname"];
            string bDate = f["slBirthDay"];
            string bMonth = f["slbirthMonth"];
            string bYear = f["slbirthYear"];
            string gender = f["slGender"];
            string address = f["txtAddress"];
            bool poli = (f["cbPoli"] == "on") ? true : false;
            string s = bYear + "-" + bMonth + "-" + bDate;

            AccountDAL accountDal = new AccountDAL();
            bool checkExist = accountDal.checkExist(email);
            if (checkExist == false)
            {
                DateTime bd = Convert.ToDateTime(s);
                DateTime today = DateTime.Today;
                User_Account u_account = new User_Account { Email = email, Password = password, Full_Name = fullName, Birthday = bd, Gender = gender, Address = address, Cancel_Amount = 0, created_Date = today };
                accountDal.registerAccount(email, password, fullName, bd, gender, address, today);
                Session["USER"] = u_account;
                Session["DATE"] = bDate;
                Session["MONTH"] = bMonth;
                Session["YEAR"] = bYear;
                Session["GENDER"] = gender;

                addToCart();

                return RedirectToAction("ViewCart", "Cart");
            }
            else
            {
                Session["ERROR"] = "Your Email is existed!!!";
                return RedirectToAction("Login", "Login");
            }
        }

        private bool addToCart()
        {
            List<Product_Cart> myCart = new List<Product_Cart>();
            if (Session["CART"] != null)
            {
                myCart = (List<Product_Cart>)Session["CART"];
            }

            if (Session["USER"] != null)
            {
                User_Account userAccount = (User_Account)Session["USER"];
                ICollection<Product_Cart> productCarts = userAccount.Product_Cart;
                Product_CartDAL productCartDal = new Product_CartDAL();

                foreach (var myProduct in myCart)
                {
                    int productId = myProduct.Product_ID;
                    int quantity = (int)myProduct.Quantity;

                    ProductDAL productDal = new ProductDAL();
                    Product product = productDal.getProductById(productId);

                    bool isContain = false;
                    foreach (var item in productCarts)
                    {
                        if (item.Product_ID == productId)
                        {
                            isContain = true;
                            if (item.Quantity + quantity <= product.Products_Available)
                            {
                                item.Quantity += quantity;
                                productCartDal.updateQuantity(item);
                            }
                            else
                            {
                                item.Quantity = product.Products_Available;
                                productCartDal.updateQuantity(item);
                            }
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
            }
            else return false;
            return true;
        }
    }
}