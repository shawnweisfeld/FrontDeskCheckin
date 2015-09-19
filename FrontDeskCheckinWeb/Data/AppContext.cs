using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskCheckinWeb.Data
{
    public class AppContext : DbContext
    {
        public AppContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
    }
}
