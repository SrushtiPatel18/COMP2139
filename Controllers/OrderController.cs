using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
using Assignment1.Models;

public class OrdersController : Controller
{
    private readonly InventoryDbContext _context;
    public OrdersController(InventoryDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(List<OrderItems> items) // Fixed incorrect model name "OrderItems"
    {
        if (items == null || !items.Any())
        {
            ModelState.AddModelError("", "Order cannot be empty");
            return View();
        }
        
        var order = new Orders // Fixed incorrect model name "Orders"
        {
            OrderDate = DateTime.Now,
            TotalPrice = items.Sum(i => i.Quantity * i.Price),
            OrderItems = items
        };
        
        _context.Orders.Add(order);
        _context.SaveChanges(); // Ensure database changes are saved
        
        return RedirectToAction("Index");
    }
    
    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToList();
        return View(orders);
    }
}