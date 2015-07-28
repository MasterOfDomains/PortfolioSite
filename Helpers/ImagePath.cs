using PortfolioSite.Models;
using System;

namespace PortfolioSite.Helpers
{
    public class ImagePath
    {
        private string AppImageRoot = "Content/Img/";

        public enum ImageCategory
        {
            Clothing,
            Other
        }

        public ImageCategory category { get; private set; }

        public ImagePath(ImageCategory category)
        {
            this.category = category;
        }

        private string GetCategory()
        {
            string catString = "";
            switch (category)
            {
                case ImageCategory.Clothing:
                    catString = "Clothing/";
                    break;
                case ImageCategory.Other:
                    catString = "Other/";
                    break;
            }
            return catString;
        }

        public string GetPath(Item item, bool includeFileName)
        {
            string returnVal;
            String path;
            if (ImageExists(item.Picture, item.Category, ImagePath.ImageCategory.Clothing, out path))
            {
                if (includeFileName)
                    returnVal = System.IO.Path.Combine(path.ToString(), item.Picture);
                else
                    returnVal = path.ToString();
            }
            else
                returnVal = AppImageRoot + GetCategory() + "placeholder.png";
            return returnVal;
        }

        private string GetPartialPath(PortfolioSite.Models.Category category,
            PortfolioSite.Helpers.ImagePath.ImageCategory imgCategory)
        {
            if (imgCategory == ImageCategory.Clothing)
            {
                string basePath = AppImageRoot + GetCategory();
                string genderDir = "Neutral/";

                if (category.HasGender)
                {
                    bool? isMale = category.IsMale;
                    if (!isMale.HasValue)
                        throw new ArgumentNullException("Invalid Gender");
                    else
                    {
                        if ((bool)category.IsMale)
                            genderDir = "Male";
                        else
                            genderDir = "Female";
                    }
                }

                string path = basePath;
                path += genderDir + "/";
                path += category.AgeGroup.Name + "/";
                path += category.CategoryName.Name + "/";
                return path;
            }
            else
                throw new Exception("Image Category Not Supported");
        }

        public bool ImageExists(string fileName, 
            PortfolioSite.Models.Category category, 
            PortfolioSite.Helpers.ImagePath.ImageCategory imgCategory,
            out String partialPath)
        {
            bool returnVal = false;
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            partialPath = GetPartialPath(category, imgCategory);
            string pathSys = partialPath.Replace("/", "\\");
            string fileSys = System.IO.Path.Combine(baseDirectory, pathSys, fileName);
            if (System.IO.File.Exists(fileSys))
                returnVal = true;
            return returnVal;
        }
    }
}