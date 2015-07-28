using System;
using System.Web;

namespace PortfolioSite.Helpers
{
    public class ParamHelper
    {
        public static bool GetFormParamInt(string postParamName, int? getParam, HttpContextBase context, out int? returnVal)
        {
            bool paramFound = false;
            string strPostParam = context.Request.Form.Get(postParamName);
            int param = 0;

            if (!String.IsNullOrEmpty(strPostParam))
            {
                if (Int32.TryParse(strPostParam, out param))
                    paramFound = true;
                else
                    throw new ArgumentException("Invalid POST Parameter");
                returnVal = (int)param;
            }
            else if (!paramFound)
            {
                if (getParam.HasValue)
                {
                    paramFound = true;
                    returnVal = (int)getParam;
                }
                else
                    returnVal = null;
            }
            else
                returnVal = null;

            return paramFound;
        }

        public static bool GetFormParamBool(string postParamName, bool? getParam, HttpContextBase context, out bool? returnVal)
        {
            bool paramFound = false;
            string strPostParam = context.Request.Form.Get(postParamName);
            bool param = false;

            if (!String.IsNullOrEmpty(strPostParam))
            {
                if (Boolean.TryParse(strPostParam, out param))
                    paramFound = true;
                else
                    throw new ArgumentException("Invalid POST Parameter");
                returnVal = (bool)param;
            }
            else if (!paramFound)
            {
                if (getParam.HasValue)
                {
                    paramFound = true;
                    returnVal = (bool)getParam;
                }
                else
                    returnVal = null;
            }
            else
                returnVal = null;

            return paramFound;
        }
        
        public static bool getFormParamString(string postParamName, string getParam, HttpContextBase context, out String returnVal)
        {
            bool paramFound = false;
            string strParam = context.Request.Form.Get(postParamName);

            if (!String.IsNullOrEmpty(strParam))
            {
                paramFound = true;
                returnVal = strParam;
            }
            else if (!paramFound)
            {
                if (!String.IsNullOrEmpty(getParam))
                {
                    paramFound = true;
                    returnVal = getParam;
                }
                else
                    returnVal = null;
            }
            else
                returnVal = null;

            return paramFound;
        }
    }
}