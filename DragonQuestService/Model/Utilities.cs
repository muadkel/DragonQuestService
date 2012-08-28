using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace NPSP.Model
{
    public class Utilities
    {

        public static Model.Objects.Money getLowestMoneyVal(Model.Objects.Money firstMny, Model.Objects.Money secondMny)
        {
            Model.Objects.Money rtnMoney = new Objects.Money();

            if (firstMny.getDecimalValue() < secondMny.getDecimalValue())
                rtnMoney = firstMny;
            else
                rtnMoney = secondMny;

            return rtnMoney;
        }

        public static decimal getLowestDecimalVal(decimal firstDec, decimal secondDec)
        {
            decimal rtnDecimal = -1;

            if (firstDec < secondDec)
                rtnDecimal = firstDec;
            else
                rtnDecimal = secondDec;

            return rtnDecimal;
        }

        public static Model.Objects.PriceListObj getPriceListObjByName(string ploName, Model.Objects.PriceListMangObj myPriceListMang)
        {
            Model.Objects.PriceListObj rtnPLO = new Model.Objects.PriceListObj();

            foreach (Model.Objects.PriceListObj curPLO in myPriceListMang.lstPriceObjs)
            {
                if (curPLO.Name == ploName)
                {
                    rtnPLO = curPLO;
                    break;
                }

            }

            return rtnPLO;
        }

        public static Dictionary<string, string> getPriceListTypes(List<Model.Objects.PriceListObj> rtnPLists)
        {
            Dictionary<string, string> dictPriceListDDL = new Dictionary<string, string>();

            foreach (Model.Objects.PriceListObj curPLO in rtnPLists)
            {
                dictPriceListDDL.Add(curPLO.DisplayName, curPLO.Name);
            }

            return dictPriceListDDL;
        }

        public static string getDepartmentSQLStringFromList(List<Department> deptList)
        {
            string rtnDeptString = "";


            int i = 0;
            foreach (Department curDept in deptList)
            {
                string strCommaNspace = ", ";

                if (i == deptList.Count-1)
                    strCommaNspace = "";

                rtnDeptString = rtnDeptString + "[" + curDept.departmentID.ToString() + "]" + strCommaNspace;


                i++;
            }


            return rtnDeptString;
        }


        public static string getDelimiterVal(string strDelimiter)
        {
            string rtnVal = "";

            if (strDelimiter == "{TAB}")
            {
                rtnVal = "\t";
            }
            else
                rtnVal = strDelimiter;


            return rtnVal;
        }


        public static List<string> getSAPImportTypes()
        {
            List<string> rtnImportTypes = new List<string>();

            rtnImportTypes.Add("630");
            rtnImportTypes.Add("741");


            return rtnImportTypes;
        }






        //Form helpers
        public static void setDropDownList(DropDownList myDDL, string value)
        {
            myDDL.SelectedIndex = -1;

            foreach (ListItem curLI in myDDL.Items)
            {
                if (curLI.Value == value)
                {
                    curLI.Selected = true;
                }
            }

        }

        public static void setRadioButtonList(RadioButtonList myDDL, string value)
        {
            myDDL.SelectedIndex = -1;

            foreach (ListItem curLI in myDDL.Items)
            {
                if (curLI.Value == value)
                {
                    curLI.Selected = true;
                }
            }

        }







        //Type exception helper(s)
        public static decimal safeDivide(decimal numerator, decimal denominator)
        {
            decimal rtnValue = 0;

            if (denominator > 0)
                rtnValue = numerator / denominator;


            return rtnValue;

        }


        //Value Checks
        public static bool IsDate(Object obj)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNumeric(string numberString)
        {
            if (null == numberString)
                return false;

            //Lets remove the junk that makes this function think its a string
            //Trim off any white space and remove all commas.
            //This is all I(Jon Morin) found to cause this to not validate correctly.
            //You may remove other junk the same way I did below.
            numberString = numberString.Trim();
            numberString = numberString.Replace(",", "");
            numberString = numberString.Replace("-", "");
            

            if (string.IsNullOrEmpty(numberString))
                return false;

            foreach (char c in numberString)
            {
                if (!char.IsNumber(c) && c != '.')
                    return false;
            }
            return true;
        }





        public static string GetHttpUserName()
        {
            string _userName = "";
            // Get the user name from the context
            string strTemp = HttpContext.Current.User.Identity.Name;
            _userName = strTemp.Remove(0, strTemp.LastIndexOf('\\') + 1);



            return _userName;

        }
    
    }
}
