using PagedList;
using PortfolioSite.Helpers;
using PortfolioSite.Models;
using PortfolioSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PortfolioSite.Controllers
{
    public class StoreController : Controller
    {
        private ClothingDataEntities db = new ClothingDataEntities();

        public ViewResult Index(int? page, string sortOrderGet, string searchStringGet,
            bool? isMaleGet, bool? isFemaleGet, int? ageGroupIdGet, int? categoryNameIdGet)
        {
            var items = from i in db.Items select i;

            bool? isMale = null;
            ViewBag.IsMaleGet = null;
            // Form post gets preference
            if (ParamHelper.GetFormParamBool("IsMale", null, this.HttpContext, out isMale))
            {
                if (isMale.HasValue)
                    ViewBag.IsMaleGet = isMale;
            }
            if (!isMale.HasValue)
            {
                if (ParamHelper.GetFormParamBool("", isMaleGet, this.HttpContext, out isMale))
                {
                    if (isMale.HasValue)
                        ViewBag.IsMaleGet = isMale;
                }

            }
            if (isMale.HasValue)
                items = items.Where(i => i.Category.IsMale == isMale);

            int? ageGroupId = null;
            if (ParamHelper.GetFormParamInt("AgeGroupId", ageGroupIdGet, this.HttpContext, out ageGroupId))
            {
                ViewBag.AgeGroupIdGet = ageGroupId;
                items = items.Where(i => i.Category.AgeGroupID == ageGroupId);
            }

            int? categoryNameId = null;
            if (ParamHelper.GetFormParamInt("CategoryNameID", categoryNameIdGet, this.HttpContext, out categoryNameId))
            {
                ViewBag.CategoryNameIdGet = categoryNameId;
                items = items.Where(i => i.Category.CategoryNameID == categoryNameId);
            }

            String searchString = null;
            if (ParamHelper.getFormParamString("SearchString", searchStringGet, this.HttpContext, out searchString))
            {
                ViewBag.SearchStringGet = searchString.ToString();
                items = items.Where(i => i.Name.Contains(searchString.ToString())
                                       || i.Blurb.Contains(searchString.ToString())
                                       || i.Category.CategoryName.Name.Contains(searchString.ToString()));
            }

            if (items.Count() == 0)
            {
                string noMatches = "Sorry but no items match your search. Please revise your expectations.";
                ViewBag.NoMatches = noMatches;
            }

            String sortOrder = null;
            if (ParamHelper.getFormParamString("SortOrder", sortOrderGet, this.HttpContext, out sortOrder))
            {
                ViewBag.SortOrderGet = sortOrder.ToString();
                if (sortOrder.ToString() == "LowestFirst")
                    items = items.OrderBy(i => i.Price);
                else
                    items = items.OrderByDescending(i => i.Price);
            }
            else
                items = items.OrderByDescending(i => i.Price);
            
            // Prepare forms for display

            if (ageGroupId.HasValue)
                ViewBag.AgeGroupId = SelectListHelper.GetAgeGroupList((int)ageGroupId, false);
            else
                ViewBag.AgeGroupId = SelectListHelper.GetAgeGroupList(null, false);

            if (categoryNameId.HasValue)
                ViewBag.CategoryNameId = SelectListHelper.GetCategoryNameList((int)categoryNameId, true);
            else
                ViewBag.CategoryNameId = SelectListHelper.GetCategoryNameList(true);

            // Set page number
            if (HttpContext.Request.HttpMethod == "POST")
                page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Details(int id)
        {
            return RedirectToAction("AddToCart", "ShoppingCart");
        }

        // GET: Store/Details/5
        public ActionResult Details(int? id)
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

            var StockEnum = item.Stocks.AsEnumerable();

            InventoryDetailGenerator InventoryGenerator = new InventoryDetailGenerator();
            List<InventoryDetail> inventoryDetails = InventoryGenerator.GetInventoryDetails(item);
            List<SelectListItem> sizeSelections = new List<SelectListItem>();

            foreach (InventoryDetail detail in inventoryDetails)
            {
                IEnumerable<string[]> sizes = detail.GetSizes(); // 1 for W, 1 for L
                string selectText = "";
                if (sizes.Count() > 1)
                {
                    foreach (string[] size in sizes)
                    {
                        selectText += size[0] + ": " + size[1] + " ";
                    }
                }
                else
                {
                    string[] value = sizes.ElementAt(0);
                    selectText += value[1] + " ";
                }
                selectText += "(" + detail.Qty + " Available)";
                sizeSelections.Add(new SelectListItem {
                    Text = selectText,
                    Value = detail.InventoryItemID.ToString()
                    }
                );
            }

            var viewModel = new DetailsAndSizesViewModel
            {
                Item = item,
                SizeString = InventoryGenerator.GetInventoryDetailString(inventoryDetails, true),
                InventoryId = sizeSelections
            };

            return View(viewModel);
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID");
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,Name,CategoryID,Price,Blurb,Picture")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", item.CategoryID);
            return View(item);
        }

        // GET: Store/Edit/5
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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", item.CategoryID);
            return View(item);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,Name,CategoryID,Price,Blurb,Picture")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", item.CategoryID);
            return View(item);
        }

        // GET: Store/Delete/5
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

        // POST: Store/Delete/5
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
