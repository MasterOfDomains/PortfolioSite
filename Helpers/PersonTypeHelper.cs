using PortfolioSite.Models;
using System;

namespace PortfolioSite.Helpers
{
    public class PersonTypeHelper
    {
        public static string GetPersonType(Category category, bool possessive)
        {
            string personType;
            if (category.HasGender)
            {
                bool? isMaleEntry = category.IsMale;
                if (!isMaleEntry.HasValue)
                {
                    throw new ArgumentNullException("Invalid Category Object. It has gender but none is listed.");
                }
                else
                {
                    if (category.AgeGroup.Name == "Adult")
                    {
                        if ((bool)category.IsMale)
                        {
                            if (possessive)
                                personType = "Men's";
                            else
                                personType = "Men";
                        }
                        else
                        {
                            if (possessive)
                                personType = "Women's";
                            else
                                personType = "Women";
                        }
                    }
                    else if (category.AgeGroup.Name == "Child")
                    {
                        if ((bool)category.IsMale)
                        {
                            if (possessive)
                                personType = "Boys'";
                            else
                                personType = "Boy";
                        }
                        else
                        {
                            if (possessive)
                                personType = "Girls'";
                            else
                                personType = "Girl";
                        }
                    }
                    else
                        throw new ArgumentNullException("Invalid Category Object. Has gender but not Adult or Child");
                }
            }
            else
            {
                if (category.AgeGroup.Name == "Infant")
                {
                    if (possessive)
                        personType = "Infants'";
                    else
                        personType = "Infant";
                }
                else
                    throw new ArgumentNullException("Invalid Category Object. Has no gender but not Infant");
            }
            return personType;
        }
    }
}