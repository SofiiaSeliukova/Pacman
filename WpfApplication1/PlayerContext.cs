using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WpfApplication1
{
    public class PlayerContext : DbContext
    {
        public PlayerContext()
            : base("DbConnection")
        { }
        public DbSet<PlayerDB> Players { get; set; }
    }
}
