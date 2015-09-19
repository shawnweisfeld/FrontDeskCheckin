using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskCheckinClient
{
    public class Visitor
    {
        public int Id { get; set; }
        public Terminal Terminal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Sponsor { get; set; }
        public DateTime ArrivedAt { get; set; }
        public DateTime DepartedAt { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Company))
                return string.Format("{0}, {1}", LastName, FirstName);
            else
                return string.Format("{0}, {1} : {2}", LastName, FirstName, Company);
        }
    }
}
