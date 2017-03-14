﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAL;
using Model.Models;
using Model.ViewModel;

namespace OnlineShopping.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }       

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string email = f["txtEmail"];
            string password = f["txtPassword"];
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

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        
        [HttpGet]
        public ActionResult Register()
        {
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["ERROR"] = "Your Email is existed!!!";
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("USER");
            return RedirectToAction("Index", "Home");
        }
    }
}