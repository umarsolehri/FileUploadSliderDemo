using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slider.Models
{
    class SliderContext:DbContext
    {
        public DbSet<Gallery> gallery { get; set; }
    }
}
