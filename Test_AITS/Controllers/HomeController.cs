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
            if (dealer.DealerTypeId == 1) dealer.IsSIDC = 1;
            else if (dealer.DealerTypeId == 2) dealer.IsAMC = 1;
            else dealer.IsBMC = 1;
            db.Dealers.Add(dealer);
            db.SaveChanges();
            int dealerId = dealer.Id;

            DealerInfo dealerInfo = new DealerInfo();
            dealerInfo.DealerCode = GetDealerCode(dealer.DealerTypeId,dealerId);
            dealerInfo.DealerId = dealerId;
            db.DealerInfos.Add(dealerInfo);
            db.SaveChanges();
            


            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult CreateExistingMember()
        {
            var dealerId = Request.Form["dId"];
            var name = Request.Form["name"];
            var companyId = Request.Form["cId"];
            var dealerTypeId = Convert.ToInt32(Request.Form["dTypeId"]);
            var thanaId = Request.Form["tId"];

            var data = db.Dealers.Find(Convert.ToInt32(dealerId));
            if (dealerTypeId == 1)
            {
                if (data.IsSIDC == 1)
                {
                    TempData["message"] = "This user is Already Registered as SIDC member";
                    return Redirect("Index");
                }
                else
                {
                    data.IsSIDC = 1;
                }
            }
            else if (dealerTypeId == 2)
            {
                if (data.IsAMC == 1)
                {
                    TempData["message"] = "This user is Already Registered as AMC member";
                    return Redirect("Index");
                }
                else
                {
                    data.IsAMC = 1;
                }

            }
            else
            {
                if (data.IsBMC == 1)
                {
                    TempData["message"] = "This user is Already Registered as BMC member";
                    return Redirect("Index");
                }
                else
                {
                    data.IsBMC = 1;
                }
            }



                DealerInfo dealerInfo = new DealerInfo();
                dealerInfo.DealerCode = GetDealerCode(dealerTypeId, Convert.ToInt32(dealerId));
                dealerInfo.DealerId = Convert.ToInt32(dealerId);
                db.Dealers.Update(data);
                db.DealerInfos.Add(dealerInfo);
                db.SaveChanges();
                return Redirect("Index");
        }
        [HttpPost]
        public IActionResult Sell()
        {
            var dealerId = Convert.ToInt32(Request.Form["dealerId"]);
            var dealerCode = Request.Form["dealerCode"].ToString();
            var sellAmount = Convert.ToInt32(Request.Form["sellAmount"]);
            var date = Request.Form["date"];
            var dealer = db.DealerInfos.FirstOrDefault(c=>c.DealerCode == dealerCode);

            DateTime dt = new DateTime();
            //string sellEntryTime = date;//+"-"+dt.ToString("hh:mm tt");
            Dictionary <string,int> commision = CalculateCommission(dealerId,sellAmount);

            Sale saleInfo = new Sale();
            saleInfo.DealerCode = dealerCode;
            saleInfo.SellAmount = sellAmount;
            saleInfo.Date = Convert.ToDateTime(date);
            saleInfo.SalesCommission = commision["EmployeeCommission"];
            saleInfo.InboundCommission = commision["AlphaCommission"];
            saleInfo.OutboundCommission = commision["BetaCommission"];
            saleInfo.OrdinalCommission = commision["AlphaCommission"]+commision["BetaCommission"];
            saleInfo.GBCommission = commision["GBCommission"];
            db.Sales.Add(saleInfo);


            dealer.SalesCommission += commision["EmployeeCommission"];
            dealer.InboundCommission += commision["AlphaCommission"];
            dealer.OutboundCommission += commision["BetaCommission"];
            dealer.OrdinalCommission += commision["AlphaCommission"] + commision["BetaCommission"];
            saleInfo.GBCommission = commision["GBCommission"];
            dealer.SellAmount += saleInfo.SellAmount;
            db.DealerInfos.Update(dealer);

            db.SaveChanges(); 


            return Redirect("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Search()
        {
            dynamic dy = new ExpandoObject();
            var data = db.Sales;
            dy.Report = data;
            dy.totalSell = data.Sum(x => x.SellAmount);
            dy.TotalSalesCommission = data.Sum(x => x.SalesCommission);
            dy.TotalInBoundCommission = data.Sum(x => x.InboundCommission);
            dy.TotalOutBoundCommission = data.Sum(x => x.OutboundCommission);
            dy.TotalOrdinalCommission = data.Sum(x => x.OrdinalCommission);
            dy.TotalGBCommission = data.Sum(x => x.GBCommission);
            dy.totalSumCommission = (dy.TotalSalesCommission + dy.TotalInBoundCommission + dy.TotalOutBoundCommission + dy.TotalOrdinalCommission + dy.TotalGBCommission);


            return View(dy);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SearchCommissionResulByDealerCode(string employeeCode)
        {
            dynamic dy = new ExpandoObject();
            var data = db.Sales.Where(x => x.DealerCode == employeeCode);
            dy.Report = data;
            dy.totalSell = data.Sum(x => x.SellAmount);
            dy.TotalSalesCommission = data.Sum(x => x.SalesCommission);
            dy.TotalInBoundCommission = data.Sum(x => x.InboundCommission);
            dy.TotalOutBoundCommission = data.Sum(x => x.OutboundCommission);
            dy.TotalOrdinalCommission = data.Sum(x => x.OrdinalCommission);
            dy.TotalGBCommission = data.Sum(x => x.GBCommission);
            dy.totalSumCommission = (dy.TotalSalesCommission + dy.TotalInBoundCommission + dy.TotalOutBoundCommission + dy.TotalOrdinalCommission + dy.TotalGBCommission);

            TempData["empCode"] = employeeCode;
            return View("Search", dy);
        }
        [HttpPost]
        public IActionResult SearchCommissionResulByDate(string fromDate,string toDate)
        {
            //return Redirect("Index");
            dynamic dy = new ExpandoObject();
            var data = db.Sales.Where(x => x.Date >= Convert.ToDateTime(fromDate) && x.Date <= Convert.ToDateTime(toDate));
            dy.Report = data;
            dy.totalSell = data.Sum(x => x.SellAmount);
            dy.TotalSalesCommission = data.Sum(x => x.SalesCommission);
            dy.TotalInBoundCommission = data.Sum(x => x.InboundCommission);
            dy.TotalOutBoundCommission = data.Sum(x => x.OutboundCommission);
            dy.TotalOrdinalCommission = data.Sum(x => x.OrdinalCommission);
            dy.TotalGBCommission = data.Sum(x => x.GBCommission);
            dy.totalSumCommission = (dy.TotalSalesCommission + dy.TotalInBoundCommission + dy.TotalOutBoundCommission + dy.TotalOrdinalCommission + dy.TotalGBCommission);

            TempData["fromDate"] = fromDate;
            TempData["toDate"] = toDate;
            return View("Search",dy);
           
        }
        public string GetDealerCode(int id, int dealerId)
        {
            string code;
            if (id == 1)
            {
                string value = String.Format("{0:0000}", dealerId);
                code = "E-" + value;
                

            }
            else if (id == 2)
            {
                string value = String.Format("{0:0000}", dealerId);
                code = "AL-" + value;
                
            }
            else
            {
                string value = String.Format("{0:0000}", dealerId);
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
            var data = db.DealerInfos.Include(d=>d.Dealer).Where(c => c.DealerCode ==code);
            
            if (data!=null)
            {
                return Json(data);
            }
            return Json("False");
        }

        private Dictionary<string, int> CalculateCommission(int dealerId, int sellAmount)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            var dealer = db.Dealers.Find(dealerId);
            List<Commission> commision = db.Commissions.ToList();
            if(dealer.IsSIDC == 1)
            {
                foreach (var item in commision)
                {
                    if(item.Type == "SIDC")
                    {
                        if (sellAmount >= item.FromAmount && sellAmount < item.ToAmount)
                        {
                            float comEmployee = (sellAmount * item.Percentage) / 100;
                            data.Add("EmployeeCommission",Convert.ToInt32(comEmployee));
                        }
                    }
                }
            }
            else
            {
                data.Add("EmployeeCommission", 0);
            }
            if (dealer.IsAMC == 1)
            {
                foreach (var item in commision)
                {
                    if (item.Type == "AMC")
                    {
                        if (sellAmount >= item.FromAmount && sellAmount < item.ToAmount)
                        {
                            float comEmployee = (sellAmount * item.Percentage) / 100;
                            data.Add("AlphaCommission", Convert.ToInt32(comEmployee));
                        }
                    }
                }
            }
            else
            {
                data.Add("AlphaCommission", 0);
            }
            if (dealer.IsBMC == 1)
            {
                foreach (var item in commision)
                {
                    if (item.Type == "BMC")
                    {
                        if (sellAmount >= item.FromAmount && sellAmount < item.ToAmount)
                        {
                            float comEmployee = (sellAmount * item.Percentage) / 100;
                            data.Add("BetaCommission", Convert.ToInt32(comEmployee));
                        }
                    }
                }
            }
            else
            {
                data.Add("BetaCommission", 0);
            }
            data.Add("GBCommission", 0);
            return data;
        }
    }
}
