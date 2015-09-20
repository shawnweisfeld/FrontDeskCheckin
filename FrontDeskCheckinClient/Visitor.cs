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
            var tmp = string.Format("{0}, {1}", LastName, FirstName);

            if (!string.IsNullOrEmpty(Company))
                tmp += string.Format(" ({0})", Company);

            if (!string.IsNullOrEmpty(Sponsor))
                tmp += string.Format(": {0}", Sponsor);

            return tmp;
        }
    }
}
