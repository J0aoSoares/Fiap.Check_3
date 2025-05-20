using Oracle.ManagedDataAccess.Client;
using System;

namespace Fiap.Check_3.Model
{
    public class DbConnection
    {
        private static string _connectionString =
            "User Id=rm551410;Password=220205;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SID=ORCL)));";

        public static OracleConnection GetConnection()
        {
            var conn = new OracleConnection(_connectionString);
            try
            {
                conn.Open();
                Console.WriteLine("Conexão com o banco estabelecida com sucesso.");
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar ao banco: " + ex.Message);
                throw;
            }
        }
    }
}