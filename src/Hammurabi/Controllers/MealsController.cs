using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hammurabi.Data;
using Hammurabi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hammurabi.Controllers
{
    public class MealsController : Controller
    {
        private readonly RestaurantContext _context;

        public MealsController(RestaurantContext context)
        {
            _context = context;    
        }

        // GET: Meals. Add sorting Functionality: Preparation Time or Price.
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "Time_desc" : "Time";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            //return View(await _context.Meals.ToListAsync());

            var meals = from m in _context.Meals
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                meals = meals.Where(s => s.Name.Contains(searchString));
            }
            //.OrderBy(s => s.Price);
            switch (sortOrder)
            {
                case "Time_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.PreparationTime);
                    break;
                case "Time":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.PreparationTime);
                    break;
                case "Price_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.Price);
                    break;

                default:
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.Price);
                    break;
            }


            return View(await meals
                        .AsNoTracking()
                        .ToListAsync());
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> EditMenu(string sortOrder, string searchString)
        {
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "Time_desc" : "Time";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            //return View(await _context.Meals.ToListAsync());

            var meals = from m in _context.Meals
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                meals = meals.Where(s => s.Name.Contains(searchString));
            }
            //.OrderBy(s => s.Price);
            switch (sortOrder)
            {
                case "Time_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.PreparationTime);
                    break;
                case "Time":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.PreparationTime);
                    break;
                case "Price_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.Price);
                    break;

                default:
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.Price);
                    break;
            }


            return View(await meals
                        .AsNoTracking()
                        .ToListAsync());
        }

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(s => s.MealIngredients)
                    .ThenInclude(e => e.Ingredient)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Meals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,PreparationTime,Price")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(meal);
        }

        // GET: Meals/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.SingleOrDefaultAsync(m => m.ID == id);
            if (meal == null)
            {
                return NotFound();
            }
            return View(meal);
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,PreparationTime,Price")] Meal meal)
        {
            if (id != meal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(meal);
        }

        // GET: Meals/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.SingleOrDefaultAsync(m => m.ID == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }


        // POST: Meals/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meals.SingleOrDefaultAsync(m => m.ID == id);
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.ID == id);
        }

    }
}
