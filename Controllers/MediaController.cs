using MediaCollectionMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq.Expressions;

namespace MediaCollectionMVC.Controllers
{

    public class MediaController : Controller
    {
        private readonly MediaDbContext _context;
        //private readonly object? formCollection;

        public MediaController(MediaDbContext context)
        {
            _context = context;
        }

        // GET: ScannedMediums
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string sortField = "Title_asc", string searchTerm = "")
        {
            searchTerm = string.IsNullOrEmpty(searchTerm) ? "" : searchTerm.ToLower();

            IQueryable<ScannedMedium> query = _context.ScannedMedia
                                               .Where(x => x.Title != null && x.Title.ToLower().Contains(searchTerm))
                                               .Where(x => x.IsPrivate == false);


            var sortOptions = new Dictionary<string, Expression<Func<ScannedMedium, object>>>
            {
                {"Title_asc", e => e.Title},
                {"Authors_asc", e => e.Authors_LNFN},
                {"Categories_asc", e => e.Categories},
                {"PublishedDate_asc", e => e.PublishedDate},
                {"Publisher_asc", e => e.Publisher},
                {"Pages_asc", e => e.Pages},
                {"Isbn_asc", e => e.Isbn},
                {"IsRead_asc", e => e.IsRead},
                {"Title_desc", e => e.Title},
                {"Authors_desc", e => e.Authors_LNFN},
                {"Categories_desc", e => e.Categories},
                {"PublishedDate_desc", e => e.PublishedDate},
                {"Publisher_desc", e => e.Publisher},
                {"Pages_desc", e => e.Pages},
                {"Isbn_desc", e => e.Isbn},
                {"IsRead_desc", e => e.IsRead}
            };

            if (sortOptions.ContainsKey(sortField))
            {
                if (sortField.EndsWith("_asc"))
                {
                    query = query.OrderBy(sortOptions[sortField]);
                }
                else
                {
                    query = query.OrderByDescending(sortOptions[sortField]);
                }
            }
            else
            {
                query = query.OrderBy(e => e.Title);
            }

            var count = await query.CountAsync();

            var dataObjects = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
                    Title = (sortField == "Title_asc") ? "Title_desc" : "Title_asc",
                    Authors = (sortField == "Authors_asc") ? "Authors_desc" : "Authors_asc",
                    Categories = (sortField == "Categories_asc") ? "Categories_desc" : "Categories_asc",
                    PublishedDate = (sortField == "PublishedDate_asc") ? "PublishedDate_desc" : "PublishedDate_asc",
                    Publisher = (sortField == "Publisher_asc") ? "Publisher_desc" : "Publisher_asc",
                    Pages = (sortField == "Pages_asc") ? "Pages_desc" : "Pages_asc",
                    ISBN = (sortField == "ISBN_asc") ? "ISBN_desc" : "ISBN_asc",
                    IsRead = (sortField == "IsRead_asc") ? "IsRead_desc" : "IsRead_asc",
                    LastSort = !string.IsNullOrEmpty(sortField) ? sortField : "Title_asc",
                    LastSearch = searchTerm
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
        public IActionResult Create()  // You could alternatively use public IActionResult Create() and create the data of the View synchronously.
        {
            var model = new ScannedMedium
            {
                CategoryNames = GetDistinctValues(m => m.Categories),
                PublisherNames = GetDistinctValues(m => m.Publisher),
                QualityCategories = new List<SelectListItem>
                {
                    new SelectListItem { Value = "New", Text = "New" },
                    new SelectListItem { Value = "Used - Like New", Text = "Used - Like New" },
                    new SelectListItem { Value = "Used - Very Good", Text = "Used - Very Good" },
                    new SelectListItem { Value = "Used - Good", Text = "Used - Good" },
                    new SelectListItem { Value = "Unacceptable", Text = "Unacceptable" }
                }
            };

            return View(model);
        }

        private List<SelectListItem> GetDistinctValues(Func<ScannedMedium, string> selector)
        {
            return _context.ScannedMedia
                .Select(selector)
                .Distinct()
                .Select(value => new SelectListItem { Value = value, Text = value })
                .OrderBy(item => item.Text)
                .ToList();
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

            // Populate the PublisherNames property with the distinct PublisherNames from the database
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
        public async Task<IActionResult> Edit(int id, [Bind("MediaId,Title,Authors,Categories,PublishedDate,Publisher,Pages,Isbn,IsRead,ReadingPeriods,Comments,Summary,CoverPath,IsAudioBook,IsPaperBook,IsPdfbook,IsDonated,DonationDate,Medium,Quality,OktoDonate")] ScannedMedium scannedMedium, Microsoft.AspNetCore.Http.IFormCollection collection)
        {
            //if (id != scannedMedium.MediaId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    //Fixing up checkboxes.  This is what you get for having nullable bit fields in your db & bool? in your model.
                    scannedMedium.OktoDonate = (collection["OkToDonateCheckbox"].ToString().Split(',')[0] == "true") ? true : false;
                    scannedMedium.IsAudioBook = (collection["IsAudioBookCheckbox"].ToString().Split(',')[0] == "true") ? true : false;
                    scannedMedium.IsPaperBook = (collection["IsPaperBookCheckbox"].ToString().Split(',')[0] == "true") ? true : false;
                    scannedMedium.IsPdfbook = (collection["IsPdfbookCheckbox"].ToString().Split(',')[0] == "true") ? true : false;
                    scannedMedium.IsDonated = (collection["IsDonatedCheckbox"].ToString().Split(',')[0] == "true") ? true : false;

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
