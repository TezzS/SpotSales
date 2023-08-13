using System;
using SpotSales.Models;
using System.Collections.Generic;
using SpotSales.Interface;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace SpotSales.Services
{
	public class AdvertisementService : IAdvertisementService
    {
        private readonly string _connectionString;
        public AdvertisementService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Advertisement> GetAllAd()
        {
            List<Advertisement> adList = new List<Advertisement>();

            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        con.Open();
                        cmd.Connection = con; // Set the command's connection
                        cmd.BindByName = true;
                        cmd.CommandText = "SELECT ADVERTISEMENTID, ADVERTISEMENTTITLE, ADVERTISEMENTDESCRIPTION, PRICE, STOCK FROM SS_ADVERTISEMENT";

                        using (OracleDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Advertisement ad = new Advertisement
                                {
                                    Id = Convert.ToInt32(rdr["ADVERTISEMENTID"]),
                                    Title = rdr["ADVERTISEMENTTITLE"].ToString(),
                                    Desc = rdr["ADVERTISEMENTDESCRIPTION"].ToString(),
                                    Price = Convert.ToDecimal(rdr["PRICE"]),
                                    Stock = Convert.ToInt32(rdr["STOCK"])
                                };
                                adList.Add(ad);
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

            return adList;
        }

        public Advertisement GetAdById()
        {
            Advertisement ad = new Advertisement();
            //TODO
            return ad;
        }
        public void AddCart()
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
                            cmd.CommandType = CommandType.StoredProcedure; // Set the command type to StoredProcedure
                            cmd.CommandText = "SS_SHIP_PACKAGE.SS_AddToCart"; // Assuming SS_AddToCart is the name of your stored procedure

                            // Add input parameters
                            cmd.Parameters.Add("p_shopperId", OracleDbType.Int32).Value = 3003;
                            cmd.Parameters.Add("p_advertisementId", OracleDbType.Int32).Value = 2003;
                            cmd.Parameters.Add("p_quantity", OracleDbType.Int32).Value = 1;

                            // Execute the stored procedure
                            int rowsAffected = cmd.ExecuteNonQuery();

                            transaction.Commit();
                            Console.WriteLine("Updated.");
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
        public void ChangeName()
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
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SS_SHIP_PACKAGE.prodname_chg_sp"; // Set the name of your PL/SQL procedure

                            // Add input parameters
                            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = 2001;
                            cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = "Sherlock Holmes";

                            // Execute the PL/SQL procedure
                            int rowsAffected = cmd.ExecuteNonQuery();

                            transaction.Commit();
                            Console.WriteLine("Advertisement name updated.");
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

    }
}

