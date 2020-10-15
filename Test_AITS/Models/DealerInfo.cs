using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_AITS.Models
{
    public class DealerInfo
    {
        public int Id { get; set; }
        public String DealerCode { get; set; }
        public int SellAmount { get; set; }
        public int DealerId { get; set; }
        public int SalesCommission { get; set; }
        public int OrdinalCommission { get; set; }
        public int InboundCommission { get; set; }
        public int OutboundCommission { get; set; }
        public Dealer Dealer { get; set; }
    }
}
