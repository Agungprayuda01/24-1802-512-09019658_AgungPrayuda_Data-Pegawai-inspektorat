using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data_Pegawai_Inspektorat.Data;
using Data_Pegawai_Inspektorat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Data_Pegawai_Inspektorat.Controllers
{
    [Authorize]
    public class PegawaisController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PegawaisController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Pegawais
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pegawais.ToListAsync());
        }

        // GET: Pegawais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.Pegawais
                .FirstOrDefaultAsync(m => m.Nik == id);
            if (pegawai == null)
            {
                return NotFound();
            }

            return View(pegawai);
        }

        // GET: Pegawais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pegawais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nik,Name,alamat,Ttl,role,path")] Pegawai pegawai, IFormFile file)
        {
            var check = await _context.Pegawais.Where(a => a.Nik == pegawai.Nik).FirstOrDefaultAsync();
            if (check != null)
            {
                return BadRequest("nik sudah ada");
            }

            if (file == null || file.Length == 0)
            {
                return NotFound("foto tidak ada");
            }

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return BadRequest("File harus gambar dengan extensi .jpg, .jpeg, .png, or .gif extension.");
            }

            long maxSize = 2 * 1024 * 1024;

            if (file.Length > maxSize)
            {
                return BadRequest("file terlalu besar dan harus di bawah 2 MB.");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var username = User.Identity.Name;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            pegawai.path = uniqueFileName;
            pegawai.email = username;
            _context.Add(pegawai);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pegawais/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.Pegawais.FindAsync(id);
            if (pegawai == null)
            {
                return NotFound();
            }
            return View(pegawai);
        }

        // POST: Pegawais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nik,Name,alamat,Ttl,role,path,email")] Pegawai pegawai)
        {
            var user = await _userManager.FindByEmailAsync(pegawai.email);
            if (!_roleManager.RoleExistsAsync("Admin").Result) { await _roleManager.CreateAsync(new IdentityRole("Admin")); }
            
            if (id != pegawai.Nik)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var checkrole = await _userManager.IsInRoleAsync(user, "Admin");
                    var checkrole2 = await _userManager.IsInRoleAsync(user, "Staff");
                    if (checkrole)
                    {
                        var remove = await _userManager.RemoveFromRoleAsync(user, "Admin");
                        var add = await _userManager.AddToRoleAsync(user, pegawai.role);
                    }
                    if (checkrole2)
                    {
                        var remove = await _userManager.RemoveFromRoleAsync(user, "Staff");
                        var add = await _userManager.AddToRoleAsync(user, pegawai.role);
                    }
                    else
                    {
                        var add = await _userManager.AddToRoleAsync(user, pegawai.role);
                    }
                    _context.Update(pegawai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PegawaiExists(pegawai.Nik))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(pegawai);
        }

        // GET: Pegawais/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.Pegawais
                .FirstOrDefaultAsync(m => m.Nik == id);
            if (pegawai == null)
            {
                return NotFound();
            }

            return View(pegawai);
        }

        // POST: Pegawais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            
            var pegawai = await _context.Pegawais.FindAsync(id);
            if (pegawai != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", pegawai.path);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.Pegawais.Remove(pegawai);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PegawaiExists(string id)
        {
            return _context.Pegawais.Any(e => e.Nik == id);
        }
    }
}
