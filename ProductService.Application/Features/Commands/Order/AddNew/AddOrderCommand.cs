using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Commands.Order.AddNew
{
    public class AddOrderCommand
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}

