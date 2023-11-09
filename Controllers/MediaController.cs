using MediaCollectionMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string sortField = "Title_asc", string searchTerm = "")
        {
            // Get the total number of data objects
            int count = await _context.ScannedMedia.CountAsync();
            searchTerm = string.IsNullOrEmpty(searchTerm) ? "" : searchTerm.ToLower();
            IQueryable<ScannedMedium> query = (from sm in _context.ScannedMedia  // var media
                                               where searchTerm == "" || sm.Title.ToLower().Contains(searchTerm)
                                               select sm);
            switch (sortField)
            {
                // Ascending
                case "Title_asc":
                    query = query.OrderBy(e => e.Title);
                    break;
                case "Authors_asc":
                    query = query.OrderBy(e => e.Authors);
                    break;
                case "Categories_asc":
                    query = query.OrderBy(e => e.Categories);
                    break;
                case "PublishedDate_asc":
                    query = query.OrderBy(e => e.PublishedDate);
                    break;
                case "Publisher_asc":
                    query = query.OrderBy(e => e.Publisher);
                    break;
                case "Pages_asc":
                    query = query.OrderBy(e => e.Pages);
                    break;
                case "Isbn_asc":
                    query = query.OrderBy(e => e.Isbn);
                    break;
                case "IsRead_asc":
                    query = query.OrderBy(e => e.IsRead);
                    break;
                // Descending
                case "Title_desc":
                    query = query.OrderByDescending(e => e.Title);
                    break;
                case "Authors_desc":
                    query = query.OrderByDescending(e => e.Authors);
                    break;
                case "Categories_desc":
                    query = query.OrderByDescending(e => e.Categories);
                    break;
                case "PublishedDate_desc":
                    query = query.OrderByDescending(e => e.PublishedDate);
                    break;
                case "Publisher_desc":
                    query = query.OrderByDescending(e => e.Publisher);
                    break;
                case "Pages_desc":
                    query = query.OrderByDescending(e => e.Pages);
                    break;
                case "Isbn_desc":
                    query = query.OrderByDescending(e => e.Isbn);
                    break;
                case "IsRead_desc":
                    query = query.OrderByDescending(e => e.IsRead);
                    break;

                default:
                    query = query.OrderBy(e => e.Title);
                    break;
            }

            query = query.Skip((pageIndex - 1) * pageSize);
            query = query.Take(pageSize);

            var dataObjects = await query.ToListAsync();

            // Create a new instance of your view model
            var model = new ScannedMediumViewModel
            {
                ScannedMediaObjects = dataObjects,
                Pagination = new PaginationModel
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = count
                },
                MediaSort = new MediaSortModel
                {
                    TitleSortOrder = (sortField == "Title_asc") ? "Title_desc" : "Title_asc",
                    AuthorSortOrder = (sortField == "Authors_asc") ? "Authors_desc" : "Authors_asc",
                    CategorySortOrder = (sortField == "Categories_asc") ? "Categories_desc" : "Categories_asc",
                    PublishedDateSortOrder = (sortField == "PublishedDate_asc") ? "PublishedDate_desc" : "PublishedDate_asc",
                    PublisherSortOrder = (sortField == "Publisher_asc") ? "Publisher_desc" : "Publisher_asc",
                    PagesSortOrder = (sortField == "Pages_asc") ? "Pages_desc" : "Pages_asc",
                    ISBNSortOrder = (sortField == "ISBN_asc") ? "ISBN_desc" : "ISBN_asc",
                    IsReadSortOrder = (sortField == "IsRead_asc") ? "IsRead_desc" : "IsRead_asc",
                    LastSort = !string.IsNullOrEmpty(sortField) ? sortField : "Title_asc" 
                }
            };

            return View(model);
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
