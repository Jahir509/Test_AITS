using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            var associateOf = Request.Form["associateOf"].ToString();
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
            dealerInfo.DealerCode = GetDealerCode(dealer.DealerTypeId, dealerId);
            dealerInfo.DealerId = dealerId;
            if (associateOf == "") dealerInfo.AssociateOf = "Company";
            else dealerInfo.AssociateOf = associateOf;
            dealerInfo.Designation = "A";
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
            var associateOf = Request.Form["associatorOf"];

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
            if (associateOf == "") dealerInfo.AssociateOf = "Company";
            else dealerInfo.AssociateOf = associateOf;
            dealerInfo.Designation = "A";
            db.Dealers.Update(data);
            db.DealerInfos.Add(dealerInfo);
            db.SaveChanges();
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult Details(string code)
        {
            dynamic dy = new ExpandoObject();
            var dealerDetails = db.DealerInfos.Include(d => d.Dealer).FirstOrDefault(d => d.DealerCode == code);
            var associates = db.DealerInfos.Where(d => d.AssociateOf == code);
            var designation = associates.Select(d => d.Designation).Distinct();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in designation)
            {
                data.Add(item, GetDesignatedEmployee(item, associates));
            }
            dy.dealer = dealerDetails;
            dy.data = data.ToList();
            dy.designation = designation.ToList();

            return View(dy);
        }

        public string GetDesignatedEmployee(string item, IQueryable<DealerInfo> associates)
        {
            string str = null;
            foreach (var a in associates)
            {
                if (a.Designation == item)
                {
                    str = str + a.DealerCode + ",";
                }
            }
            return str;
        }

        [HttpPost]
        public IActionResult Sell()
        {
            List<SalesCommission> salesCommission = db.SalesCommissions.ToList();
            var dealerId = Convert.ToInt32(Request.Form["dealerId"]);
            var dealerCode = Request.Form["dealerCode"].ToString();
            var sellAmount = Convert.ToInt32(Request.Form["sellAmount"]);
            var date = Request.Form["date"];
            var dealer = db.DealerInfos.FirstOrDefault(c => c.DealerCode == dealerCode);

            DateTime dt = new DateTime();
            //string sellEntryTime = date;//+"-"+dt.ToString("hh:mm tt");
            Dictionary<string, float> commision = CalculateCommission(dealerId, sellAmount);

            Sale saleInfo = new Sale();
            saleInfo.DealerCode = dealerCode;
            saleInfo.SellAmount = sellAmount;
            saleInfo.Date = Convert.ToDateTime(date);
            saleInfo.SalesCommission = (int)commision["EmployeeCommission"];
            saleInfo.InboundCommission = (int)commision["AlphaCommission"];
            saleInfo.OutboundCommission = (int)commision["BetaCommission"];
            saleInfo.OrdinalCommission = (int)commision["AlphaCommission"] + (int)commision["BetaCommission"];
            saleInfo.GBCommission = (int)commision["GBCommission"];
            db.Sales.Add(saleInfo);



            dealer.SalesCommission += (int)commision["EmployeeCommission"];
            dealer.InboundCommission += (int)commision["AlphaCommission"];
            dealer.OutboundCommission += (int)commision["BetaCommission"];
            dealer.OrdinalCommission += (int)commision["AlphaCommission"] + (int)commision["BetaCommission"];
            saleInfo.GBCommission = (int)commision["GBCommission"];
            dealer.SellAmount += saleInfo.SellAmount;
            db.DealerInfos.Update(dealer);

            db.SaveChanges();


            SetAssociateCommission(saleInfo.Id, dealer.AssociateOf, commision["CommissionPercent"], saleInfo.Date, saleInfo.SellAmount, salesCommission);


            return Redirect("Index");
        }

        private Boolean SetAssociateCommission(int s, string dealerCode, float v, DateTime date, int saleAmount, List<SalesCommission> salesCommission)
        {
            var dealer = db.DealerInfos.FirstOrDefault(d => d.DealerCode == dealerCode);
            if (dealerCode == "Company" || dealer.Designation == "Company")
            {
                float rate = 26 - v;
                AssociationCommission aCommission = new AssociationCommission();
                aCommission.CommissionType = "Associates";
                aCommission.SaleId = s;
                aCommission.GoesTo = "Company";
                aCommission.Amount = Convert.ToInt32((saleAmount * rate) / 100);
                aCommission.Date = date;
                db.AssociationCommissions.Add(aCommission);
                db.SaveChanges();
                return true;
            }

            else
            {
                //var dealerAssociate = db.DealerInfos.FirstOrDefault(d => d.DealerCode == dealer.AssociateOf);
                if (dealer.SellAmount >= 4000)
                {
                    foreach (var item in salesCommission)
                    {
                        if (item.Designation == dealer.Designation)
                        {
                            float rate = item.Percentage - v;
                            AssociationCommission aCommission = new AssociationCommission();
                            aCommission.CommissionType = "Associates";
                            aCommission.SaleId = s;
                            aCommission.GoesTo = dealer.DealerCode;
                            aCommission.Amount = Convert.ToInt32((saleAmount * rate) / 100);
                            aCommission.Date = date;
                            v = item.Percentage;
                            db.AssociationCommissions.Add(aCommission);
                            db.SaveChanges();
                            break;

                        }
                    }
                    return SetAssociateCommission(s, dealer.AssociateOf, v, date, saleAmount, salesCommission);
                }
                else
                {
                    return SetAssociateCommission(s, dealer.AssociateOf, v, date, saleAmount, salesCommission);
                }


            }

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
            var ordinalCommissionData = db.AssociationCommissions.Where(d => d.GoesTo == employeeCode);
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
        public IActionResult SearchCommissionResulByDate(string fromDate, string toDate)
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
            return View("Search", dy);

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
            var data = db.DealerInfos.Include(d => d.Dealer).Where(c => c.DealerCode == code);

            if (data != null)
            {
                return Json(data);
            }
            return Json(data);
        }

        private Dictionary<string, float> CalculateCommission(int dealerId, int sellAmount)
        {
            Dictionary<string, float> data = new Dictionary<string, float>();
            var dealerInfo = db.DealerInfos.Include(d => d.Dealer).FirstOrDefault(c => c.DealerId == dealerId);
            List<Commission> commision = db.Commissions.ToList();
            List<SalesCommission> salesCommission = db.SalesCommissions.ToList();
            if (dealerInfo.Dealer.IsSIDC == 1)
            {
                if (dealerInfo.Designation == "A")
                {
                    foreach (var item in commision)
                    {
                        if (item.Type == "SIDC")
                        {
                            if (sellAmount >= item.FromAmount && sellAmount < item.ToAmount)
                            {
                                float comEmployee = (sellAmount * item.Percentage) / 100;
                                data.Add("EmployeeCommission", Convert.ToInt32(comEmployee));
                                data.Add("CommissionPercent", Convert.ToInt32(item.Percentage));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in salesCommission)
                    {
                        if (item.Designation == dealerInfo.Designation)
                        {
                            float comEmployee = (sellAmount * item.Percentage) / 100;
                            data.Add("EmployeeCommission", Convert.ToInt32(comEmployee));
                            data.Add("CommissionPercent", Convert.ToInt32(item.Percentage));
                        }
                    }
                }
            }
            else
            {
                data.Add("EmployeeCommission", 0);
            }
            if (dealerInfo.Dealer.IsAMC == 1)
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
            if (dealerInfo.Dealer.IsBMC == 1)
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

        [HttpPost]
        public IActionResult PostOrdinalCommission()
        {
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            string currentMonthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
            DateTime fromDate = Convert.ToDateTime(month + "-" + "01-" + year);
            DateTime toDate = Convert.ToDateTime(month + "-" + currentMonthDays + "-" + year);

            var results = db.Sales
                            .Where(d => d.Date >= fromDate && d.Date <= toDate)
                            .Where(c => c.DealerCode.Contains("AL") || c.DealerCode.Contains("BL"));
            int total = results.Sum(x => x.SellAmount);
            DistributeOrdinalCommission(total);
            return Redirect("Index");
        }

        private void DistributeOrdinalCommission(int total)
        {
            List<DealerInfo> dealers = db.DealerInfos.ToList();
            foreach (var item in dealers)
            {
                if (item.Designation == "Company" || item.Designation == "A" || item.Designation == "B")
                {
                    continue;
                }
                else
                {
                    int amount = GetAmountWithDesignation(total, item);
                    if (amount != 0)
                    {
                        AssociationCommission aCommission = new AssociationCommission();
                        aCommission.SaleId = 0;
                        aCommission.CommissionType = "Ordinal";
                        aCommission.GoesTo = item.DealerCode;
                        aCommission.Date = DateTime.Now;
                        aCommission.Amount = amount;
                        db.SaveChanges();
                    }
                }
            }
        }
        public int GetAmountWithDesignation(int total, DealerInfo item)
        {
            if (item.Designation == "C" && item.SellAmount >= 20000)
            {
                int amount = (int)((total * 1.5) / 100);
                return amount;
            }
            else if (item.Designation == "D" && item.SellAmount >= 30000)
            {
                int amount = (int)((total * 0.5) / 100);
                return amount;
            }
            else if (item.Designation == "E" && item.SellAmount >= 70000)
            {
                int amount = (int)((total * 6.5) / 100);
                return amount;
            }
            else if (item.Designation == "F")
            {
                int amount = (int)((total * 1.5) / 100);
                return amount;
            }
            else if (item.Designation == "G")
            {
                int amount = (int)((total * 1.5) / 100);
                return amount;
            }
            else if (item.Designation == "H")
            {
                int amount = (int)((total * 2.5) / 100);
                return amount;
            }
            else if (item.Designation == "I")
            {
                int amount = (int)((total * 1.0) / 100);
                return amount;
            }
            else if (item.Designation == "J")
            {
                int amount = (int)((total * 0.5) / 100);
                return amount;
            }
            else if (item.Designation == "K")
            {
                int amount = (int)((total * 1.0) / 100);
                return amount;
            }
            else
            {
                return 0;
            }

        }
    }
}
