using Microsoft.AspNetCore.Mvc;
using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly ApplicationDbContext _db;

    public UsersController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var users = _db.Users.ToList();
        return View(users);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(string username, string password, string[]? roles)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Username and password required";
            return View();
        }

        if (_db.Users.Any(u => u.Username == username))
        {
            ViewBag.Error = "Username already exists";
            return View();
        }

        var user = new Demo.Models.User
        {
            Username = username,
            PasswordHash = Demo.Models.User.HashPassword(password),
            Roles = (roles == null || roles.Length == 0) ? "User" : string.Join(',', roles)
        };
        _db.Users.Add(user);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var u = _db.Users.FirstOrDefault(x => x.Id == id);
        if (u == null) return NotFound();
        return View(u);
    }

    [HttpPost]
    public IActionResult Edit(int id, string? password, string[]? roles)
    {
        var u = _db.Users.FirstOrDefault(x => x.Id == id);
        if (u == null) return NotFound();
        if (!string.IsNullOrWhiteSpace(password))
        {
            u.PasswordHash = Demo.Models.User.HashPassword(password);
        }
        u.Roles = (roles == null || roles.Length == 0) ? "User" : string.Join(',', roles);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var u = _db.Users.FirstOrDefault(x => x.Id == id);
        if (u != null)
        {
            _db.Users.Remove(u);
            _db.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
