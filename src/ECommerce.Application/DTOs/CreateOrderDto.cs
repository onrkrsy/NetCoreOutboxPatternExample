using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
