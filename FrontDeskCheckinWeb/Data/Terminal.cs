using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskCheckinWeb.Data
{
    public class Terminal
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string SiteName { get; set; }
        public string Building { get; set; }
        public virtual List<Visitor> Visitors { get; set; }
    }
}
