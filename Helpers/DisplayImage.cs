using PortfolioSite.Models;
using System;
using System.Drawing;
using System.Text;

namespace PortfolioSite.Helpers
{
    public class DisplayImage
    {
        private static string AppImageRoot = "Content/Img/";

        private static int ClothingImageHeight = 300;
        private static int ClothingImageWidth = 300;
        private static string ClothingImageDir = "Clothing/";

        private static string OtherImageDir = "Other/";

        public enum ImageCategory
        {
            Clothing,
            Other
        }

        public ImageCategory category { get; private set; }

        public DisplayImage(ImageCategory category)
        {
            this.category = category;
        }

        private string GetCategoryDir()
        {
            string catString = "";
            switch (category)
            {
                case ImageCategory.Clothing:
                    catString = ClothingImageDir;
                    break;
                default:
                    catString = OtherImageDir;
                    break;
            }
            return catString;
        }

        private decimal GetRatio(decimal height, decimal width)
        {
            decimal ratio = 1.00m;

            switch (category)
            {
                case ImageCategory.Clothing:
                    ratio = height / width;
                    break;
                default:
                    ratio = height / width;
                    break;
            }

            return ratio;
        }

        public string GenerateImageName(int numChars, string origName)
        {
            string name = "";
            string extension = origName.Substring(origName.IndexOf("."));

            System.Random random = new System.Random();

            int totalChars = 0;
            char[] chars = new char[numChars];

            while (totalChars < numChars)
            {
                int rand = random.Next(48, 90);
                if (rand >= 65 || rand <= 57)
                {
                    chars[totalChars] = (char)rand;
                    totalChars++;
                    rand = random.Next(48, 90);
                }
            }
            name = new String(chars).ToString() + extension;

            return name;
        }

        public bool ResizeImage(Bitmap bitmap)
        {
            bool success = false;
            decimal targetRatio;

            switch (category)
            {
                case ImageCategory.Clothing:
                    targetRatio = GetRatio(ClothingImageHeight, ClothingImageWidth);
                    break;
                default:
                    targetRatio = GetRatio(ClothingImageHeight, ClothingImageWidth);
                    break;
            }
            decimal paramRatio = GetRatio(bitmap.Height, bitmap.Width);
            Bitmap resizingBitmap;

            if (paramRatio == targetRatio)
            {
                var size = new System.Drawing.Size(ClothingImageWidth, ClothingImageHeight);
                resizingBitmap = new Bitmap(bitmap, size);
                bitmap = resizingBitmap;
                success = true;
            }

            return success;
        }

        public string GetPath(Item item, bool includeFileName)
        {
            string returnVal;
            String path;
            if (ImageExists(item.Picture, item.Category, DisplayImage.ImageCategory.Clothing, out path))
            {
                if (includeFileName)
                    returnVal = System.IO.Path.Combine(path.ToString(), item.Picture);
                else
                    returnVal = path.ToString();
            }
            else
                returnVal = AppImageRoot + GetCategoryDir() + "placeholder.png";
            return returnVal;
        }

        public string GetPartialPath(Category category, DisplayImage.ImageCategory imgCategory)
        {
            if (imgCategory == ImageCategory.Clothing)
            {
                string basePath = AppImageRoot + GetCategoryDir();
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
                throw new NotImplementedException("Image Category Not Supported");
        }

        public bool ImageExists(string fileName, Category category, DisplayImage.ImageCategory imgCategory,
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