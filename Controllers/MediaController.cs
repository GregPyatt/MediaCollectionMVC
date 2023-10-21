using MediaCollectionMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediaCollectionMVC.Controllers
{
    public class MediaController : Controller
    {
        private readonly MediaDbContext _context;

        public MediaController(MediaDbContext context)
        {
            _context = context;
        }

        // GET: ScannedMediums
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            // Get the total number of data objects
            int count = await _context.ScannedMedia.CountAsync();

            // Retrieve the data objects for the current page
            var dataObjects = await _context.ScannedMedia
                .OrderBy(sm => sm.Title)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            // Create a new instance of your view model
            var model = new ScannedMediumViewModel
            {
                ScannedMediaObjects = dataObjects,
                Pagination = new PaginationModel
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = count
                }
            };

            return View(model);

            //=> View(repository.Products
            //.OrderBy(p => p.ProductID)
            //.Skip((productPage - 1) * PageSize)
            //.Take(PageSize));

            //int pageSize = 3;
            //return View(await PaginatedList<ScannedMedium>.CreateAsync(_context.ScannedMedia, pageNumber ?? 1, pageSize));


            //List<ScannedMedium> dataList = new List<ScannedMedium>();
            //if (_context.ScannedMedia != null) {
            //    dataList = await _context.ScannedMedia.ToListAsync();
            //    dataList.Sort((x, y) => x.Title.CompareTo(y.Title));
            //}
            //return View(dataList);

            //IActionResult returnedValue = _context.ScannedMedia != null ?
            //    View(await _context.ScannedMedia.ToListAsync()
            //        .OrderBy(p => p.Title)) :
            //    Problem("Entity set 'MediaDbContext.ScannedMedia'  is null.");

            //return returnedValue;
            //return _context.ScannedMedia != null ?
            //              View(await _context.ScannedMedia.ToListAsync()) :
            //              Problem("Entity set 'MediaDbContext.ScannedMedia'  is null.");
        }

        // GET: ScannedMediums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScannedMedia == null)
            {
                return NotFound();
            }

            var scannedMedium = await _context.ScannedMedia
                .FirstOrDefaultAsync(m => m.MediaId == id);
            if (scannedMedium == null)
            {
                return NotFound();
            }

            return View(scannedMedium);
        }

        // GET: ScannedMediums/Create
        public async Task<IActionResult> Create()  // You could alternatively use public IActionResult Create() and create the data of the View synchronously.
        {

            // This should popuate the dropdown list for CategoryNames
            // Create a new model
            var model = new ScannedMedium();

            // Populate the CategoryNames property with the distinct categories from the database
            model.CategoryNames = _context.ScannedMedia
                .Select(m => m.Categories)
                .Distinct()
                .Select(c => new SelectListItem { Value = c, Text = c })
                .ToList();

            // Populate the PublisherNames property with the distinct publishers from the database
            model.PublisherNames = _context.ScannedMedia
                .Select(m => m.Publisher)
                .Distinct()
                .Select(p => new SelectListItem { Value = p, Text = p })
                .ToList();


            return View(model);
        }

        // POST: ScannedMediums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MediaId,Title,Authors,Categories,PublishedDate,Publisher,Pages,Isbn,IsRead,ReadingPeriods,Comments,Summary,CoverPath,IsAudioBook,IsPaperBook,IsPdfbook,IsDonated,DonationDate,Medium,Quality,OktoDonate")] ScannedMedium scannedMedium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scannedMedium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scannedMedium);
        }

        // GET: ScannedMediums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScannedMedia == null)
            {
                return NotFound();
            }

            var scannedMedium = await _context.ScannedMedia.FindAsync(id);
            if (scannedMedium == null)
            {
                return NotFound();
            }

            // This should popuate the dropdown list for CategoryNames
            // Note: You may need to put this section in the Edit(int, bind) overload below.
            scannedMedium.CategoryNames = _context.ScannedMedia
                    .Select(m => m.Categories)
                    .Distinct()
                    .Select(c => new SelectListItem { Value = c, Text = c })
                    .ToList();

            scannedMedium.PublisherNames = _context.ScannedMedia
                    .Select(m => m.Publisher)
                    .Distinct()
                    .Select(c => new SelectListItem { Value = c, Text = c })
                    .ToList();

            return View(scannedMedium);
        }

        // POST: ScannedMediums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MediaId,Title,Authors,Categories,PublishedDate,Publisher,Pages,Isbn,IsRead,ReadingPeriods,Comments,Summary,CoverPath,IsAudioBook,IsPaperBook,IsPdfbook,IsDonated,DonationDate,Medium,Quality,OktoDonate")] ScannedMedium scannedMedium)
        {
            if (id != scannedMedium.MediaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scannedMedium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScannedMediumExists(scannedMedium.MediaId))
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
            return View(scannedMedium);
        }

        // GET: ScannedMediums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScannedMedia == null)
            {
                return NotFound();
            }

            var scannedMedium = await _context.ScannedMedia
                .FirstOrDefaultAsync(m => m.MediaId == id);
            if (scannedMedium == null)
            {
                return NotFound();
            }

            return View(scannedMedium);
        }

        // POST: ScannedMediums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScannedMedia == null)
            {
                return Problem("Entity set 'MediaDbContext.ScannedMedia'  is null.");
            }
            var scannedMedium = await _context.ScannedMedia.FindAsync(id);
            if (scannedMedium != null)
            {
                _context.ScannedMedia.Remove(scannedMedium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScannedMediumExists(int id)
        {
          return (_context.ScannedMedia?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
    }
}
