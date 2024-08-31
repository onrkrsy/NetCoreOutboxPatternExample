using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Events
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
    }
}
