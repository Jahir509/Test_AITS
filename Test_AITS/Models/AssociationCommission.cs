using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_AITS.Models
{
    public class AssociationCommission
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string CommissionType { get; set; }
        public string GoesTo { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
