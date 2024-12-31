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
    public class ShareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShareController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Share
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shares.ToListAsync());
        }

        // GET: Share/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var share = await _context.Shares
                .FirstOrDefaultAsync(m => m.ShareId == id);
            if (share == null)
            {
                return NotFound();
            }

            return View(share);
        }
        
        public async Task<IActionResult> ShareDetails(string symbol)
        {
            var share = await _context.Shares.Where(x => x.Symbol == symbol).FirstOrDefaultAsync(); 
            return View("Details", share);
        }

        // GET: Share/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Share/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShareId,Symbol,Price,Quantity")] Share request)
        {
            if (ModelState.IsValid)
            {
                var share = await _context.Shares.Where(x => x.Symbol == request.Symbol).FirstOrDefaultAsync();
                if (share is not null)
                {
                    ViewData["Error"] = "Share symbol is already in use.";
                    return View(request);
                }
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Share/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var share = await _context.Shares.FindAsync(id);
            if (share == null)
            {
                return NotFound();
            }
            return View(share);
        }

        // POST: Share/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShareId,Symbol,Price,Quantity")] Share share)
        {
            if (id != share.ShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(share);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShareExists(share.ShareId))
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
            return View(share);
        }

        // GET: Share/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var share = await _context.Shares
                .FirstOrDefaultAsync(m => m.ShareId == id);
            if (share == null)
            {
                return NotFound();
            }

            return View(share);
        }

        // POST: Share/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var share = await _context.Shares.FindAsync(id);
            if (share != null)
            {
                _context.Shares.Remove(share);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShareExists(int id)
        {
            return _context.Shares.Any(e => e.ShareId == id);
        }
    }
}
