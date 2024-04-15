// AccountController.cs
using event_project_1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Admin()
    {
        var users = _context.Users.ToList();
        return View(users);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {
            user.AdminOverride = user.Role == "Teacher"?true:false;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([Bind("Username, Password")] User us)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == us.Username && u.Password == us.Password);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Persist the cookie
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("FullCalendar1", "Events");
        }
        ModelState.AddModelError(string.Empty, "Invalid username or password");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }


    [HttpPost]
    public async Task<IActionResult> SavePermission(int userId, bool adminOverride)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.AdminOverride = adminOverride;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin");
        }
        else
        {
            return NotFound();
        }
    }

}
