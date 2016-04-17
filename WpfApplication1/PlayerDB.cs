using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WpfApplication1
{
    public class PlayerDB
    {
        [Key]
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }

    }
}
