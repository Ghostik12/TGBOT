using System.Data.SQLite;
using System.Data;
using Dapper;

namespace TgBotKwork.DAL.Repositories
{
    public class TgContext
    {
        protected T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }

        protected List<T> Query<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.Query<T>(sql, parameters).ToList();
            }
        }

        protected int Execute(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.Execute(sql, parameters);
            }
        }

        protected void ExecuteEx(string sql, object parametrs = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                connection.Execute(sql, parametrs);
            }
        }
        private IDbConnection CreateConnection()
        {
            return new SQLiteConnection("Data Source = D:\\Проекты\\1kworkTGBOTOpenAi\\TgBotKwork\\TgBotKwork\\DAL\\DB\\Tg.db; Version = 3");
        }
    }
}
