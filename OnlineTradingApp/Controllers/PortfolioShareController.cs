using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTradingApp.Data;
using OnlineTradingApp.Models;

namespace OnlineTradingApp
{
    public class PortfolioShareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortfolioShareController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PortfolioShare
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PortfolioShares.Include(p => p.Portfolio).Include(p => p.Share);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PortfolioShare/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioShare = await _context.PortfolioShares
                .Include(p => p.Portfolio)
                .Include(p => p.Share)
                .FirstOrDefaultAsync(m => m.PortfolioShareId == id);
            if (portfolioShare == null)
            {
                return NotFound();
            }

            return View(portfolioShare);
        }

        // GET: PortfolioShare/Create
        public IActionResult Create()
        {
            ViewData["PortfolioId"] = new SelectList(_context.Portfolios, "PortfolioId", "PortfolioId");
            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol");
            return View();
        }

        // POST: PortfolioShare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PortfolioShareId,Quantity,PortfolioId,ShareId")] PortfolioShare portfolioShare)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portfolioShare);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PortfolioId"] = new SelectList(_context.Portfolios, "PortfolioId", "PortfolioId", portfolioShare.PortfolioId);
            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", portfolioShare.ShareId);
            return View(portfolioShare);
        }

        // GET: PortfolioShare/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioShare = await _context.PortfolioShares.FindAsync(id);
            if (portfolioShare == null)
            {
                return NotFound();
            }
            ViewData["PortfolioId"] = new SelectList(_context.Portfolios, "PortfolioId", "PortfolioId", portfolioShare.PortfolioId);
            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", portfolioShare.ShareId);
            return View(portfolioShare);
        }

        // POST: PortfolioShare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PortfolioShareId,Quantity,PortfolioId,ShareId")] PortfolioShare portfolioShare)
        {
            if (id != portfolioShare.PortfolioShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portfolioShare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioShareExists(portfolioShare.PortfolioShareId))
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
            ViewData["PortfolioId"] = new SelectList(_context.Portfolios, "PortfolioId", "PortfolioId", portfolioShare.PortfolioId);
            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", portfolioShare.ShareId);
            return View(portfolioShare);
        }

        // GET: PortfolioShare/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioShare = await _context.PortfolioShares
                .Include(p => p.Portfolio)
                .Include(p => p.Share)
                .FirstOrDefaultAsync(m => m.PortfolioShareId == id);
            if (portfolioShare == null)
            {
                return NotFound();
            }

            return View(portfolioShare);
        }

        // POST: PortfolioShare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portfolioShare = await _context.PortfolioShares.FindAsync(id);
            if (portfolioShare != null)
            {
                _context.PortfolioShares.Remove(portfolioShare);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioShareExists(int id)
        {
            return _context.PortfolioShares.Any(e => e.PortfolioShareId == id);
        }
    }
}
