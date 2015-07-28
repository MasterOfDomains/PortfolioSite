using PortfolioSite.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PortfolioSite.Controllers
{
    public class EditCategoriesController : Controller
    {
        private PortfolioDataEntities db = new PortfolioDataEntities();

        // GET: EditCategories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.AgeGroup).Include(c => c.CategoryName);
            return View(categories.ToList());
        }

        // GET: EditCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: EditCategories/Create
        public ActionResult Create()
        {
            ViewBag.AgeGroupID = new SelectList(db.AgeGroups, "AgeGroupID", "Name");
            ViewBag.CategoryNameID = new SelectList(db.CategoryNames, "CategoryNameID", "Name");
            return View();
        }

        // POST: EditCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,HasGender,IsMale,CategoryNameID,AgeGroupID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgeGroupID = new SelectList(db.AgeGroups, "AgeGroupID", "Name", category.AgeGroupID);
            ViewBag.CategoryNameID = new SelectList(db.CategoryNames, "CategoryNameID", "Name", category.CategoryNameID);
            return View(category);
        }

        // GET: EditCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgeGroupID = new SelectList(db.AgeGroups, "AgeGroupID", "Name", category.AgeGroupID);
            ViewBag.CategoryNameID = new SelectList(db.CategoryNames, "CategoryNameID", "Name", category.CategoryNameID);
            return View(category);
        }

        // POST: EditCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,HasGender,IsMale,CategoryNameID,AgeGroupID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgeGroupID = new SelectList(db.AgeGroups, "AgeGroupID", "Name", category.AgeGroupID);
            ViewBag.CategoryNameID = new SelectList(db.CategoryNames, "CategoryNameID", "Name", category.CategoryNameID);
            return View(category);
        }

        // GET: EditCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: EditCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
