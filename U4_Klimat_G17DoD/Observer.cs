using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U4_Klimat_G17DoD
{
    public class Observer
    {

        public int? id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }


        public override string ToString()
        {
            return $"{firstname} {lastname}";
        }

    }
}
