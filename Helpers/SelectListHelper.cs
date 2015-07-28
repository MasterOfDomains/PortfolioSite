using PortfolioSite.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PortfolioSite.Helpers
{
    public class SelectListHelper
    {
        private static IEnumerable<SelectListItem> CategoryNamesList(Item item, int? categoryNameId, bool includeEmpty)
        {
            PortfolioDataEntities db = new PortfolioDataEntities();
            IEnumerable<CategoryName> categoryNames = db.Items.Select(i => i.Category.CategoryName).Distinct();
            if (item != null)
                categoryNameId = item.Category.CategoryNameID;

            List<SelectListItem> categorieSelectItems = new List<SelectListItem>();
            if (includeEmpty)
                categorieSelectItems.Add(new SelectListItem { Text = "", Value = "" });
            foreach (CategoryName categoryName in categoryNames)
            {
                bool selected = false;
                if (categoryNameId.HasValue)
                {
                    if (categoryName.CategoryNameID == (int)categoryNameId)
                        selected = true;
                }
                categorieSelectItems.Add(
                    new SelectListItem
                    {
                        Text = categoryName.Name,
                        Value = categoryName.CategoryNameID.ToString(),
                        Selected = selected
                    }
               );
            }

            return categorieSelectItems;
        }

        public static IEnumerable<SelectListItem> GetAgeGroupList(int? selectedAgeGroupId, bool includeEmpty)
        {
            PortfolioDataEntities db = new PortfolioDataEntities();
            IEnumerable<AgeGroup> ageGroups = db.Categories.Select(a => a.AgeGroup).Distinct();
            List<SelectListItem> ageGroupSelectItems = new List<SelectListItem>();

            foreach (AgeGroup ageGroup in ageGroups)
            {
                bool selected = false;
                if (selectedAgeGroupId.HasValue)
                {
                    if (ageGroup.AgeGroupID == (int)selectedAgeGroupId)
                        selected = true;
                }
                ageGroupSelectItems.Add(
                    new SelectListItem
                    {
                        Text = ageGroup.Name,
                        Value = ageGroup.AgeGroupID.ToString(),
                        Selected = selected
                    }
                );
            }
            return ageGroupSelectItems;
        }

        public static IEnumerable<SelectListItem> GetCategoryList(Category selectedCategory, bool includeEmpty)
        {
            PortfolioDataEntities db = new PortfolioDataEntities();
            IEnumerable<Category> categories = db.Categories.Where(c => true);
            int? selectedCategoryId = null;
            if (selectedCategory != null)
                selectedCategoryId = selectedCategory.CategoryID;
            List<SelectListItem> categorieSelectItems = new List<SelectListItem>();

            if (includeEmpty)
                categorieSelectItems.Add(new SelectListItem { Text = "", Value = "" });
            foreach (Category currentCategory in categories)
            {
                bool selected = false;
                if (selectedCategoryId.HasValue)
                {
                    if (currentCategory.CategoryID == (int)selectedCategoryId)
                        selected = true;
                }
                string personType = " (" + PersonTypeHelper.GetPersonType(currentCategory, false) + ")";

                categorieSelectItems.Add(
                    new SelectListItem
                    {
                        Text = currentCategory.CategoryName.Name + personType,
                        Value = currentCategory.CategoryID.ToString(),
                        Selected = selected
                    }
               );
            }
            return categorieSelectItems;
        }

        public static IEnumerable<SelectListItem> GetCategoryNameList(bool includeEmpty)
        {
            IEnumerable<SelectListItem> categorieSelectItems = CategoryNamesList(null, null, includeEmpty);
            return categorieSelectItems;
        }

        public static IEnumerable<SelectListItem> GetCategoryNameList(Item selectedItem, bool includeEmpty)
        {
            IEnumerable<SelectListItem> categorieSelectItems = CategoryNamesList(selectedItem, null, includeEmpty);
            return categorieSelectItems;
        }

        public static IEnumerable<SelectListItem> GetCategoryNameList(int selectedCategoryNameId, bool includeEmpty)
        {
            IEnumerable<SelectListItem> categorieSelectItems = CategoryNamesList(null, selectedCategoryNameId, includeEmpty);
            return categorieSelectItems;
        }
    }
}