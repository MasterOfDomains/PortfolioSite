using PagedList;
using PortfolioSite.Helpers;
using PortfolioSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PortfolioSite.Controllers
{
    [Authorize(Roles = "Admin,Employer")]
    public class EditItemsController : Controller
    {
        private ClothingDataEntities db = new ClothingDataEntities();

        // GET: EditItems
        public ViewResult Index(string currentFilter, string searchString, int? page)
        {
            var items = db.Items.Include(i => i.Category);

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Name.Contains(searchString)
                                       || i.Blurb.Contains(searchString)
                                       || i.Category.CategoryName.Name.Contains(searchString));
            }

            items = items.OrderBy(i => i.Name);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: EditItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = SelectListHelper.GetCategoryList(null, false);
            return View();
        }

        // POST: EditItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,Name,CategoryID,Price,Blurb,Picture")] Item item,
            HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                Category category = db.Categories.Single(c => c.CategoryID == item.CategoryID);
                db.Items.Add(item);
                db.SaveChanges();

                if (imageFile != null)
                {
                    string imageName = System.IO.Path.GetFileName(imageFile.FileName);
                    var imagePath = new DisplayImage(DisplayImage.ImageCategory.Clothing);
                    // Property not constructed by ASP.NET
                    item.Category = db.Categories.Single(c => c.CategoryID == item.CategoryID);
                    string path = System.IO.Path.Combine(
                                        Server.MapPath("~/" + imagePath
                                            .GetPath(item, false)), imageName);
                    imageFile.SaveAs(path);
                }

                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = SelectListHelper.GetCategoryList(item.Category, false);
            return View(item);
        }

        // GET: EditItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.OriginalPicture = item.Picture;
            return View(item);
        }

        // POST: EditItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,Name,CategoryID,Price,Blurb,Picture")] Item item,
            HttpPostedFileBase imageFile, String OriginalPicture)
        {
            item.Category = db.Categories.Single(c => c.CategoryID == item.CategoryID);

            if (ModelState.IsValid)
            {
                string imageName;
                string originalImagePath;

                if (imageFile != null)
                {
                    imageName = System.IO.Path.GetFileName(imageFile.FileName);

                    var displayImage = new DisplayImage(DisplayImage.ImageCategory.Clothing);

                    originalImagePath = displayImage.GetPath(item, true);
                    string originalImageSystemPath = Server.MapPath("~/" + originalImagePath);

                    var bitmap = new Bitmap(imageFile.InputStream);

                    if (displayImage.ResizeImage(bitmap))
                    {
                        bool uniqueNameGenerated = false;
                        string newImagePartialPath = "";
                        while (!uniqueNameGenerated)
                        {
                            item.Picture = displayImage.GenerateImageName(8, item.Picture);
                            if (!displayImage.ImageExists(item.Picture, item.Category,
                                DisplayImage.ImageCategory.Clothing, out newImagePartialPath))
                                uniqueNameGenerated = true;
                        }

                        string newImageSystemPath = System.IO.Path.Combine(
                                        Server.MapPath("~/" + newImagePartialPath), item.Picture);

                        ImageConverter byteArrayConverter = new ImageConverter();

                        System.IO.File.WriteAllBytes(newImageSystemPath,
                            (byte[])byteArrayConverter.ConvertTo(bitmap, typeof(byte[])));

                        System.IO.File.Delete(originalImageSystemPath);
                    }
                    else
                    {
                        ViewBag.BadImageMessage = "The image must be Square (Height = Width).";
                        return View(item);
                    }
                }

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: EditItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: EditItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
