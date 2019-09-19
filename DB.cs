using MySql.Data.MySqlClient;
using System;

namespace Engine
{
    namespace Advanced
    {
        /// <summary>
        /// This class manages connections to MYSQL databases and can be used to interact with them.
        /// </summary>
        public class DB
        {
            private MySqlConnection DB_CONNECTION;
            private string DB_SERVER;
            private string DB_DATABASE;
            private string DB_USER;
            private string DB_PASSWORD;

            private MySqlCommand QUERY;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="server">Server Address.</param>
            /// <param name="database">Database Name</param>
            /// <param name="user">Username</param>
            /// <param name="password">Password</param>
            public DB(string server, string database, string user, string password)
            {
                DB_CONNECTION = new MySqlConnection();

                DB_SERVER = server;
                DB_DATABASE = database;
                DB_USER = user;
                DB_PASSWORD = password;

                DB_CONNECTION.ConnectionString = String.Format("server={0};database={1};uid={2};password={3}", DB_SERVER, DB_DATABASE, DB_USER, DB_PASSWORD);
            }

            public MySqlParameterCollection Parameters
            {
                get { return QUERY.Parameters; }
                set
                {
                    foreach (MySqlParameter param in value)
                        QUERY.Parameters.Add(param);
                }
            }

            public bool OpenConnection()
            {
                try
                {
                    DB_CONNECTION.Open();
                    QUERY = new MySqlCommand() { Connection = DB_CONNECTION };
                }
                catch (MySqlException ex)
                {
                    //When handling errors, you can your application's response based on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //1042: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    switch (ex.Number)
                    {
                        case 1042:
                            throw new ServerUnavaliableException();
                        case 1045:
                            throw new IncorrectLoginException();
                    }
                    return false;
                }
                return true;
            }

            private bool CloseConnection()
            {
                try
                {
                    DB_CONNECTION.Close();
                    _refreshCommand(null);
                    return true;
                }
                catch (MySqlException ex)
                {
                    if (ex.InnerException == null)
                        throw new Exception(ex.Message);
                    throw new Exception(ex.InnerException.Message);
                }
            }

            public int ExecuteStoredProcedure(string name)
            {
                QUERY.CommandType = System.Data.CommandType.StoredProcedure;
                QUERY.CommandText = name;
                int count = QUERY.ExecuteNonQuery();

                _refreshCommand(DB_CONNECTION);
                return count;
            }

            private void _refreshCommand(MySqlConnection con)
            {
                QUERY = new MySqlCommand() { Connection = con };
            }

            #region EXCEPTIONS

            [Serializable]
            public class ServerUnavaliableException : Exception
            {
                public override string Message => "Cannot connect to server. Contact administrator !";
            }

            [Serializable]
            public class IncorrectLoginException : Exception
            {
                public override string Message => "Wrong User/Pass. Try again.";
            }

            #endregion EXCEPTIONS
        }
    }
}