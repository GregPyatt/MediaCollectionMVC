using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollectionMVC.Controllers
{
    public class MediaCollection : Controller
    {
        // GET: MediaCollection
        public ActionResult Index()
        {
            return View();
        }

        // GET: MediaCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MediaCollection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MediaCollection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MediaCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MediaCollection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MediaCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MediaCollection/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
