using Model.DAL;
using Model.Models;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace OnlineShopping.Controllers
{
    public class ProvinceController : Controller
    {
        // GET: Province
        Province_Town_DistrictDAL dal = new Province_Town_DistrictDAL();
        public ActionResult Index(FormCollection f)
        {
            User_Account userAccount = (User_Account)Session["USER"];

            if (userAccount == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                Province_Bind();
                return View();
            }
        }



        public void Province_Bind()
        {

            DataSet ds = dal.getProvince();
            List<SelectListItem> provinceList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                provinceList.Add(new SelectListItem { Text = dr["Province_Name"].ToString(), Value = dr["Province_ID"].ToString() });
            }
            ViewBag.province = provinceList;
        }

        public JsonResult District_Bind(int provinceId)
        {
            DataSet ds = dal.getAllDistrict(provinceId);
            int i1 = ds.Tables[0].Rows.Count;
            List<SelectListItem> districtList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                districtList.Add(new SelectListItem { Text = dr["District_Name"].ToString(), Value = dr["District_ID"].ToString() });
            }
            return this.Json(districtList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Town_Bind(int districtId)
        {
            DataSet ds = dal.getAllTown(districtId);
            int i = ds.Tables[0].Rows.Count;
            List<SelectListItem> townList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                townList.Add(new SelectListItem { Text = dr["Town_Name"].ToString(), Value = dr["Town_ID"].ToString() });
            }
            return this.Json(townList, JsonRequestBehavior.AllowGet);
        }

    }
}