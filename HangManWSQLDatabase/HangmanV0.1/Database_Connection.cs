using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/*
 * Database Connection class used to connect to the database.
 * It flushes itself due to using statement so no need to flush it.
 */
namespace HangmanV0._1
{
    class Database_Connection
    {
        public List<string> Data_Gathering()
        {
            string connection = Properties.Settings.Default.Hangman_WordsConnectionString;
            List<string> data = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();

                    SqlCommand select = new SqlCommand("SELECT Words FROM [Table]", conn);
                    SqlDataReader reader = null;

                    reader = select.ExecuteReader();

                    while (reader.Read())
                    {
                        data.Add(reader["Words"].ToString());
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.ToString());
                Data_Gathering();
                return data;
            }
        } 
      
    }
}
