


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_AITS.Models
{
    public class Commission
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int FromAmount { get; set; }
        public int ToAmount { get; set; }
        public float Percentage { get; set; }
    }
}
