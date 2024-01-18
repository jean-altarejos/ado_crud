using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Crud_ADO.Models;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace Crud_ADO.DAL
{
    public class Product_DAL
    {
        
        string conString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["ADOConnectionString"];


        //Get All Products
        public List<Product> GetAllProducts()

        {
            List<Product> productList = new List<Product>();

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand command = conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                conn.Open();
                sqlDA.Fill(dtProducts);
                conn.Close();

                foreach(DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product { 
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["qty"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }
            return productList;
        }

        //Insert Products
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                conn.Open();
                id = command.ExecuteNonQuery();
                conn.Close();
            }
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Product> GetProductByID(int ProductID)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach(DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["qty"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }
            return productList;
        }

        public bool UpdateProduct(Product product)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                conn.Open();
                i = command.ExecuteNonQuery();
                conn.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string DeleteProduct(int productid)
            {
            string result = "";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productid", productid);
                command.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                connection.Open();

                command.ExecuteNonQuery();
                result = command.Parameters["@OutputMessage"].Value.ToString();
                
                connection.Close();

            }
            return result;
        }
    }
}
