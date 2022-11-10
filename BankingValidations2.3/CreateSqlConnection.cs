using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingValidations2._3
{
    class CreateSqlConnection
    {
        SqlConnection Con;
        public DataTable FetchTable()
        {
            string _connectionString = @"Server=DESKTOP-OJ9JERU\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;";
            string _tableName = "BankingDetailsLive";
            DataTable BankDetailsTable = new DataTable();
            Con = new SqlConnection(@_connectionString);
            Con.Open();
            string CmdText = $"Select * From {_tableName} ";
            SqlCommand FillTable = new SqlCommand(CmdText, Con);
            SqlDataAdapter DataAdapter = new SqlDataAdapter(FillTable);
            DataAdapter.Fill(BankDetailsTable);
            Con.Close();
            return BankDetailsTable;

        }
        public void LogResponce(string PracticeNo,string Responce)
        {
            Con.Open();
            string _tableName = "ErrorLogTableNetCashLive";
            string TableField1 = "PracticeNo";
            string TableField2 = "Error";
            string CmdText = $"INSERT INTO {_tableName}({TableField1},{TableField2}) VALUES(\'{PracticeNo}\',\'{Responce}\'); ";
            SqlCommand FillTable = new SqlCommand(CmdText, Con);
            try
            {
                FillTable.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                using (StreamWriter sw = File.CreateText(ConfigurationManager.AppSettings["DocumentPath"]))
                {
                    sw.WriteLine($"LOGGING ERROR:{e.Message}");
                }

            }
            Con.Close();
        }



    }
}

