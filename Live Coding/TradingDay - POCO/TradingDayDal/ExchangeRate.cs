using System;
using System.Data.SqlClient;

namespace TradingDayDal
{
    public class ExchangeRate
    {
        public double EuroValue { get; set; }
        public string Symbol { get; set; }
        public TradingDay TradingDay { get; set; }

        #region POCO-Implementierung
        public int Id { get; set; }

        /// <summary>
        /// Konstruktor für das Anlegen neuer ExchangeRates
        /// </summary>
        public ExchangeRate()
        {

        }

        /// <summary>
        /// Konstruktor für das Initialisierung eíner ExchnageRate-Instanz aus der DB.
        /// </summary>
        /// <param name="id">Primärschlüsselwert der zu ladenden Instanz</param>
        public ExchangeRate(int id)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand()
                    {
                        CommandText = "SELECT * FROM ExchangeRates WHERE ID = @Id",
                        Transaction = transaction,
                        Connection = connection
                    })
                    {
                        SqlParameter parId = new SqlParameter("@Id", id);
                        command.Parameters.Add(parId);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            this.Id = Convert.ToInt32( reader["Id"]);
                            this.EuroValue = Convert.ToDouble(reader["EuroValue"]);
                            this.Symbol = reader["Symbol"].ToString();
                        }
                    }
                }
            }
        }

        public void Save()
        {
            if (this.Id > 0)
            {
                // UPDATE
            }
            else
            {
                // INSERT

                // ID aus der DB holen
                // @@SCoope_IDentity
                // Ident_Current('Tablename')
            }
        }

        public void Delete()
        {

        }
        #endregion
    }
}