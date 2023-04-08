using FlightManager.Data;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                GetUserList(1).userList = GetUserList(1).userList.Where(s => s.LastName.Contains(searchString));
            }

            return View(GetUserList(1));
        }
        [HttpPost]
        public IActionResult Index(int currentPageIndex)
        {
            return View(GetUserList(currentPageIndex));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "Employee";
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public IActionResult Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = _db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = _db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(string id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(user);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _db.Users.FindAsync(id);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private UserViewModel GetUserList(int currentPage)
        {
            int maxRowsPerPAge = 10;
            UserViewModel userModel = new UserViewModel();

            userModel.userList =  _db.Users
                .Skip((currentPage - 1) * maxRowsPerPAge).Take(maxRowsPerPAge).ToList();

            double pageCount = (double)((decimal)_db.Users.Count() / Convert.ToDecimal(maxRowsPerPAge));

            userModel.pageCount = (int)Math.Ceiling(pageCount);
            userModel.currentPageIndex = currentPage;

            return userModel;
        }
    }
}
