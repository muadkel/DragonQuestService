using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace NPSP.Model
{
    public class DataUtilities
    {


        //////////////////////////////////////////////////////////////////////////////////////////////
        //Pulling from PricingDB (Select statements)



        public static Model.Objects.ManualPriceObj GetManualPriceByMatIDAndTradeName(string matID, string tradeName, string tableName)
        {
            Model.Objects.ManualPriceObj objReturn = new Objects.ManualPriceObj();

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP (1) MaterialID, Manual, TradeName, DPrice, CPrice, BPrice, APrice FROM " + tableName + " WHERE MaterialID=" + matID + " AND TradeName='" + tradeName + "'";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    objReturn.MatID = matID;
                    objReturn.tradeName = tradeName;

                    objReturn.Manual = dr["Manual"].ToString();

                    objReturn.APrice = dr["APrice"].ToString();
                    objReturn.BPrice = dr["BPrice"].ToString();
                    objReturn.CPrice = dr["CPrice"].ToString();
                    objReturn.DPrice = dr["DPrice"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return objReturn;
        }


        public static string GetDepartmentNameID(int iDepartmentID)
        {
            string strDepartmentName = "Department Name not found!";

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP 1 DepartmentName FROM tbl_Departments WHERE DepartmentID = " + iDepartmentID + "";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    strDepartmentName = dr["DepartmentName"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return strDepartmentName;
        }
           


        public static string GetMaterialFromID(int iMaterialID)
        {
            string strMaterial = "Material not found!";

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP 1 Material FROM tbl_MaterialsActive WHERE MaterialID = " + iMaterialID + "";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    strMaterial = dr["Material"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return strMaterial;
        }


        public static string GetMaterialDescriptionFromID(int iMaterialID)
        {
            string strMaterialDescription = "Description not found!";

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP 1 MaterialDescription FROM tbl_MaterialsActive WHERE MaterialID = " + iMaterialID + "";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    strMaterialDescription = dr["MaterialDescription"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return strMaterialDescription;
        }


    //Protected Function GetMaterialDescriptionFromID(ByVal iMaterialID As Integer) As String

    //    Dim strMaterialDescription As String = "Description not found!"

    //    Using objConn As New SqlConnection(ConfigurationManager.ConnectionStrings("NewProductsConnectionString").ConnectionString)

    //        Dim lkpQuery As String = "SELECT TOP 1 MaterialDescription FROM tbl_MaterialsActive WHERE MaterialID = " & iMaterialID & ";"
    //        Dim objCmd As New SqlCommand(lkpQuery, objConn)
    //        objCmd.CommandType = CommandType.Text

    //        objConn.Open()

    //        Dim objReader As SqlDataReader = objCmd.ExecuteReader(CommandBehavior.CloseConnection)

    //        If objReader.Read() Then
    //            strMaterialDescription = Convert.ToString(objReader("MaterialDescription"))

    //        End If

    //        objConn.Close()
    //        objConn.Dispose()

    //    End Using

    //    Return strMaterialDescription

    //End Function



        public static ErrorHandling.ErrorObj deleteBatchByID(int batchID)
        {//Does not delete materials attached to batch
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();


            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tbl_Batches WHERE BatchID = @BatchID ";

            cmd.Parameters.Add("@BatchID", SqlDbType.Int).Value = batchID;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static Model.Objects.ActiveMatTTObj getTTInfoFromBatches(string materialID)
        {
            Model.Objects.ActiveMatTTObj rtnTT = new Objects.ActiveMatTTObj();

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT MaterialID, Quantity, PublishTT, PublishTTCA, PriceListHeader1, PriceListHeader2, PriceListHeader3, " +
                            "PriceListHeader1PriceList, PriceListHeader2PriceList, " +
                            "PriceListHeader3PriceList FROM tbl_Mat_PLH_TradeTemplates WHERE (MaterialID = " + materialID + ") ";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnTT.PriceListHeader1 = dr["PriceListHeader1"].ToString();
                    rtnTT.PriceListHeader1PriceList = dr["PriceListHeader1PriceList"].ToString();
                    rtnTT.PriceListHeader2 = dr["PriceListHeader2"].ToString();
                    rtnTT.PriceListHeader2PriceList = dr["PriceListHeader2PriceList"].ToString();
                    rtnTT.PriceListHeader3 = dr["PriceListHeader3"].ToString();
                    rtnTT.PriceListHeader3PriceList = dr["PriceListHeader3PriceList"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnTT;

        }




        public static List<Model.ColumnAttributes> getColumnAttributesFromDB()
        {
            List<Model.ColumnAttributes> rtnColumnAttributesList = new List<ColumnAttributes>();


            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tbl_DataColumnAttributes;";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Model.ColumnAttributes curCA = new ColumnAttributes();


                    curCA.columnName = dr["ColumnName"].ToString();
                    curCA.style = dr["Style"].ToString();
                    curCA.maxLength = dr["MaxLength"].ToString();
                    curCA.displayLength = dr["DisplayLength"].ToString();
                    curCA.defaultValue = dr["DefaultValue"].ToString();


                    rtnColumnAttributesList.Add(curCA);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnColumnAttributesList;
        }



        //public static bool checkMaterialIDinTable(int materialID, string tableName)
        //{
        //    bool blnReturn = false;

        //    dbHandler dbConn = new dbHandler();


        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = dbConn.conn;
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "SELECT MaterialID FROM " + tableName + " WHERE MaterialID = " + materialID + ";";

        //    try
        //    {
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.Read())
        //        {
        //            blnReturn = true;
        //        }
        //        dr.Close();
        //        dr = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorHandling.ErrorException.UnanticipatedError(ex);
        //        //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
        //    }

        //    cmd = null;
        //    dbConn.Disconnect();

        //    return blnReturn;
        //}


        public static int getMaterialIDByMatNumAndBatchID(string strMaterial, string batchID)
        {
            int rtnMatID = -1;

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT MaterialID FROM tbl_MaterialsActive WHERE Material = " + strMaterial + " AND BatchID=" + batchID + ";";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    rtnMatID = dr.GetInt32(0);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();

            return rtnMatID;
        }


        public static int getMaterialMasterID(string strMaterial)
        {
            int rtnMatID = -1;

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID FROM tbl_Template_MaterialMaster WHERE Material = '" + strMaterial + "' ";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    rtnMatID = dr.GetInt32(0);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();

            return rtnMatID;
        }

        public static List<Model.Department> getDepartmentsFromDB()
        {
            List<Model.Department> rtnDeptList = new List<Department>();


            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tbl_Departments;";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Model.Department curDept = new Department();


                    curDept.departmentName = dr["DepartmentName"].ToString();
                    curDept.deptID = new Model.TypeConversions.SafeInt(dr["DeptID"].ToString()).intVal;
                    curDept.departmentID = new Model.TypeConversions.SafeInt(dr["DepartmentID"].ToString()).intVal;


                    rtnDeptList.Add(curDept);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnDeptList;
        }

        //Not Used right now
        public static DataSet getManageValuesDS()
        {
            DataSet dsBatch = new DataSet();

            dbHandler dbConn = new dbHandler();
            dbConn.ConnectToPricingDB();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tbl_Mang_Values;";

            try
            {

                SqlDataAdapter objDataAdapt = new SqlDataAdapter(cmd);

                objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return dsBatch;

        }

        public static int checkMaterialIsInTemplateMaster(string materialID, string tradeTemplateID)
        {
            int blnReturn = -1;


            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID FROM tbl_TradeTemplate_Materials WHERE MaterialID = " + materialID + " AND TradeTemplateID = "  + tradeTemplateID + "";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    //Pull the ID as an Int! XD
                    blnReturn = dr.GetInt32(0);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                blnReturn = -1;
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return blnReturn;
        }


        public static List<Model.Objects.MaterialTemplateObj> getMaterialsAndTemplateHeaderByColumnFromBatch(string batchID, string strColNumber)
        {
            List<Model.Objects.MaterialTemplateObj> listOfObjs = new List<Objects.MaterialTemplateObj>();


            dbHandler dbConn = new dbHandler();
            dbConn.ConnectToPricingDB();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ma.MaterialID, ma.BatchID, ma.Material, ma.PriceNoteList, ma.MaterialDescription, tbl_Mat_PLH_TradeTemplates.Quantity, tbl_Mat_PLH_TradeTemplates.PublishTT, tbl_Mat_PLH_TradeTemplates.PublishTTCA, tbl_Mat_PLH_TradeTemplates.PriceListHeader" + strColNumber + "PriceList, " +
                                "tbl_Mat_PLH_TradeTemplates.PriceListHeader" + strColNumber + " " +
                                "FROM tbl_Mat_PLH_TradeTemplates INNER JOIN " +
                                "tbl_TradeTemplates_Master ON tbl_Mat_PLH_TradeTemplates.PriceListHeader" + strColNumber + " = tbl_TradeTemplates_Master.ID RIGHT OUTER JOIN " +
                                "tbl_MaterialsActive AS ma ON tbl_Mat_PLH_TradeTemplates.MaterialID = ma.MaterialID " +
                                "WHERE (ma.BatchID = " + batchID + ")";

            try
            {

                SqlDataReader objDataReader = cmd.ExecuteReader();


                while (objDataReader.Read())
                {
                    Model.Objects.MaterialTemplateObj curMatTemplateObj = new Objects.MaterialTemplateObj();
                    //PriceListHeader" + strColNumber + "PriceList
                    curMatTemplateObj.TradeTemplate = objDataReader["PriceListHeader" + strColNumber + ""].ToString();
                    curMatTemplateObj.PriceList = objDataReader["PriceListHeader" + strColNumber + "PriceList"].ToString();
                    curMatTemplateObj.Material = objDataReader["Material"].ToString();
                    curMatTemplateObj.Description = objDataReader["MaterialDescription"].ToString();
                    curMatTemplateObj.PriceNoteList = objDataReader["PriceNoteList"].ToString();

                    curMatTemplateObj.Quantity = objDataReader["Quantity"].ToString();
                    curMatTemplateObj.Publish = objDataReader["PublishTT"].ToString();
                    curMatTemplateObj.PublishCA = objDataReader["PublishTTCA"].ToString();



                    listOfObjs.Add(curMatTemplateObj);
                }

                //objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return listOfObjs;
        }

        public static List<Model.Objects.MaterialTemplateObj> getMaterialsAndADRenatalHeaderByColumnFromBatch(string batchID, string strColNumber)
        {
            List<Model.Objects.MaterialTemplateObj> listOfObjs = new List<Objects.MaterialTemplateObj>();


            dbHandler dbConn = new dbHandler();
            dbConn.ConnectToPricingDB();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT     ma.MaterialID, ma.BatchID, ma.Material, tbl_Mat_PLH_TradeTemplates.Quantity, tbl_Mat_PLH_ADRental.PublishADRental, " +
                            "tbl_Mat_PLH_ADRental.PublishADRentalCA, tbl_Mat_PLH_ADRental.PriceListHeader" + strColNumber + "PriceList, tbl_Mat_PLH_ADRental.PriceListHeader" + strColNumber + " " +
                            "FROM         tbl_Mat_PLH_TradeTemplates RIGHT OUTER JOIN " +
                            "tbl_MaterialsActive AS ma ON tbl_Mat_PLH_TradeTemplates.MaterialID = ma.MaterialID LEFT OUTER JOIN " +
                            "tbl_Mat_PLH_ADRental INNER JOIN " +
                            "tbl_TradeTemplates_Master ON tbl_Mat_PLH_ADRental.PriceListHeader" + strColNumber + " = tbl_TradeTemplates_Master.ID ON  " +
                            "ma.MaterialID = tbl_Mat_PLH_ADRental.MaterialID " +
                            "WHERE     (ma.BatchID = " + batchID + ")";

            try
            {

                SqlDataReader objDataReader = cmd.ExecuteReader();


                while (objDataReader.Read())
                {
                    Model.Objects.MaterialTemplateObj curMatTemplateObj = new Objects.MaterialTemplateObj();
                    //PriceListHeader" + strColNumber + "PriceList
                    curMatTemplateObj.TradeTemplate = objDataReader["PriceListHeader" + strColNumber + ""].ToString();
                    curMatTemplateObj.PriceList = objDataReader["PriceListHeader" + strColNumber + "PriceList"].ToString();
                    curMatTemplateObj.Material = objDataReader["Material"].ToString();

                    curMatTemplateObj.Quantity = objDataReader["Quantity"].ToString();
                    curMatTemplateObj.Publish = objDataReader["PublishADRental"].ToString();
                    curMatTemplateObj.PublishCA = objDataReader["PublishADRentalCA"].ToString();


                    listOfObjs.Add(curMatTemplateObj);
                }

                //objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return listOfObjs;
        }


        public static bool isMaterialInSAP(string strMaterial)
        {
            bool rtnBool = false;

            string strSQLStatement = "SELECT TOP(1) RATE FROM Master_SAPImport_PR00 WHERE Material = '" + strMaterial + "'";


            List<string> strListRates = Model.DataUtilities.getColumnBySQL(strSQLStatement);

            if (strListRates.Count > 0)
            {
                rtnBool = true;
            }


            return rtnBool;
        }



        public static List<Model.Objects.MaterialTemplateObj> getHeadersByMaterial(string material, string strPriceListName)
        {
            List<Model.Objects.MaterialTemplateObj> listOfObjs = new List<Objects.MaterialTemplateObj>();


            dbHandler dbConn = new dbHandler();
            dbConn.ConnectToPricingDB();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP (3) tbl_Template_MaterialMaster.Material, tbl_TradeTemplates_Master.TradeTemplate, tbl_TradeTemplates_Master.ID, tbl_TradeTemplate_Materials.USTT, tbl_TradeTemplate_Materials.CATT, " +
                                "tbl_TradeTemplate_Materials.USAD_R, tbl_TradeTemplate_Materials.CAAD_R, tbl_TradeTemplate_Materials.DEMO " +
                                "FROM         tbl_Template_MaterialMaster INNER JOIN " +
                                "    tbl_TradeTemplate_Materials ON tbl_Template_MaterialMaster.ID = tbl_TradeTemplate_Materials.MaterialID INNER JOIN " +
                                "    tbl_TradeTemplates_Master ON tbl_TradeTemplate_Materials.TradeTemplateID = tbl_TradeTemplates_Master.ID " +
                                "WHERE     (tbl_Template_MaterialMaster.Material = N'" + material + "') ";

            try
            {

                SqlDataReader objDataReader = cmd.ExecuteReader();


                while (objDataReader.Read())
                {
                    
                    
                    Model.Objects.MaterialTemplateObj curMatTemplateObj = new Objects.MaterialTemplateObj();

                    if (strPriceListName == "TT")
                    {
                        //PriceListHeader" + strColNumber + "PriceList
                        curMatTemplateObj.ID = Convert.ToInt32(objDataReader["ID"]);
                        curMatTemplateObj.TradeTemplate = objDataReader["TradeTemplate"].ToString();
                        curMatTemplateObj.Publish = objDataReader["USTT"].ToString();
                        curMatTemplateObj.PublishCA = objDataReader["CATT"].ToString();

                        //Check if both flags are off
                        if (curMatTemplateObj.Publish != "Y" && curMatTemplateObj.PublishCA != "Y")
                        {//Clear it if both are No's
                            curMatTemplateObj = new Objects.MaterialTemplateObj();
                        }
                    }
                    else if(strPriceListName == "AD_R")
                    {
                        //PriceListHeader" + strColNumber + "PriceList
                        curMatTemplateObj.ID = Convert.ToInt32(objDataReader["ID"]);
                        curMatTemplateObj.TradeTemplate = objDataReader["TradeTemplate"].ToString();
                        curMatTemplateObj.Publish = objDataReader["USAD_R"].ToString();
                        curMatTemplateObj.PublishCA = objDataReader["CAAD_R"].ToString();

                        //Check if both flags are off
                        if (curMatTemplateObj.Publish != "Y" && curMatTemplateObj.PublishCA != "Y")
                        {//Clear it if both are No's
                            curMatTemplateObj = new Objects.MaterialTemplateObj();
                        }
                    }
                    else if (strPriceListName == "Demo")
                    {
                        //PriceListHeader" + strColNumber + "PriceList
                        curMatTemplateObj.ID = Convert.ToInt32(objDataReader["ID"]);
                        curMatTemplateObj.TradeTemplate = objDataReader["TradeTemplate"].ToString();
                        curMatTemplateObj.Publish = objDataReader["DEMO"].ToString();
                        //curMatTemplateObj.PublishCA = objDataReader["CAAD_R"].ToString();

                        //Check if both flags are off
                        //if (curMatTemplateObj.Publish == "N" && curMatTemplateObj.PublishCA == "N")
                        if (curMatTemplateObj.Publish != "Y")
                        {//Clear it if both are No's
                            curMatTemplateObj = new Objects.MaterialTemplateObj();
                        }
                    }

                    listOfObjs.Add(curMatTemplateObj);
                }

                //objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return listOfObjs;
        }


//        SELECT     TOP (3) tbl_Template_MaterialMaster.Material, tbl_TradeTemplates_Master.TradeTemplate, tbl_TradeTemplate_Materials.USTT, tbl_TradeTemplate_Materials.CATT, 
//                      tbl_TradeTemplate_Materials.USAD_R, tbl_TradeTemplate_Materials.CAAD_R, tbl_TradeTemplate_Materials.DEMO
//FROM         tbl_Template_MaterialMaster INNER JOIN
//                      tbl_TradeTemplate_Materials ON tbl_Template_MaterialMaster.ID = tbl_TradeTemplate_Materials.MaterialID INNER JOIN
//                      tbl_TradeTemplates_Master ON tbl_TradeTemplate_Materials.TradeTemplateID = tbl_TradeTemplates_Master.ID
//WHERE     (tbl_Template_MaterialMaster.Material = N'435442')

        public static List<Model.Objects.MaterialTemplateObj> getMaterialsAndDemoHeaderByColumnFromBatch(string batchID, string strColNumber)
        {
            List<Model.Objects.MaterialTemplateObj> listOfObjs = new List<Objects.MaterialTemplateObj>();


            dbHandler dbConn = new dbHandler();
            dbConn.ConnectToPricingDB();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT     ma.MaterialID, ma.BatchID, ma.Material, tbl_Mat_PLH_Demo.PublishDemo, tbl_Mat_PLH_Demo.PriceListHeader" + strColNumber + "PriceList, tbl_Mat_PLH_Demo.PriceListHeader" + strColNumber + ", " +
                                "tbl_Mat_PLH_TradeTemplates.Quantity " +
                                "FROM         tbl_Mat_PLH_TradeTemplates RIGHT OUTER JOIN " +
                                "tbl_MaterialsActive AS ma ON tbl_Mat_PLH_TradeTemplates.MaterialID = ma.MaterialID LEFT OUTER JOIN " +
                                "tbl_Mat_PLH_Demo INNER JOIN " +
                                "tbl_TradeTemplates_Master ON tbl_Mat_PLH_Demo.PriceListHeader" + strColNumber + " = tbl_TradeTemplates_Master.ID ON ma.MaterialID = tbl_Mat_PLH_Demo.MaterialID " +
                                "WHERE     (ma.BatchID = " + batchID + ")";


            try
            {

                SqlDataReader objDataReader = cmd.ExecuteReader();


                while (objDataReader.Read())
                {
                    Model.Objects.MaterialTemplateObj curMatTemplateObj = new Objects.MaterialTemplateObj();
                    //PriceListHeader" + strColNumber + "PriceList
                    curMatTemplateObj.TradeTemplate = objDataReader["PriceListHeader" + strColNumber + ""].ToString();
                    curMatTemplateObj.PriceList = objDataReader["PriceListHeader" + strColNumber + "PriceList"].ToString();
                    curMatTemplateObj.Material = objDataReader["Material"].ToString();

                    curMatTemplateObj.Quantity = objDataReader["Quantity"].ToString();
                    curMatTemplateObj.Publish = objDataReader["PublishDemo"].ToString();


                    listOfObjs.Add(curMatTemplateObj);
                }

                //objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return listOfObjs;
        }


        public static List<Model.Objects.ManageValueObj> getActiveManageValuesObj()
        {
            DataSet dsBatch = new DataSet();

            List<Model.Objects.ManageValueObj> listOfMVObjs = new List<Objects.ManageValueObj>();

            dbHandler dbConn = new dbHandler();
            dbConn.ConnectToPricingDB();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tbl_Mang_Values WHERE Active = 1";

            try
            {

                SqlDataReader objDataReader = cmd.ExecuteReader();


                while (objDataReader.Read())
                {
                    Model.Objects.ManageValueObj curMVObj = new Objects.ManageValueObj();

                    curMVObj.ID = Convert.ToInt32(objDataReader["ID"].ToString());
                    curMVObj.HeaderText = objDataReader["HeaderText"].ToString();
                    curMVObj.USPercent = objDataReader["USPercent"].ToString();
                    curMVObj.CAPercent = objDataReader["CAPercent"].ToString();
                    curMVObj.PRPercent = objDataReader["PRPercent"].ToString();


                    listOfMVObjs.Add(curMVObj);
                }

                //objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return listOfMVObjs;

        }


        public static DataSet getBatchMaterialsDSByID(string iBatchID)
        {
            DataSet dsBatch = new DataSet();

            dbHandler dbConn = new dbHandler();
            dbConn = new dbHandler(dbConn.NewProductsConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tbl_MaterialsActive WHERE BatchID = " + iBatchID + ";";

            try
            {

                SqlDataAdapter objDataAdapt = new SqlDataAdapter(cmd);

                objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return dsBatch;

        }

        public static DataSet getBatchMaterialListDSByID(string iBatchID, string newBatchID)
        {
            DataSet dsBatch = new DataSet();

            dbHandler dbConn = new dbHandler();
            dbConn = new dbHandler(dbConn.NewProductsConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT " + newBatchID + " AS BatchID, Material, MaterialDescription, ProdHierarchy AS ProductGroupCode, SalesUOI AS UOM, '" + DateTime.Now.ToString() + "' AS CreateDate  FROM tbl_MaterialsActive INNER JOIN tbl_SalesOrg ON tbl_MaterialsActive.MaterialID=tbl_SalesOrg.MaterialID WHERE BatchID = " + iBatchID + ";";

            try
            {

                SqlDataAdapter objDataAdapt = new SqlDataAdapter(cmd);

                objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return dsBatch;

        }


        public static DataSet getBatchMaterialListFieldsDSByID(string iBatchID, string newBatchID)
        {
            DataSet dsBatch = new DataSet();

            dbHandler dbConn = new dbHandler();
            dbConn = new dbHandler(dbConn.NewProductsConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT " + newBatchID + " AS BatchID, Material, ProdHierarchy AS ProductGroupCode, '" + DateTime.Now.ToString() + "' AS CreateDate  FROM tbl_MaterialsActive WHERE BatchID = " + iBatchID + ";";

            try
            {

                SqlDataAdapter objDataAdapt = new SqlDataAdapter(cmd);

                objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return dsBatch;

        }


        public static DataSet getBatchDSByID(string iBatchID)
        {//Use the New Product Database??
            DataSet dsBatch = new DataSet();

            dbHandler dbConn = new dbHandler();
            dbConn = new dbHandler(dbConn.NewProductsConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM tbl_Batches WHERE BatchID = " + iBatchID + ";";

            try
            {

                SqlDataAdapter objDataAdapt = new SqlDataAdapter(cmd);

                objDataAdapt.Fill(dsBatch);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return dsBatch;

        }


        public static string GetBUNameFromID(string buD)
        {
            string strBatchName = "BU not found!";

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT BU FROM tbl_TradeTemplate_BU WHERE ID = " + buD + ";";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    strBatchName = dr["BU"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return strBatchName;
        }


        public static Model.Objects.BUObj GetBUObjFromID(string buD)
        {
            Model.Objects.BUObj objBatchName = new Objects.BUObj();

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, BU, Notes  FROM tbl_TradeTemplate_BU WHERE ID = " + buD + ";";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    objBatchName.BU = dr["BU"].ToString();
                    objBatchName.Notes = dr["Notes"].ToString();
                    objBatchName.ID = Convert.ToInt32(dr["ID"]);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return objBatchName;
        }

        public static string GetBatchNameFromID(string iBatchID)
        {
            string strBatchName = "Batch not found!";

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP 1 BatchName FROM tbl_Batches WHERE BatchID = " + iBatchID + ";";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    strBatchName = dr["BatchName"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return strBatchName;
        }


        public static List<Model.Objects.MaterialTemplateObj> GetMaterialTemplateObjsFromTemplateID(string ID, Model.Objects.PriceListObj curPLO)
        {
            List<Model.Objects.MaterialTemplateObj> rtnTT = new List<Model.Objects.MaterialTemplateObj>();

            dbHandler dbConn = new dbHandler();

            if (curPLO.Name == "USDemo" || curPLO.Name == "CADemo" || curPLO.Name == "PRDemo")
            {
                curPLO.Name = "Demo";
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT tbl_TradeTemplate_Materials.MaterialID, tbl_TradeTemplate_Materials.ID, tbl_TradeTemplate_Materials.Sequence, tbl_Template_MaterialMaster.Material, " +
                              "tbl_Template_MaterialMaster.Description, tbl_Template_MaterialMaster.Notes, tbl_Template_MaterialMaster.Quantity " +
                              "FROM tbl_TradeTemplate_Materials INNER JOIN " +
                              "tbl_Template_MaterialMaster ON tbl_TradeTemplate_Materials.MaterialID = tbl_Template_MaterialMaster.ID " +
                              "WHERE (tbl_TradeTemplate_Materials.TradeTemplateID = " + ID + ")  AND [" + curPLO.Name + "] = 'Y'" +
                              "ORDER BY CASE WHEN 1 = IsNumeric(Sequence) THEN CAST(Sequence AS DECIMAL(18, 1)) ELSE 0 END, tbl_TradeTemplate_Materials.Sequence";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Model.Objects.MaterialTemplateObj curMatTempObj = new Objects.MaterialTemplateObj();

                    curMatTempObj.Material = dr["Material"].ToString();
                    curMatTempObj.Description = dr["Description"].ToString();
                    curMatTempObj.Notes = dr["Notes"].ToString();
                    curMatTempObj.Sequence = dr["Sequence"].ToString();
                    curMatTempObj.Quantity = dr["Quantity"].ToString();
                    curMatTempObj.ID = Convert.ToInt32(dr["ID"]);


                    rtnTT.Add(curMatTempObj);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnTT;
        }


        public static Model.Objects.MaterialTemplateObj GetMaterialTemplateObjFromID(string MaterialID)
        {
            Model.Objects.MaterialTemplateObj rtnTT = new Objects.MaterialTemplateObj();

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT Material, Notes, Description, ID, Quantity FROM tbl_Template_MaterialMaster WHERE ID = " + MaterialID + " ";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnTT.Material = dr["Material"].ToString();
                    rtnTT.Description = dr["Description"].ToString();
                    rtnTT.Notes = dr["Notes"].ToString();
                    rtnTT.ID = Convert.ToInt32(dr["ID"]);
                    rtnTT.Quantity = dr["Quantity"].ToString();

                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnTT;
        }


        public static List<Model.Objects.TradeTemplateObj> GetTemplateObjectsByBUID(string ID, Model.Objects.PriceListObj curPLO)
        {
            List<Model.Objects.TradeTemplateObj> rtnTT = new List<Objects.TradeTemplateObj>();

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TradeTemplate, Notes, Sequence, BUID, ID FROM tbl_TradeTemplates_Master WHERE BUID = " + ID + " AND [" + curPLO.Name + "] = 'Y'";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Model.Objects.TradeTemplateObj curTTO = new Objects.TradeTemplateObj();

                    curTTO.BUID = Convert.ToInt32(dr["BUID"]);
                    curTTO.TradeTemplate = dr["TradeTemplate"].ToString();
                    curTTO.Notes = dr["Notes"].ToString();
                    curTTO.Sequence = dr["Sequence"].ToString();
                    curTTO.ID = Convert.ToInt32(dr["ID"]);
                    

                    rtnTT.Add(curTTO);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnTT;
        }


        public static Model.Objects.TradeTemplateObj GetTemplateObjFromID(string ID)
        {
            Model.Objects.TradeTemplateObj rtnTT = new Objects.TradeTemplateObj();

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TradeTemplate, Notes, Sequence, BUID, ID, USTT, CATT, USAD_R, CAAD_R, DEMO FROM tbl_TradeTemplates_Master WHERE ID = " + ID + " ";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnTT.BUID = Convert.ToInt32(dr["BUID"]);
                    rtnTT.TradeTemplate = dr["TradeTemplate"].ToString();
                    rtnTT.Notes = dr["Notes"].ToString();
                    rtnTT.Sequence = dr["Sequence"].ToString();
                    rtnTT.ID = Convert.ToInt32(dr["ID"]);

                    rtnTT.USTT = dr["USTT"].ToString();
                    rtnTT.CATT = dr["CATT"].ToString();
                    rtnTT.USAD_R = dr["USAD_R"].ToString();
                    rtnTT.CAAD_R = dr["CAAD_R"].ToString();
                    rtnTT.DEMO = dr["DEMO"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnTT;
        }

        public static string GetTemplateNameFromID(string ID, Model.Objects.PriceListObj curPLO)
        {
            string strTemplateName = "Batch not found!";

            dbHandler dbConn = new dbHandler();
            

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TradeTemplate FROM tbl_TradeTemplates_Master WHERE ID = " + ID + " ";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    strTemplateName = dr["TradeTemplate"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return strTemplateName;
        }


        public static string GetIDFromTemplateName(string strTradeTemplate, Model.Objects.PriceListObj curPLO)
        {
            string rtnStrID = "-1";

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID FROM " + curPLO.TableName + " WHERE TradeTemplate = '" + strTradeTemplate + "' ";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnStrID = dr["ID"].ToString();
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnStrID;
        }


        public static ArrayList getDepartmentIDsFromDatabase()
        {
            ArrayList rtnDepartmentIDs = new ArrayList();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DepartmentID FROM tbl_Departments";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnDepartmentIDs.Add(dr["DepartmentID"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnDepartmentIDs;
        }

        public static List<string> getAllUniqueMaterials()
        {
            //SELECT     Material FROM         tbl_TradeTemplate_Materials GROUP BY Material
            List<string> rtnMaterials = new List<string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT Material FROM tbl_Template_MaterialMaster ORDER BY Material";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnMaterials.Add(dr["Material"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnMaterials;
        }

        public static List<string> getColumnBySQL(string strSQL)
        {
            List<string> rtnColumnData = new List<string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSQL;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnColumnData.Add(dr[0].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnColumnData;
        }


        public static Dictionary<string, string> getHeaderDictionaryByBUIDFromDatabase(string BUID)
        {
            Dictionary<string, string> rtnUsers = new Dictionary<string, string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, BUID, TradeTemplate, Sequence, Notes, USTT, CATT, USAD_R, CAAD_R, Demo " +
                            "FROM tbl_TradeTemplates_Master " +
                            "WHERE (BUID = " + BUID + ")";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnUsers.Add(dr["TradeTemplate"].ToString(), dr["ID"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnUsers;
        }

        public static Dictionary<string, string> getHeaderDictionaryByMultiBUIDsFromDatabase(string BUIDs)
        {
            Dictionary<string, string> rtnUsers = new Dictionary<string, string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, BUID, TradeTemplate, Sequence, Notes, USTT, CATT, USAD_R, CAAD_R, Demo " +
                            "FROM tbl_TradeTemplates_Master " +
                            "WHERE (BUID IN (" + BUIDs + "))";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnUsers.Add(dr["TradeTemplate"].ToString(), dr["ID"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnUsers;
        }



        public static Dictionary<string, string> getBUDictionaryFromDatabase()
        {
            Dictionary<string, string> rtnUsers = new Dictionary<string, string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, BU FROM tbl_TradeTemplate_BU ORDER BY CASE WHEN 1 = IsNumeric(tbl_TradeTemplate_BU.Sequence) THEN CAST(tbl_TradeTemplate_BU.Sequence AS DECIMAL(18, 1)) ELSE 0 END, tbl_TradeTemplate_BU.Sequence";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnUsers.Add(dr["BU"].ToString(), dr["ID"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnUsers;
        }

        public static Dictionary<string, string> getBatchsDictionaryFromDatabase()
        {
            Dictionary<string, string> rtnUsers = new Dictionary<string, string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT BatchID, BatchName FROM tbl_Batches ORDER BY BatchName";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnUsers.Add(dr["BatchName"].ToString(), dr["BatchID"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnUsers;
        }


        public static List<Model.Objects.BUObj> getBUObjsFromDatabase()
        {
            List<Model.Objects.BUObj> rtnBUs = new List<Model.Objects.BUObj>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, BU, Notes FROM tbl_TradeTemplate_BU ORDER BY BU";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Model.Objects.BUObj curBU = new Objects.BUObj();
                    curBU.BU = dr["BU"].ToString();
                    curBU.Notes = dr["Notes"].ToString();
                    curBU.ID = Convert.ToInt32(dr["ID"]);

                    rtnBUs.Add(curBU);
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnBUs;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////
        //Updating
        public static ErrorHandling.ErrorObj updateManageValueByObj(Model.Objects.ManageValueObj myManagValObj)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE tbl_Mang_Values SET USPercent=@USPercent, CAPercent=@CAPercent, PRPercent=@PRPercent, Active=@Active WHERE ID=@ID ";

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = myManagValObj.ID;
            cmd.Parameters.Add("@USPercent", SqlDbType.NVarChar, 10).Value = myManagValObj.USPercent;
            cmd.Parameters.Add("@CAPercent", SqlDbType.NVarChar, 10).Value = myManagValObj.CAPercent;
            cmd.Parameters.Add("@PRPercent", SqlDbType.NVarChar, 10).Value = myManagValObj.PRPercent;
            cmd.Parameters.Add("@Active", SqlDbType.Bit).Value = myManagValObj.Active;

            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }

        public static ErrorHandling.ErrorObj deleteTradeTemplateMaterialsByID(int ID, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tbl_TradeTemplate_Materials WHERE TradeTemplateID=@TradeTemplateID ";

            cmd.Parameters.Add("@TradeTemplateID", SqlDbType.Int).Value = ID;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }



        public static ErrorHandling.ErrorObj deleteTradeTemplateByID(int ID, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tbl_TradeTemplates_Master WHERE ID=@ID ";

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }

        public static ErrorHandling.ErrorObj deleteMaterialTradeTemplateByID(int ID, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE tbl_TradeTemplate_Materials SET " + curPLO.Name + "='N' WHERE ID=@ID ";

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static ErrorHandling.ErrorObj insertTradeTemplateByObj(Model.Objects.TradeTemplateObj myTTValObj, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "INSERT INTO tbl_TradeTemplates_Master (BUID, TradeTemplate, Notes, USTT, CATT, USAD_R, CAAD_R, DEMO) VALUES (@BUID, @TradeTemplate, @Notes, @USTT, @CATT, @USAD_R, @CAAD_R, @DEMO) ";

            cmd.Parameters.Add("@BUID", SqlDbType.Int).Value = myTTValObj.BUID;
            cmd.Parameters.Add("@TradeTemplate", SqlDbType.NVarChar, 150).Value = myTTValObj.TradeTemplate;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = myTTValObj.Notes;
            cmd.Parameters.Add("@USTT", SqlDbType.NVarChar, 7).Value = myTTValObj.USTT;
            cmd.Parameters.Add("@CATT", SqlDbType.NVarChar, 7).Value = myTTValObj.CATT;
            cmd.Parameters.Add("@USAD_R", SqlDbType.NVarChar, 7).Value = myTTValObj.USAD_R;
            cmd.Parameters.Add("@CAAD_R", SqlDbType.NVarChar, 7).Value = myTTValObj.CAAD_R;
            cmd.Parameters.Add("@DEMO", SqlDbType.NVarChar, 7).Value = myTTValObj.DEMO;



            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static ErrorHandling.ErrorObj insertMaterialTradeTemplateByObj(Model.Objects.MaterialTemplateObj myTTValObj, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "INSERT INTO " + curPLO.MaterialTableName + " (Material, Description, Notes, TradeTemplateID, Quantity) VALUES (@Material, @Description, @Notes, @TradeTemplateID, @Quantity) ";

            cmd.Parameters.Add("@Material", SqlDbType.Int).Value = myTTValObj.Material;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 150).Value = myTTValObj.Description;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = myTTValObj.Notes;
            cmd.Parameters.Add("@TradeTemplateID", SqlDbType.NVarChar, 200).Value = myTTValObj.TradeTemplate;
            cmd.Parameters.Add("@Quantity", SqlDbType.NVarChar, 5).Value = myTTValObj.Quantity;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }

        public static ErrorHandling.ErrorObj updateMaterialTradeTemplateByObj(Model.Objects.MaterialTemplateObj myTTValObj, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE tbl_Template_MaterialMaster SET Material=@Material, Description=@Description, Notes=@Notes, Quantity=@Quantity WHERE ID=@ID ";

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = myTTValObj.ID;
            cmd.Parameters.Add("@Material", SqlDbType.Int).Value = myTTValObj.Material;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 150).Value = myTTValObj.Description;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = myTTValObj.Notes;
            cmd.Parameters.Add("@Quantity", SqlDbType.NVarChar, 5).Value = myTTValObj.Quantity;



            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static ErrorHandling.ErrorObj updateMaterialTradeTemplateSeqByObj(string ID, string seq, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE tbl_TradeTemplate_Materials SET Sequence=@Sequence WHERE ID=" + ID + " ";

            cmd.Parameters.Add("@Sequence", SqlDbType.NVarChar, 50).Value = seq;




            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static ErrorHandling.ErrorObj updateTradeTemplateSeqByObj(string ID, string seq, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE tbl_TradeTemplates_Master SET Sequence=@Sequence WHERE ID=" + ID + " ";

            cmd.Parameters.Add("@Sequence", SqlDbType.NVarChar, 50).Value = seq;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static ErrorHandling.ErrorObj updateTradeTemplateByObj(Model.Objects.TradeTemplateObj myTTValObj, Model.Objects.PriceListObj curPLO)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE tbl_TradeTemplates_Master SET TradeTemplate=@TradeTemplate, BUID=@BUID, Notes=@Notes, USTT=@USTT, CATT=@CATT, USAD_R=@USAD_R, CAAD_R=@CAAD_R WHERE ID=@ID ";

            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = myTTValObj.ID;
            cmd.Parameters.Add("@BUID", SqlDbType.Int).Value = myTTValObj.BUID;
            cmd.Parameters.Add("@TradeTemplate", SqlDbType.NVarChar, 150).Value = myTTValObj.TradeTemplate;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = myTTValObj.Notes;
            cmd.Parameters.Add("@USTT", SqlDbType.NVarChar, 7).Value = myTTValObj.USTT;
            cmd.Parameters.Add("@CATT", SqlDbType.NVarChar, 7).Value = myTTValObj.CATT;
            cmd.Parameters.Add("@USAD_R", SqlDbType.NVarChar, 7).Value = myTTValObj.USAD_R;
            cmd.Parameters.Add("@CAAD_R", SqlDbType.NVarChar, 7).Value = myTTValObj.CAAD_R;
            cmd.Parameters.Add("@DEMO", SqlDbType.NVarChar, 7).Value = myTTValObj.DEMO;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////
        //Deleting
        public static ErrorHandling.ErrorObj deleteAllRowsInTable(string tableName)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();

            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM " + tableName + " ";


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        public static ErrorHandling.ErrorObj deleteActiveMaterialsByID(int matID)
        {
            ErrorHandling.ErrorObj rtnErrorObj = new ErrorHandling.ErrorObj();


            dbHandler dbConn = new dbHandler();
            dbConn.Disconnect();

            dbConn = new dbHandler(dbConn.PricingDBConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tbl_MaterialsActive WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_PLH_ADRental WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_PLH_Demo WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_PLH_TradeTemplates WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_TradesPrimSec WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_USOtherPrices WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_CAOtherPrices WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_USPrimaryTrade WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_CAPrimaryTrade WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_USTradePrices WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_USADRentalPrices WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_CATradePrices WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_CAADRentalPrices WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_Mat_DemoTools WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_StatusCurrent WHERE MaterialID = @MaterialID " +
                              "DELETE FROM tbl_StatusHistory WHERE MaterialID = @MaterialID";


            cmd.Parameters.Add("@MaterialID", SqlDbType.Int).Value = matID;


            try
            {
                cmd.ExecuteNonQuery();
                rtnErrorObj.hasErrors = false;
            }
            catch (Exception ex)
            {
                rtnErrorObj.hasErrors = true;
                rtnErrorObj.errorReturnMessage = ex.Message;
                //Response.Write("Error deleting Product:  " + ex.Message);
            }
            cmd = null;
            dbConn.Disconnect();


            return rtnErrorObj;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        //Inserting into PricingDB (Insert statements)
        public static Model.PricingDBInsertObj insertBatchToPricingDB(Model.BatchObj myBatchObj)
        {
            Model.PricingDBInsertObj rtnPrDBInsert = new PricingDBInsertObj();


            dbHandler dbConn = new dbHandler();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            string strSQLInsertBatch = "INSERT INTO [tbl_Batches] ([BatchName], [AssignedTo], [CreatedBy], [CreateDate], [LastUpdateDate]) VALUES (@BatchName, @AssignedTo, @CreatedBy, getdate(), getdate())";

            cmd.CommandText = strSQLInsertBatch;

            cmd.Parameters.Add("@BatchName", SqlDbType.NVarChar, 35).Value = myBatchObj.BatchName;
            cmd.Parameters.Add("@AssignedTo", SqlDbType.NVarChar, 12).Value = myBatchObj.AssignedTo;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 12).Value = myBatchObj.CreatedBy;

            try
            {
                cmd.ExecuteNonQuery();
                rtnPrDBInsert.rtnError.hasErrors = false;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //Response.Write("Error deleting Product:  " + ex.Message);
                rtnPrDBInsert.rtnError.errorReturnMessage = "Failed to load the Batch into the PricingDB. See error MSG: " + ex.Message;
                rtnPrDBInsert.rtnError.hasErrors = true;
            }
            
            cmd = null;


            /////////////////////////////////////////////////////////////////
            //Getting new ID
            //Retrieve new id
            cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT @@IDENTITY AS 'Identity'";
            try
            {
                rtnPrDBInsert.rtnID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //errorMessage = ex.Message;
                //Response.Write("Error determining new Product's ID:  " + ex.Message);
                //return -1;

                rtnPrDBInsert.rtnError.errorReturnMessage = "Failed to get the new Batch ID from the PricingDB. See error MSG: " + ex.Message;
                rtnPrDBInsert.rtnError.hasErrors = true;
            }
            

            cmd = null;
            dbConn.Disconnect();
            

            return rtnPrDBInsert;
        }

        public static bool insertMaterialIntoMasterMatTable(Model.Objects.MaterialTemplateObj myMatTempObj)
        {
            bool rtnBool = false;


            dbHandler dbConn = new dbHandler();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            string strSQLInsertBatch = "INSERT INTO tbl_Template_MaterialMaster (Material, Description, Notes, Quantity) VALUES (@Material, @Description, @Notes, @Quantity)";

            cmd.CommandText = strSQLInsertBatch;

            cmd.Parameters.Add("@Material", SqlDbType.Int).Value = myMatTempObj.Material;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 50).Value = myMatTempObj.Description;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 50).Value = myMatTempObj.PriceNoteList;
            cmd.Parameters.Add("@Quantity", SqlDbType.NVarChar, 50).Value = myMatTempObj.Quantity;



            try
            {
                cmd.ExecuteNonQuery();
                rtnBool = true;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //Response.Write("Error deleting Product:  " + ex.Message);
            }

            cmd = null;


            return rtnBool;
        }

        public static bool insertMaterialIntoTemplateTable(Model.Objects.MaterialTemplateObj myMatTempObj, string strColumn)
        {
           bool rtnBool = false;


            dbHandler dbConn = new dbHandler();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            string strSQLInsertBatch = "INSERT INTO tbl_TradeTemplate_Materials ([TradeTemplateID], [MaterialID], [Quantity], [Sequence], [" + strColumn + "]) VALUES (@TradeTemplateID, @MaterialID, @Quantity, @Sequence, @" + strColumn + ")";

            cmd.CommandText = strSQLInsertBatch;

            cmd.Parameters.Add("@TradeTemplateID", SqlDbType.Int).Value = Convert.ToInt32(myMatTempObj.TradeTemplate);
            cmd.Parameters.Add("@Material", SqlDbType.NVarChar, 50).Value = myMatTempObj.Material;
            cmd.Parameters.Add("@Quantity", SqlDbType.NVarChar, 50).Value = myMatTempObj.Quantity;
            cmd.Parameters.Add("@Publish", SqlDbType.NVarChar, 50).Value = myMatTempObj.Publish;

            cmd.Parameters.Add("@" + strColumn, SqlDbType.NVarChar, 2).Value = "Y";
            

            try
            {
                cmd.ExecuteNonQuery();
                rtnBool = true;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //Response.Write("Error deleting Product:  " + ex.Message);
            }

            cmd = null;


            return rtnBool;
        }

        public static void LogCurrentMaterialStatus(int BatchID, int MaterialID, int DepartmentID, int Status, string userName)
        {
            //Protected Sub LogCurrentMaterialStatus(ByVal BatchID As Integer, ByVal MaterialID As Integer, ByVal DepartmentID As Integer, ByVal Status As Integer)
            //'Only proceed if the user is authenticated
            //Dim CreatedBy As String
            //If Request.IsAuthenticated Then
            //    'Get information about the currently logged on user
            //    Dim usr As MembershipUser = Membership.GetUser
            //    If usr Is Nothing Then
            //        'Whoops, we don't know who this user is!
            //        'Exit Sub
            //        CreatedBy = "Application"
            //    End If
            //    'Read in the user's UserId value
            //    CreatedBy = CType(usr.UserName, String)
            //Else

            //    CreatedBy = "Application"
            //End If
            string CreatedBy = userName;

            dbHandler dbConn = new dbHandler();

            //'Call the sp_UpdateCurrentStatus stored procedure
            using (SqlConnection myConnection = new SqlConnection(dbConn.PricingDBConnectionString))
            {
                SqlCommand myCommand = new SqlCommand("sp_UpdateCurrentMaterialStatus", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("@BatchID", BatchID);
                myCommand.Parameters.AddWithValue("@MaterialID", MaterialID);
                myCommand.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                myCommand.Parameters.AddWithValue("@Status", Status);
                myCommand.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                myCommand.Parameters.AddWithValue("@CurrentTime", DateTime.Now);

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();

            }

        }

        
        //Utitilities
        public static DataTable runSelectSQL(string sqlString)
        {
            DataTable dtReturn = new DataTable();

            dbHandler dbConn = new dbHandler();
            //dbConn.openConn();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = sqlString;

            try
            {

                SqlDataReader reader = cmd.ExecuteReader();

                dtReturn.Load(reader);

            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return dtReturn;

        }


        public static bool runUpdateSQL(string sqlString)
        {
            List<PricingSQLParameter> myEmptyLst = new List<PricingSQLParameter>();
            return runUpdateSQL(sqlString, myEmptyLst);
        }

        public static bool runUpdateSQL(string sqlString, List<PricingSQLParameter> lstSQLParams)
        {
            bool blnReturn = true;

            dbHandler dbConn = new dbHandler();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlString;


            foreach (PricingSQLParameter curParam in lstSQLParams)
            {
                if(curParam.sqlDBType == SqlDbType.NVarChar)
                {
                    cmd.Parameters.Add(curParam.Name, curParam.sqlDBType, curParam.ValMaxLength).Value = curParam.Value;
                }
                else if (curParam.sqlDBType == SqlDbType.Int)
                {
                    cmd.Parameters.Add(curParam.Name, curParam.sqlDBType).Value = curParam.Value;
                }

            }

            //cmd.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = orderNum;
            //cmd.Parameters.Add("@OrderStatus", SqlDbType.NVarChar, 50).Value = orderStatus;

            try
            {
                cmd.ExecuteNonQuery();

                //dr.Close();
                //dr = null;
                blnReturn = true;
            }
            catch (Exception ex)
            {
                blnReturn = false;
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();



            return blnReturn;

        }



        //////////////////////////////////////////////////////////////////////////////////////////////
        //Security

        public static string GetApplicationUserName()
        {
            string applicationUserName = "";


            applicationUserName = Membership.GetUser().UserName;

            return applicationUserName;
        }

    //        Protected Function GetApplicationUserName() As String
    //    Dim applicationUserName As String = "Annonymous"
    //    If Request.IsAuthenticated Then
    //        'Get information about the currently logged on user
    //        Dim usr As MembershipUser = Membership.GetUser
    //        If usr Is Nothing Then
    //            'Whoops, we don't know who this user is!

    //        Else

    //            'Read in the user's UserId value
    //            Dim UserId As Guid = CType(usr.ProviderUserKey, Guid)

    //            applicationUserName = Membership.GetUser(UserId).UserName
    //        End If
    //    End If

    //    Return applicationUserName

    //End Function


        public static string GetApplicationUserEmail()
        {
            string applicationUserName = "jon.morin@gmail.com";

            return applicationUserName;
        }




        public static bool isUserInAdminTable(string userName)
        {
            bool rtnIsAdmin = false;

            dbHandler dbConn = new dbHandler();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID FROM tbl_PricingAdminUsers WHERE UserName = '" + userName + "';";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnIsAdmin = true;
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //ErrorHandling.ErrorException.UnanticipatedError(ex);
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnIsAdmin;
        }


        public static Dictionary<string, string> getPricingAdminUsersDictionaryFromDatabase()
        {
            Dictionary<string, string> rtnUsers = new Dictionary<string, string>();

            dbHandler dbConn = new dbHandler();

            // Check to see if user in UserAdmin table
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dbConn.conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, UserName FROM tbl_PricingAdminUsers ORDER BY UserName";

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rtnUsers.Add(dr["UserName"].ToString(), dr["ID"].ToString());
                }
                dr.Close();
                dr = null;
            }
            catch (Exception ex)
            {
                //lblSaveError.Text = "Error determining if abbreviation is duplicate:  " + ex.Message;
            }

            cmd = null;
            dbConn.Disconnect();


            return rtnUsers;
        }


    //        Protected Function GetApplicationUserEmail() As String
    //    Dim applicationUserEmail As String = "NewProducts_NoReply@hilti.com"
    //    If Request.IsAuthenticated Then
    //        'Get information about the currently logged on user
    //        Dim usr As MembershipUser = Membership.GetUser
    //        If usr Is Nothing Then
    //            'Whoops, we don't know who this user is!

    //        Else

    //            'Read in the user's UserId value
    //            Dim UserId As Guid = CType(usr.ProviderUserKey, Guid)

    //            applicationUserEmail = Membership.GetUser(UserId).Email
    //        End If
    //    End If

    //    Return applicationUserEmail

    //End Function
    }
}
