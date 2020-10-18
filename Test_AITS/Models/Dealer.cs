using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_AITS.Models
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int DealerTypeId { get; set; }
        public int ThanaId { get; set; }

        public int IsSIDC { get; set; }
        public int IsAMC { get; set; }
        public int IsBMC { get; set; }

        public Company Company { get; set; }
        public Thana Thana { get; set; }
        public DealerType DealerType { get; set; }

    }
}
