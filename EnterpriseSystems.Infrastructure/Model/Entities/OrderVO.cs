using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseSystems.Infrastructure.Model.Entities
{
    class OrderVO
    {
        public OrderVO()
        {
            Products = new List<Product>();
        }
        public int Identity { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
