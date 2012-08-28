using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;

namespace NPSP
{
    public class dbHandler
    {
        // Object for handling the database connection
        private string _connString;
        private bool _connOpen;
        private SqlConnection _connection;

        //public string OLDNewProductsConnectionString;

        public string NewProductsConnectionString;
        public string MasterDataConnectionString;
        public string PricingDBConnectionString;


        public bool connOpen
        {
            get
            {
                return _connOpen;
            }
        }
        public SqlConnection conn
        {
            get
            {
                return _connection;
            }

        }

        public dbHandler()
        {
            //NewProductsConnectionString = WebConfigurationManager.ConnectionStrings["NewProductsConnectionString"].ConnectionString;
            //MasterDataConnectionString = WebConfigurationManager.ConnectionStrings["MasterDataConnectionString"].ConnectionString;
            
            PricingDBConnectionString = WebConfigurationManager.ConnectionStrings["PricingDBConnectionString"].ConnectionString;

            //Dev Override
            NewProductsConnectionString = WebConfigurationManager.ConnectionStrings["NewProductsConnectionString"].ConnectionString;
            MasterDataConnectionString = WebConfigurationManager.ConnectionStrings["PricingDBConnectionString"].ConnectionString;


            //OLDNewProductsConnectionString = WebConfigurationManager.ConnectionStrings["NewProductsConnectionString"].ConnectionString;


            _connString = WebConfigurationManager.ConnectionStrings["PricingDBConnectionString"].ConnectionString;
            _connOpen = false;
            Connect();
        }

        

        public dbHandler(string connectionString)
        {
            _connString = connectionString;
            _connOpen = false;
            Connect();
        }

        public bool Connect()
        {
            if (_connOpen)
                return true;

            if (_connString.Length == 0)
                return false;

            try
            {
                _connection = new SqlConnection(_connString);
                _connection.Open();
                _connOpen = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return true;
        }

        public bool ConnectToPricingDB()
        {
            if (_connOpen)
            {
                Disconnect();
            }

            try
            {
                _connection = new SqlConnection(PricingDBConnectionString);
                _connection.Open();
                _connOpen = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return true;
        }

        public bool ConnectToNewProductsDB()
        {
            if (_connOpen)
            {
                Disconnect();
            }


            try
            {
                _connection = new SqlConnection(NewProductsConnectionString);
                _connection.Open();
                _connOpen = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return true;
        }


        public bool Disconnect()
        {
            if (!_connOpen)
                return true;

            _connection.Close();
            _connection.Dispose();
            _connection = null;

            _connOpen = false;

            return true;

        }

    }
}
