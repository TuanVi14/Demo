using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.Data;
using Demo.Models;

namespace Demo.Controllers;

[Authorize(Roles = "Admin")]
public class CustomerAddressesController : Controller
{
    private readonly ApplicationDbContext _db;

    public CustomerAddressesController(ApplicationDbContext db)
    {
        _db = db;
    }

    private void PopulateUsers(int? selectedUserId = null)
    {
        ViewBag.UserList = new SelectList(_db.Users.OrderBy(u => u.Username).ToList(), "Id", "Username", selectedUserId);
    }

    public IActionResult Index(int? userId)
    {
        var addresses = _db.CustomerAddresses
            .Include(a => a.User)
            .AsQueryable();

        if (userId.HasValue)
        {
            addresses = addresses.Where(a => a.UserId == userId.Value);
            ViewBag.CurrentUserId = userId.Value;
            ViewBag.CurrentUserName = _db.Users.Where(u => u.Id == userId.Value).Select(u => u.Username).FirstOrDefault();
        }

        return View(addresses.OrderBy(a => a.Id).ToList());
    }

    public IActionResult Create(int? userId)
    {
        PopulateUsers(userId);
        return View(new CustomerAddress { UserId = userId ?? 0 });
    }

    [HttpPost]
    public IActionResult Create(CustomerAddress address)
    {
        if (address.UserId == 0 || !_db.Users.Any(u => u.Id == address.UserId))
        {
            ModelState.AddModelError("UserId", "Customer is required.");
        }

        if (!ModelState.IsValid)
        {
            PopulateUsers(address.UserId == 0 ? null : address.UserId);
            return View(address);
        }

        _db.CustomerAddresses.Add(address);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index), new { userId = address.UserId });
    }

    public IActionResult Edit(int id)
    {
        var address = _db.CustomerAddresses.FirstOrDefault(a => a.Id == id);
        if (address == null) return NotFound();
        PopulateUsers(address.UserId);
        return View(address);
    }

    [HttpPost]
    public IActionResult Edit(CustomerAddress address)
    {
        if (address.UserId == 0 || !_db.Users.Any(u => u.Id == address.UserId))
        {
            ModelState.AddModelError("UserId", "Customer is required.");
        }

        if (!ModelState.IsValid)
        {
            PopulateUsers(address.UserId == 0 ? null : address.UserId);
            return View(address);
        }

        var existing = _db.CustomerAddresses.FirstOrDefault(a => a.Id == address.Id);
        if (existing == null) return NotFound();

        existing.UserId = address.UserId;
        existing.AddressLine = address.AddressLine;
        existing.City = address.City;
        existing.State = address.State;
        existing.PostalCode = address.PostalCode;
        existing.Country = address.Country;

        _db.SaveChanges();
        return RedirectToAction(nameof(Index), new { userId = address.UserId });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var address = _db.CustomerAddresses.FirstOrDefault(a => a.Id == id);
        if (address != null)
        {
            _db.CustomerAddresses.Remove(address);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}
