using DuAn1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DuAn1.Controllers
{
    public class QuanLySanPhamController : Controller
    {
		private readonly Duan1Context _context;

		public QuanLySanPhamController(Duan1Context context)
		{
			_context = context;
		}

		// GET: SanPhams
		public async Task<IActionResult> Index()
		{
			var duan1Context = _context.SanPhams.Include(s => s.MaHangNavigation).Include(s => s.MaKhuyenMaiNavigation).Include(s => s.MaMauSacNavigation);
			return View(await duan1Context.ToListAsync());
		}

		// GET: SanPhams/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var sanPham = await _context.SanPhams
				.Include(s => s.MaHangNavigation)
				.Include(s => s.MaKhuyenMaiNavigation)
				.Include(s => s.MaMauSacNavigation)
				.FirstOrDefaultAsync(m => m.MaSanPham == id);
			if (sanPham == null)
			{
				return NotFound();
			}

			return View(sanPham);
		}

		// GET: SanPhams/Create
		public IActionResult Create()
		{
			ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang");
			ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai");
			ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac");
			return View();
		}

		// POST: SanPhams/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,KichCo,DonGia,SoLuongTonKho,TrangThai,MaKhuyenMai,MaHang,MaMauSac")] SanPham sanPham)
		{
			if (ModelState.IsValid)
			{
				_context.Add(sanPham);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
			ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
			ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
			return View(sanPham);
		}

		// GET: SanPhams/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var sanPham = await _context.SanPhams.FindAsync(id);
			if (sanPham == null)
			{
				return NotFound();
			}
			ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
			ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
			ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
			return View(sanPham);
		}

		// POST: SanPhams/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("MaSanPham,TenSanPham,KichCo,DonGia,SoLuongTonKho,TrangThai,MaKhuyenMai,MaHang,MaMauSac")] SanPham sanPham)
		{
			if (id != sanPham.MaSanPham)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(sanPham);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SanPhamExists(sanPham.MaSanPham))
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
			ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", sanPham.MaHang);
			ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
			ViewData["MaMauSac"] = new SelectList(_context.MauSacs, "MaMauSac", "MaMauSac", sanPham.MaMauSac);
			return View(sanPham);
		}

		// GET: SanPhams/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var sanPham = await _context.SanPhams
				.Include(s => s.MaHangNavigation)
				.Include(s => s.MaKhuyenMaiNavigation)
				.Include(s => s.MaMauSacNavigation)
				.FirstOrDefaultAsync(m => m.MaSanPham == id);
			if (sanPham == null)
			{
				return NotFound();
			}

			return View(sanPham);
		}

		// POST: SanPhams/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var sanPham = await _context.SanPhams.FindAsync(id);
			if (sanPham != null)
			{
				_context.SanPhams.Remove(sanPham);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool SanPhamExists(string id)
		{
			return _context.SanPhams.Any(e => e.MaSanPham == id);
		}
	}
}
