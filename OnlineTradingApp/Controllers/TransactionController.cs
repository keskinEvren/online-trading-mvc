using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTradingApp.Data;
using OnlineTradingApp.Models;

namespace OnlineTradingApp
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Share).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Share)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        public IActionResult Buy()
        {
            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail");
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(
            [Bind("TransactionId,Quantity,TransactionDate,UserId,ShareId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                

                var share = await _context.Shares.FindAsync(transaction.ShareId);
                var user = await _context.Users
                    .Include(x => x.Portfolio)
                    .FirstOrDefaultAsync(x => x.UserId == transaction.UserId);

                if (transaction.Quantity > share.Quantity)
                {
                    ViewData["Error"] = "User cannot buy more than total of share exist.";
                    ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
                    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
                    return View(transaction);
                }

                if (transaction.Quantity * share.Price > user.Portfolio.Balance)
                {
                    ViewData["Error"] = "Insufficient funds!";
                    ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
                    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
                    return View(transaction);
                }


                var portfolioShare =
                    await _context.PortfolioShares.FirstOrDefaultAsync(x =>
                        x.PortfolioId == user.Portfolio.PortfolioId);
                if (portfolioShare is null)
                {
                    _context.PortfolioShares.Add(new PortfolioShare()
                    {
                        Quantity = transaction.Quantity,
                        ShareId = transaction.ShareId,
                        PortfolioId = user.Portfolio.PortfolioId,
                    });
                }
                else
                {
                    portfolioShare.Quantity += transaction.Quantity;
                }
                transaction.TransactionDate = DateTime.Now;
                _context.Add(transaction);
                share.Quantity -= transaction.Quantity;
                user.Portfolio.Balance -= transaction.Quantity * share.Price;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
            return View(transaction);
        }
        
        
               // GET: Transaction/Create
        public IActionResult Sell()
        {
            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail");
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell(
            [Bind("TransactionId,Quantity,TransactionDate,UserId,ShareId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {

                var share = await _context.Shares.FindAsync(transaction.ShareId);
                var user = await _context.Users
                    .Include(x => x.Portfolio)
                    .FirstOrDefaultAsync(x => x.UserId == transaction.UserId);
                var portfolioShare = await _context.PortfolioShares
                    .Where(x => x.PortfolioId == user.Portfolio.PortfolioId)
                    .Where(x => x.ShareId == transaction.ShareId)
                    .FirstOrDefaultAsync();

                if (portfolioShare is null)
                {
                    ViewData["Error"] = "You dont have the share in your portfolio.";
                    ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
                    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
                    return View(transaction);
                }

                if (portfolioShare.Quantity < transaction.Quantity)
                {
                    ViewData["Error"] = "You cannot sell more than total of your share.";
                    ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
                    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
                    return View(transaction);
                }

                transaction.TransactionDate = DateTime.Now;
                _context.Add(transaction);
                
                portfolioShare.Quantity -= transaction.Quantity;
                share.Quantity += transaction.Quantity;
                user.Portfolio.Balance += transaction.Quantity * share.Price;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("TransactionId,Quantity,TransactionDate,UserId,ShareId")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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

            ViewData["ShareId"] = new SelectList(_context.Shares, "ShareId", "Symbol", transaction.ShareId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserEmail", transaction.UserId);
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Share)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}