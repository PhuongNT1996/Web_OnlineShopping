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
        public ActionResult Index()
        {
            Province_Bind();
            return View();
        }

        public void Province_Bind()
        {
            Province_Town_DistrictDAL dal = new Province_Town_DistrictDAL();
            DataSet ds = dal.getProvince();
            List<SelectListItem> provinceList = new List<SelectListItem>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                provinceList.Add(new SelectListItem {Text=dr["Province_Name"].ToString(), Value=dr["Province_ID"].ToString()});
            }
            ViewBag.Province = provinceList;
        }

    }
}