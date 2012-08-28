using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Web;

namespace NPSP.Model
{
    public class XMLDataUtilities
    {
        /// <summary>
        /// Constant path references. Perhaps pull these from AppConfig if time permits.
        /// </summary>
        public static string _XMLDataPath = "~\\XMLData\\";
                
        public static string _PricingTradeCodesXMLPath = "PricingTradeCodes.xml";
        public static string _SAPImportFilesXMLPath = "SAPImportFiles.xml";

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        public static List<Model.Objects.SAPImportFileObj> getSAPImportFilesFromXML()
        {
            List<Model.Objects.SAPImportFileObj> rtnImportFiles = new List<Model.Objects.SAPImportFileObj>();
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(_XMLDataPath + _SAPImportFilesXMLPath));
            XmlNodeList xmlTerrainNodes = xmlDoc.GetElementsByTagName("SAPImportFile");
            foreach (XmlNode curNode in xmlTerrainNodes)
            {
                Model.Objects.SAPImportFileObj curImportObj = new Model.Objects.SAPImportFileObj();
                curImportObj.HeaderName = curNode.Attributes["HeaderName"].InnerText;
                curImportObj.ImportFileName = curNode.Attributes["Name"].InnerText;
                curImportObj.ImportTableName = curNode.Attributes["ImportTableName"].InnerText;
                


                /////////////This is for the sub loops for gathering column mapping :D
                XmlNodeList xmlImportColumns = curNode.ChildNodes;
                foreach (XmlNode curColumnNode in xmlImportColumns)
                {
                    Model.Objects.ImportColumnObj curImportColumn = new Objects.ImportColumnObj();

                    string dbCol = curColumnNode.Attributes["DBColumn"].InnerText;
                    string fileCol = curColumnNode.Attributes["FileColumn"].InnerText;
                    string masterCol = curColumnNode.Attributes["MasterDBColumn"].InnerText;
                    bool primeKey = false;

                    int maxCharLen = 25;

                    if (curColumnNode.Attributes["PrimeKey"] != null)
                    {
                        if (curColumnNode.Attributes["PrimeKey"].InnerText.ToUpper() == "True".ToUpper())
                            primeKey = true;
                    }

                    if (curColumnNode.Attributes["MaxCharLength"] != null)
                    {
                        string strMaxCharLen = curColumnNode.Attributes["MaxCharLength"].InnerText;

                        if (Model.Utilities.IsNumeric(strMaxCharLen))
                            maxCharLen = Convert.ToInt32(strMaxCharLen);
                    }

                    curImportColumn.DBColumn = dbCol;
                    curImportColumn.FileColumn = fileCol;
                    curImportColumn.MasterDBColumn = masterCol;
                    curImportColumn.PrimeKey = primeKey;
                    curImportColumn.MaxCharLength = maxCharLen;

                    curImportObj.ImportColumns.Add(curImportColumn);

                }

                rtnImportFiles.Add(curImportObj);


                //    DataCards.
                //    Planet curPlanet = new DataCards.Planet();
                //    curPlanet.Name = curPlanetNode.Attributes[
                //    "Name"].InnerText;
                //    curPlanet.HeaderText = curPlanetNode.Attributes[
                //    "HeaderText"].InnerText;
                //    curPlanet.GroundSpaces =
                //    new GameEngine.GalacticComponents.PlanetSpace(Convert.ToInt32(curPlanetNode.Attributes["DefaultPlanetGroundSpaces"].InnerText));
                //    XmlNodeList xmlPlanetAttributes = curPlanetNode.ChildNodes;
                //    foreach (XmlNode curPlanetAttr in xmlPlanetAttributes)
                //    {
                //        //DataCards.Planet curPlanet = new DataCards.Planet(); 
                //        //curPlanet.Name = curPlanetNode.Attributes["Name"].InnerText; 
                //        //curPlanet.HeaderText = curPlanetNode.Attributes["HeaderText"].InnerText; 
                //        //curPlanet.Spaces = new GameEngine.GalacticComponents.PlanetSpace(Convert.ToInt32(curPlanetNode.Attributes["DefaultPlanetSpaces"].InnerText)); 
                //        if (curPlanetAttr.Name == "Description")
                //        {
                //            curPlanet.Description = curPlanetAttr.InnerText;
                //        }
                //        else
                //        {
                //            curPlanet.Description =
                //            "";
                //        }
                //    }
                //    curGalaxy.addPlanet(curPlanet);
                //}

                //XmlNodeList xmlTerrainImages = curNode.ChildNodes; 
                //foreach (XmlNode curImgNode in xmlTerrainImages) 
                //{ 
                // curWidgetData.addObjImage(new System.Drawing.Bitmap(curImgNode.Attributes["src"].InnerText)); 
                //} 
                //_terrainObjects.Add(curWidgetData); 
            }
            return rtnImportFiles;
        }





        public static List<Model.Objects.TradeCodeObj> getTradeCodesFromXML()
        {
            List<Model.Objects.TradeCodeObj> rtnTradeCodeObj = new List<Model.Objects.TradeCodeObj>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(_XMLDataPath + _PricingTradeCodesXMLPath));
            XmlNodeList xmlTerrainNodes = xmlDoc.GetElementsByTagName("PricingTradeCode");
            foreach (XmlNode curNode in xmlTerrainNodes)
            {
                Model.Objects.TradeCodeObj curTradeCodeObj = new Model.Objects.TradeCodeObj();
                curTradeCodeObj.DisplayName = curNode.Attributes["DisplayName"].InnerText;
                curTradeCodeObj.TradeName = curNode.Attributes["TradeName"].InnerText;
                curTradeCodeObj.DBColumn = curNode.Attributes["DBColumn"].InnerText;

                curTradeCodeObj.ReportDisplayName = curNode.Attributes["ReportDisplayName"].InnerText;

                curTradeCodeObj.EtcDisplay = curNode.Attributes["EtcDisplay"].InnerText;
                curTradeCodeObj.StrTradeCodes = curNode.Attributes["TradeCode"].InnerText;


                rtnTradeCodeObj.Add(curTradeCodeObj);

 
            }
            return rtnTradeCodeObj;
        }



        /// <summary>
        /// XML structure building
        /// Fun stuff like saving and reading pure XML data.
        /// Mostly for delta data for the users.
        /// </summary>
        /// <returns></returns>

        public static string getXMLFileData()
        {
            string strReturn = "";

            string FilePath = HttpContext.Current.Server.MapPath(_XMLDataPath + _PricingTradeCodesXMLPath);
            //FilePath = txtBoxInput.Text;
            if (File.Exists(FilePath))
            {
                strReturn = File.ReadAllText(FilePath);

            }

            return strReturn;
        }


        public static bool saveXMLFileData(string newXMLData)
        {
            bool blnReturn = false;

            string FilePath = HttpContext.Current.Server.MapPath(_XMLDataPath + _PricingTradeCodesXMLPath);
            //FilePath = txtBoxInput.Text;
            if (File.Exists(FilePath))
            {
                //strReturn = File.ReadAllText(FilePath);
                File.WriteAllText(FilePath, newXMLData);

                blnReturn = true;
            }

            return blnReturn;
        }
    }
}