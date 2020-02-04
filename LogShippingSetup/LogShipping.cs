using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LogShippingSetup
{
    internal class LogShipping
    {
        public SqlConnectionStringBuilder builder;

        public void backup_init_primary(List<LSDB> list_lsdb, string primary_server)
        {
            string result = "";
            string command_backup_init = "";

            //var self = (BackgroundWorker)sender;

            builder = new SqlConnectionStringBuilder
            {
                DataSource = primary_server.Trim(), // tb_primary.Text.Trim();   // update me
                IntegratedSecurity = true,
                //builder.UserID = "sa";              // update me
                //builder.Password = "your_password";      // update me
                InitialCatalog = "master"
            };

            if (list_lsdb.Count == 0)
            {
                result = "Empty list. Get list of databases and select items in list, please.";
                //return result;
            }

            foreach (LSDB item in list_lsdb)
            {
                command_backup_init += "BACKUP DATABASE [" + item.database_name + "] TO DISK = '" + item.src_share_path + "\\FULL_INIT_" + item.database_name + ".bak' WITH CHECKSUM, FORMAT, INIT, STATS = 10; ";
                //command_backup_init += " RaisError('Backup completed for database = " + item.database_name + "', 1, 1) With NoWait; ";
            }

            try
            {

                SqlCommandWithProgress run_cmd = new SqlCommandWithProgress();
                ////run_cmd.ExecuteNonQueryWithEvents(builder.ConnectionString, command_backup_init, s => Console.WriteLine(s));

                run_cmd.ExecuteNonQueryWait(builder.ConnectionString, command_backup_init, s => Console.WriteLine(s));

            }
            catch (SqlException e)
            {
                //MessageBox.Show(e.Message);
                result = e.ToString();
                Console.WriteLine(e.ToString());
            }

            //return result;
        }

        public string get_restore_cmd_from_primary(List<LSDB> list_lsdb, string pri_server, string backup_share, string mdf_path, string ldf_path)
        {
            string result = "";
            string command_restore_init = "";

            builder = new SqlConnectionStringBuilder
            {
                DataSource = pri_server, // tb_primary.Text.Trim();   // update me
                IntegratedSecurity = true,
                //builder.UserID = "sa";              // update me
                //builder.Password = "your_password";      // update me
                InitialCatalog = "master"
            };

            string commandText;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            using (Stream s = thisAssembly.GetManifestResourceStream(
                  "LogShippingSetup.SCRIPTS.RestoreInitSecondary.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    commandText = sr.ReadToEnd();
                }
            }

            //command_restore_init += "(";
            foreach (LSDB item in list_lsdb)
            {
                // if (lb_pri_db.SelectedItems.IndexOf(item) == lb_pri_db.SelectedItems.Count - 1)
                if (list_lsdb.IndexOf(item) == list_lsdb.Count - 1)
                {
                    command_restore_init += "('" + item.database_name + "')";
                }
                else
                {
                    command_restore_init += "('" + item.database_name + "'),";
                }
            }
            //command_restore_init += ")";

            string cmd_new = commandText.Replace("backup_share_template", backup_share).Replace("dbfile_mdf_template", mdf_path).Replace("logfile_ldf_template", ldf_path).Replace("('database_ls_template')", command_restore_init);

            try
            {
                //await SqlCommandWithProgress.ExecuteNonQuery(builder.ConnectionString, cmd_new, s => Console.WriteLine(s));

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("open connection.");

                    // Create a sample database

                    String sql = cmd_new;// command_restore_init;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result += reader.GetString(0);

                                //Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                 
                }
            }
            catch (SqlException e)
            {
                //MessageBox.Show(e.Message);
                result = e.ToString();
                Console.WriteLine(e.ToString());
            }

            return result;
        }

        public void restore_init_secondary(List<LSDB> list_lsdb, string pri_server, string sec_server, string backup_share, string mdf_path, string ldf_path)
        {
            string result = "";
            //string command_restore_init = "";

            string restore_command_secondary = get_restore_cmd_from_primary(list_lsdb, pri_server, backup_share, mdf_path, ldf_path);

            if (restore_command_secondary != "")
            {
                builder = new SqlConnectionStringBuilder
                {
                    DataSource = sec_server, // tb_primary.Text.Trim();   // update me
                    IntegratedSecurity = true,
                    //builder.UserID = "sa";              // update me
                    //builder.Password = "your_password";      // update me
                    InitialCatalog = "master"
                };

          
                try
                {
                    //await
                    SqlCommandWithProgress run_cmd = new SqlCommandWithProgress();
                    ////run_cmd.ExecuteNonQueryWithEvents(builder.ConnectionString, command_backup_init, s => Console.WriteLine(s));

                    run_cmd.ExecuteNonQueryWait(builder.ConnectionString, restore_command_secondary, s => Console.WriteLine(s));

                }
                catch (SqlException e)
                {
                    //MessageBox.Show(e.Message);
                    result = e.ToString();
                    Console.WriteLine(e.ToString());
                }
            }

            //return result;
        }

        //static async Task Run(String sql, String connection_string)
        //{
        //    //var constr = "server=localhost;database=tempdb;integrated security=true";
        //    //            var sql = @"
        //    //set nocount on;
        //    //select newid() d
        //    //into #foo
        //    //from sys.objects, sys.objects o2, sys.columns
        //    //order by newid();
        //    //select count(*) from #foo;
        //    //";

        //    using (var rdr = await SqlCommandWithProgress.ExecuteReader(connection_string, sql, s => Console.WriteLine(s)))
        //    {
        //        if (!rdr.IsClosed)
        //        {
        //            while (rdr.Read())
        //            {
        //                Console.WriteLine("Row read");
        //            }
        //        }
        //    }
        //    Console.WriteLine("Hit any key to exit.");
        //    Console.ReadKey();

        //}
    }

    public class SessionStats
    {
        public long Reads { get; set; }
        public long Writes { get; set; }
        public long CpuTime { get; set; }
        public long RowCount { get; set; }
        public long WaitTime { get; set; }
        public string LastWaitType { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Reads {Reads}, Writes {Writes}, CPU {CpuTime}, RowCount {RowCount}, WaitTime {WaitTime}, LastWaitType {LastWaitType}, Status {Status}";
        }
    }

    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string s)
        {
            message = s;
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

   
    public class SqlCommandWithProgress
    {
       

        public void ExecuteNonQueryWait(string ConnectionString, string Query, Action<SessionStats> OnProgress)
        {
            var mainCon = new SqlConnection(ConnectionString);

            //mainCon.ConnectionTimeout = 3600;
            mainCon.Open();
            //    monitorCon.Open();

            //var cmd = new SqlCommand("select @@spid session_id", mainCon);
            //var spid = Convert.ToInt32(cmd.ExecuteScalar());

            var cmd = new SqlCommand(Query, mainCon);
            cmd.CommandTimeout = 3000;

            //                var monitorQuery = @"
            //select s.reads, s.writes, r.cpu_time, s.row_count, r.wait_time, r.last_wait_type, r.status
            //from sys.dm_exec_requests r
            //join sys.dm_exec_sessions s
            //  on r.session_id = s.session_id
            //where r.session_id = @session_id";

            //                var monitorCmd = new SqlCommand(monitorQuery, monitorCon);
            //                monitorCmd.Parameters.Add(new SqlParameter("@session_id", spid));

            var queryTask = cmd.ExecuteNonQueryAsync();// CommandBehavior.CloseConnection);

            /*   var cols = new { reads = 0, writes = 1, cpu_time = 2, row_count = 3, wait_time = 4, last_wait_type = 5, status = 6 };*/
            while (!queryTask.IsCompleted)
            {
                //var firstTask = await Task.WhenAny(queryTask, Task.Delay(1000));
                //if (firstTask == queryTask)
                //{
                //    break;
                //}
                //using (var rdr = await monitorCmd.ExecuteReaderAsync())
                //{
                //    await rdr.ReadAsync();
                //    var result = new SessionStats()
                //    {
                //        Reads = Convert.ToInt64(rdr[cols.reads]),
                //        Writes = Convert.ToInt64(rdr[cols.writes]),
                //        RowCount = Convert.ToInt64(rdr[cols.row_count]),
                //        CpuTime = Convert.ToInt64(rdr[cols.cpu_time]),
                //        WaitTime = Convert.ToInt64(rdr[cols.wait_time]),
                //        LastWaitType = Convert.ToString(rdr[cols.last_wait_type]),
                //        Status = Convert.ToString(rdr[cols.status]),
                //    };

                //    OnRaiseCustomEvent(new CustomEventArgs("Did something"));

                //    Form1.msg_ls_primary_server = result.ToString();

                //    OnProgress(result);

                //}
            }
            //return queryTask.Result;

            //}

            //OnRaiseCustomEvent(new CustomEventArgs("Did something"));
        }

        public async void ExecuteNonQueryWithEvents(string ConnectionString, string Query, Action<SessionStats> OnProgress)
        {
            var mainCon = new SqlConnection(ConnectionString);
            using (var monitorCon = new SqlConnection(ConnectionString))
            {
                //mainCon.ConnectionTimeout = 3600;
                mainCon.Open();
                monitorCon.Open();

                var cmd = new SqlCommand("select @@spid session_id", mainCon);
                var spid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand(Query, mainCon);
                cmd.CommandTimeout = 3000;

                var monitorQuery = @"
select s.reads, s.writes, r.cpu_time, s.row_count, r.wait_time, r.last_wait_type, r.status
from sys.dm_exec_requests r
join sys.dm_exec_sessions s
  on r.session_id = s.session_id
where r.session_id = @session_id";

                var monitorCmd = new SqlCommand(monitorQuery, monitorCon);
                monitorCmd.Parameters.Add(new SqlParameter("@session_id", spid));

                var queryTask = cmd.ExecuteNonQueryAsync();// CommandBehavior.CloseConnection);

                var cols = new { reads = 0, writes = 1, cpu_time = 2, row_count = 3, wait_time = 4, last_wait_type = 5, status = 6 };
                while (!queryTask.IsCompleted)
                {
                    var firstTask = await Task.WhenAny(queryTask, Task.Delay(1000));
                    if (firstTask == queryTask)
                    {
                        break;
                    }
                    using (var rdr = await monitorCmd.ExecuteReaderAsync())
                    {
                        await rdr.ReadAsync();
                        var result = new SessionStats()
                        {
                            Reads = Convert.ToInt64(rdr[cols.reads]),
                            Writes = Convert.ToInt64(rdr[cols.writes]),
                            RowCount = Convert.ToInt64(rdr[cols.row_count]),
                            CpuTime = Convert.ToInt64(rdr[cols.cpu_time]),
                            WaitTime = Convert.ToInt64(rdr[cols.wait_time]),
                            LastWaitType = Convert.ToString(rdr[cols.last_wait_type]),
                            Status = Convert.ToString(rdr[cols.status]),
                        };

                        //OnRaiseCustomEvent(new CustomEventArgs("Did something"));

                        Form1.msg_ls_primary_server = result.ToString();

                        OnProgress(result);
                    }
                }
                //return queryTask.Result;
            }

            //OnRaiseCustomEvent(new CustomEventArgs("Did something"));
        }

        public static async Task ExecuteNonQuery(string ConnectionString, string Query, Action<SessionStats> OnProgress, IProgress<string> progress = null)
        {
            var mainCon = new SqlConnection(ConnectionString);
            using (var monitorCon = new SqlConnection(ConnectionString))
            {
                //mainCon.ConnectionTimeout = 3600;
                mainCon.Open();
                monitorCon.Open();

                var cmd = new SqlCommand("select @@spid session_id", mainCon);
                var spid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand(Query, mainCon);
                cmd.CommandTimeout = 3000;

                var monitorQuery = @"
select s.reads, s.writes, r.cpu_time, s.row_count, r.wait_time, r.last_wait_type, r.status
from sys.dm_exec_requests r
join sys.dm_exec_sessions s
  on r.session_id = s.session_id
where r.session_id = @session_id";

                var monitorCmd = new SqlCommand(monitorQuery, monitorCon);
                monitorCmd.Parameters.Add(new SqlParameter("@session_id", spid));

                var queryTask = cmd.ExecuteNonQueryAsync();// CommandBehavior.CloseConnection);

                var cols = new { reads = 0, writes = 1, cpu_time = 2, row_count = 3, wait_time = 4, last_wait_type = 5, status = 6 };
                while (!queryTask.IsCompleted)
                {
                    var firstTask = await Task.WhenAny(queryTask, Task.Delay(1000));
                    if (firstTask == queryTask)
                    {
                        break;
                    }
                    using (var rdr = await monitorCmd.ExecuteReaderAsync())
                    {
                        await rdr.ReadAsync();
                        var result = new SessionStats()
                        {
                            Reads = Convert.ToInt64(rdr[cols.reads]),
                            Writes = Convert.ToInt64(rdr[cols.writes]),
                            RowCount = Convert.ToInt64(rdr[cols.row_count]),
                            CpuTime = Convert.ToInt64(rdr[cols.cpu_time]),
                            WaitTime = Convert.ToInt64(rdr[cols.wait_time]),
                            LastWaitType = Convert.ToString(rdr[cols.last_wait_type]),
                            Status = Convert.ToString(rdr[cols.status]),
                        };

                        Form1.msg_ls_primary_server = result.ToString();

                        OnProgress(result);
                    }
                }
                //return queryTask.Result;
            }

            //using (var rdr = await ExecuteReader(ConnectionString, Query, OnProgress))
            //{
            //    rdr.Dispose();
            //}
        }

        public static async Task<DataTable> ExecuteDataTable(string ConnectionString, string Query, Action<SessionStats> OnProgress)
        {
            using (var rdr = await ExecuteReader(ConnectionString, Query, OnProgress))
            {
                var dt = new DataTable();

                dt.Load(rdr);
                return dt;
            }
        }

        public static async Task<SqlDataReader> ExecuteReader(string ConnectionString, string Query, Action<SessionStats> OnProgress)
        {
            var mainCon = new SqlConnection(ConnectionString);
            using (var monitorCon = new SqlConnection(ConnectionString))
            {
                //mainCon.ConnectionTimeout = 3600;
                mainCon.Open();
                monitorCon.Open();

                var cmd = new SqlCommand("select @@spid session_id", mainCon);
                var spid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand(Query, mainCon);

                var monitorQuery = @"
select s.reads, s.writes, r.cpu_time, s.row_count, r.wait_time, r.last_wait_type, r.status
from sys.dm_exec_requests r
join sys.dm_exec_sessions s
  on r.session_id = s.session_id
where r.session_id = @session_id";

                var monitorCmd = new SqlCommand(monitorQuery, monitorCon);
                monitorCmd.Parameters.Add(new SqlParameter("@session_id", spid));

                var queryTask = cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                var cols = new { reads = 0, writes = 1, cpu_time = 2, row_count = 3, wait_time = 4, last_wait_type = 5, status = 6 };
                while (!queryTask.IsCompleted)
                {
                    var firstTask = await Task.WhenAny(queryTask, Task.Delay(1000));
                    if (firstTask == queryTask)
                    {
                        break;
                    }
                    using (var rdr = await monitorCmd.ExecuteReaderAsync())
                    {
                        await rdr.ReadAsync();
                        var result = new SessionStats()
                        {
                            Reads = Convert.ToInt64(rdr[cols.reads]),
                            Writes = Convert.ToInt64(rdr[cols.writes]),
                            RowCount = Convert.ToInt64(rdr[cols.row_count]),
                            CpuTime = Convert.ToInt64(rdr[cols.cpu_time]),
                            WaitTime = Convert.ToInt64(rdr[cols.wait_time]),
                            LastWaitType = Convert.ToString(rdr[cols.last_wait_type]),
                            Status = Convert.ToString(rdr[cols.status]),
                        };

                        Form1.msg_ls_primary_server = result.ToString();

                        OnProgress(result);
                    }
                }
                return queryTask.Result;
            }
        }
    }
}