using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ADOX;
using System.Data.OleDb;
using System.Data;

namespace Yad.Database.Server {
    public enum InitDBResult  {
        Successful,
        CreateMDBFileFailed,
        CreateDBFailed,
        
    }

    public class YadDB {

        const string FileName = "yad2.mdb";
        const string CreateStringFormat = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5";
        const string ConnectionStringFormat = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";
        const string CreateSTM = "CREATE TABLE Player (ID AUTOINCREMENT PRIMARY KEY, LOGIN nvarchar(50) UNIQUE, PASS nvarchar(50), EMAIL nvarchar(50), WINNO int DEFAULT 0, LOSSNO int DEFAULT 0)";
        const string RegisterSTM = "INSERT INTO Player (LOGIN, PASS, EMAIL) VALUES(@login, @pass, @email)";
        const string LoginSTM = "SELECT WINNO, LOSSNO FROM Player WHERE LOGIN = @login AND PASS = @pass";
        const string UpdateResultSTM = "UPDATE Player SET WINNO = @winno, LOSSNO = @lossno WHERE LOGIN = @login";
        const string RemidDataSTM = "SELECT PASS, EMAIL FROM Player WHERE LOGIN = @login";
        
        private YadDB() {
        }

        public static InitDBResult Init() {
            if (!File.Exists(FileName)) {
                if (!CreateFile())
                    return InitDBResult.CreateMDBFileFailed;
                if (!CreateDB())
                    return InitDBResult.CreateDBFailed;
            }
            return InitDBResult.Successful;
        }

        public static bool Register(string name, string password, string mail) {

            OleDbCommand ocmd = new OleDbCommand(RegisterSTM);
            ocmd.Parameters.Add("@login", OleDbType.VarWChar, 50);
            ocmd.Parameters.Add("@pass", OleDbType.VarWChar, 50);
            ocmd.Parameters.Add("@email", OleDbType.VarWChar, 50);
            ocmd.Parameters["@login"].Value = name;
            ocmd.Parameters["@pass"].Value = password;
            ocmd.Parameters["@email"].Value = mail;

            return ExecuteCommand(ocmd);
        }

        public static bool Remind(string name,  out string email, out string password){
            OleDbCommand ocmd = new OleDbCommand(RemidDataSTM);
            ocmd.Parameters.Add("@login", OleDbType.VarWChar, 50);
            ocmd.Parameters["@login"].Value = name;
            RemindQueryReader rqr = new RemindQueryReader();
            ExecuteQuery(ocmd, rqr);
            email = null;
            password = null;
            if (rqr.Result) {
                email = rqr.Email;
                password = rqr.Password;
                return true;
            }
            return false;

        }
        public static bool Login(string name, string password, ref ushort winno, ref ushort lossno) {
            OleDbCommand ocmd = new OleDbCommand(LoginSTM);

            ocmd.Parameters.Add("@login", OleDbType.VarWChar, 50);
            ocmd.Parameters.Add("@pass", OleDbType.VarWChar, 50);
            ocmd.Parameters["@login"].Value = name;
            ocmd.Parameters["@pass"].Value = password;

            using (LoginQueryReader lqr = new LoginQueryReader()) {
                ExecuteQuery(ocmd, lqr);
                winno = lqr.Winno;
                lossno = lqr.Lossno;
                if (!lqr.Result)
                    return false;
            }
            return true;
        }

        public static bool UpdatePlayerStats(string name, int winno, int lossno) {
            
            OleDbCommand ocmd = new OleDbCommand(UpdateResultSTM);
            ocmd.Parameters.Add("@winno", OleDbType.Integer);
            ocmd.Parameters.Add("@lossno", OleDbType.Integer);
            ocmd.Parameters.Add("@login", OleDbType.VarWChar, 50);
            ocmd.Parameters["@winno"].Value = winno;
            ocmd.Parameters["@lossno"].Value = lossno;
            ocmd.Parameters["@login"].Value = name;
            return ExecuteCommand(ocmd);
        }
        private static bool CreateFile() {
            ADOX.CatalogClass cat = new ADOX.CatalogClass();
            try {
                cat.Create(string.Format(CreateStringFormat, FileName));
                cat = null;
            }
            catch (Exception) {
                return false;
            }
            return true;
        }


        private static void ExecuteQuery(OleDbCommand ocmd, IQueryReader queryReader) {
            string connString = string.Format(ConnectionStringFormat, FileName);
            OleDbConnection mdbConn = new OleDbConnection(connString);

            try {
                mdbConn.Open();
                ocmd.Connection = mdbConn;
                IDataReader reader = ocmd.ExecuteReader();
                queryReader.ReadData(reader);
            }
            catch (Exception) {
                queryReader.SetFailure();
            }
            finally {
                if (mdbConn.State != ConnectionState.Closed)
                    mdbConn.Close();
            }
        }

        private static bool ExecuteCommand(OleDbCommand ocmd) {
            string connString = string.Format(ConnectionStringFormat, FileName);
            OleDbConnection mdbConn = new OleDbConnection(connString);
            bool result = true;
            try {
                mdbConn.Open();
                ocmd.Connection = mdbConn;
                ocmd.ExecuteNonQuery();

            }
            catch (Exception) {
                result = false;
            }
            finally {
                if (mdbConn.State == ConnectionState.Open) {
                    mdbConn.Close();
                }

            }
            return result;
        }

        private static bool CreateDB() {
            string connString = string.Format(ConnectionStringFormat, FileName);
            OleDbConnection mdbConn = new OleDbConnection(connString);

            try {
                mdbConn.Open();
                OleDbCommand mdbCommand = new OleDbCommand(CreateSTM, mdbConn);
                mdbCommand.ExecuteNonQuery();
            }
            catch (Exception) {
                return false;
            }
            finally {
                if (mdbConn.State == ConnectionState.Open) {
                    mdbConn.Close();
                }

            }
            return true;
        }
    }
}
