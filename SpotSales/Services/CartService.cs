using System;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using SpotSales.Interface;
using SpotSales.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Oracle.ManagedDataAccess.Types;

namespace SpotSales.Services
{
	public class CartService : ICartService
	{
        private readonly string _connectionString;

        public decimal discount;

        public CartService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Cart> GetCart()
        {
            List<Cart> crtList = new List<Cart>();

            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        con.Open();
                        cmd.Connection = con; // Set the command's connection
                        cmd.BindByName = true;
                        cmd.CommandText = "SELECT SHOPPERID,QUANTITY,TOTAL,SHIPPING,TAX,SUBTOTAL,ORDERPLACED FROM SS_CART WHERE CARTID = 4003";

                        using (OracleDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Cart item = new Cart
                                {
                                    ShopperID = Convert.ToInt32(rdr["SHOPPERID"]),
                                    Quantity = Convert.ToInt32(rdr["QUANTITY"]),
                                    Total = Convert.ToDecimal(rdr["TOTAL"]),
                                    Shipping = Convert.ToDecimal(rdr["SHIPPING"]),
                                    Tax = Convert.ToDecimal(rdr["TAX"]),
                                    Subtotal = Convert.ToDecimal(rdr["SUBTOTAL"]),
                                    OrderPlaced = Convert.ToInt32(rdr["ORDERPLACED"]),
                                    Discount = CalDisc()
                                };
                                crtList.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or print its details
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow the exception
            }

            return crtList;
        }

        public void PlaceOrder()
        {
            Console.WriteLine("Hello");
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                con.Open();

                using (OracleTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.BindByName = true;
                            cmd.CommandText = "UPDATE ss_cart SET orderplaced = 1 WHERE cartid = 4003";

                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Optionally, you can check the rowsAffected value to ensure the update was successful.
                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                Console.WriteLine("Update successful.");
                            }
                            else
                            {
                                transaction.Rollback();
                                Console.WriteLine("Update failed.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

        }

        public decimal CalDisc()
        {
            decimal discount = 0;

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                con.Open();

                using (OracleTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.BindByName = true;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SS_SHIP_PACKAGE.ord_disc_sf1"; // Set the name of your PL/SQL function

                            // Add input parameter
                            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = 4003;

                            // Add return parameter
                            cmd.Parameters.Add("RETURN_VALUE", OracleDbType.Decimal).Direction = ParameterDirection.ReturnValue;

                            // Execute the PL/SQL function
                            cmd.ExecuteNonQuery();

                            // Retrieve the return value
                            OracleDecimal oracleDiscount = (OracleDecimal)cmd.Parameters["RETURN_VALUE"].Value;
                            discount = oracleDiscount.IsNull ? 0 : oracleDiscount.Value;

                            transaction.Commit();
                            Console.WriteLine("Discount calculated: " + discount);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }

            return discount;
        }

    }
}

