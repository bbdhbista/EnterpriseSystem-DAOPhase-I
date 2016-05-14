using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseSystems.Infrastructure.Model
{
    class Product
    {
        public int Identity { get; set; }
        public int OrderId { get; set; }
        public string ManufactutureName{ get; set; }

        public string ModelNumber { get; set; }

        public float Price { get; set; }
    }

}
