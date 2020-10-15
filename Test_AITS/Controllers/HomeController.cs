using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test_AITS.Database;
using Test_AITS.Models;

namespace Test_AITS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AITSDBContext db = new AITSDBContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            dynamic dy = new ExpandoObject();
            dy.companies = db.Companies.ToList();
            dy.thanas = db.Thanas.ToList();
            dy.dealerTypes = db.DealerTypes.ToList();
            dy.dealerInfos = db.DealerInfos.Include(d => d.Dealer);
            return View(dy);
        }

        [HttpPost]
        public IActionResult Create()
        {
            Dealer dealer = new Dealer();
            var name = Request.Form["name"];
            var companyId = Request.Form["companyId"];
            var dealerTypeId = Request.Form["dealerTypeId"];
            var thanaId = Request.Form["thanaId"];

            dealer.Name = name;
            dealer.CompanyId = Convert.ToInt32(companyId);
            dealer.ThanaId = Convert.ToInt32(thanaId);
            dealer.DealerTypeId = Convert.ToInt32(dealerTypeId);
            db.Dealers.Add(dealer);
            db.SaveChanges();
            int dealerId = dealer.Id;

            DealerInfo dealerInfo = new DealerInfo();
            dealerInfo.DealerCode = GetDealerCode(dealer.DealerTypeId);
            dealerInfo.SellAmount = 0;
            dealerInfo.SalesCommission = 0;
            dealerInfo.InboundCommission = 0;
            dealerInfo.OutboundCommission = 0;
            dealerInfo.OrdinalCommission = 0;
            dealerInfo.DealerId = dealerId;
            db.DealerInfos.Add(dealerInfo);
            db.SaveChanges();
            


            return Redirect("Index");
        }

        public IActionResult CreateExistingMember()
        {
            var dealerId = Request.Form["dId"];
            var name = Request.Form["name"];
            var companyId = Request.Form["cId"];
            var dealerTypeId = Request.Form["dTypeId"];
            var thanaId = Request.Form["tId"];


            DealerInfo dealerInfo = new DealerInfo();
            dealerInfo.DealerCode = GetDealerCode(Convert.ToInt32(dealerTypeId));
            dealerInfo.SellAmount = 0;
            dealerInfo.SalesCommission = 0;
            dealerInfo.InboundCommission = 0;
            dealerInfo.OutboundCommission = 0;
            dealerInfo.OrdinalCommission = 0;
            dealerInfo.DealerId = Convert.ToInt32(dealerId);
            db.DealerInfos.Add(dealerInfo);
            db.SaveChanges();
            return Redirect("Index");
        }
        [HttpPost]
        public IActionResult Sell()
        {
            var dealerId = Request.Form["dealerId"];
            var dealerCode = Request.Form["dealerCode"];
            var dealerName = Request.Form["dealerName"];
            var sellAmount = Request.Form["sellAmount"];
            var date = Request.Form["date"];

            DateTime dt = DateTime.Now;
            string sellEntryTime = date+"-"+dt.ToString("hh:mm tt");

            List<DealerInfo> DealerInfo = GetDealerTypeInfo(Convert.ToInt32(dealerId));



            return Redirect("Index");
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

        public string GetDealerCode(int id)
        {
            string code;
            if (id == 1)
            {
                string value = String.Format("{0:0000}", id);
                code = "E-" + value;
                

            }
            else if (id == 2)
            {
                string value = String.Format("{0:0000}", id);
                code = "AL-" + value;
                
            }
            else
            {
                string value = String.Format("{0:0000}", id);
                code = "BL-" + value;
                
            }
            return code;
        }
        public List<DealerInfo> GetDealerTypeInfo(int dealerId)
        {
            var data = db.DealerInfos.Where(c => c.DealerId == dealerId).ToList();
            return data;
        }


        public JsonResult GetDealerInfo(string code)
        {
            var data = db.DealerInfos.Where(c => c.DealerId == Convert.ToInt32(code));
            if(data!=null)
            {
                return Json(data);
            }
            return Json("False");
        }
    }
}
