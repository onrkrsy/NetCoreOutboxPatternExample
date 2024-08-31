﻿using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderDto orderDto);
    }
}
