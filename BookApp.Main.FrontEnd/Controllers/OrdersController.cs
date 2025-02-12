﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Orders.ServiceLayer.EfCoreSql.OrderServices;
using BookApp.Main.FrontEnd.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Main.FrontEnd.Controllers
{
    public class OrdersController : BaseTraceController
    {
        // GET: /<controller>/
        public IActionResult Index([FromServices] IDisplayOrdersService service)
        {
            var result = service.GetUsersOrders();
            SetupTraceInfo();
            return View(result);
        }

        public IActionResult ConfirmOrder(int orderId, [FromServices] IDisplayOrdersService service)
        {
            var result = service.GetOrderDetail(orderId);
            SetupTraceInfo();
            return View(result);
        }
    }
}