using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ProductsController : Controller
{
    private readonly InventoryDbContext _context;

    public ProductsController(InventoryDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string search, int? categoryId, string sortBy)
    {
        var products = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(search))
            products = products.Where(p => p.Name.Contains(search));

        if (categoryId.HasValue)
            products = products.Where(p => p.CategoryId == categoryId);

        products = sortBy switch
        {
            "price" => products.OrderBy(p => p.Price),
            "name" => products.OrderBy(p => p.Name),
            _ => products
        };

        return View(products.ToList());
    }

    public IActionResult Create()
    {
        // Load categories for the dropdown
        ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Products product) // Fixed incorrect model name "Products"
    {
        if (!ModelState.IsValid)
        {
            // Reload the categories if validation fails
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(product);
        }

        try
        {
            // Add and save the product
            _context.Products.Add(product);
            _context.SaveChanges(); // Fixed: Ensure database changes are saved
            Console.WriteLine("Product saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            ModelState.AddModelError("", "Something went wrong while saving the product.");
            
            // Reload categories and return the form with an error
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(product);
        }

        // Redirect to the Index action after successful save
        return RedirectToAction("Index");
    }
}