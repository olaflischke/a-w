using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal.Models
{
    public class Artist
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Album> Albums { get; set; } = new List<Album>();
    }
}
