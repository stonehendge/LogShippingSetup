using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace LogShippingSetup
{
    public partial class Form1 : Form
    {
        //private string message;

        private BackgroundWorker m_oWorker;
        private BackgroundWorker m_oWorker_restore;

        //private readonly SynchronizationContext synchronizationContext;

        private SqlConnectionStringBuilder builder;
        public static List<LSDB> selected_databases;

        public static string msg_ls_primary_server = "";

        public Form1()
        {
            InitializeComponent();
            lb_pri_db.DrawMode = DrawMode.OwnerDrawFixed;
            lb_sec_db.DrawMode = DrawMode.OwnerDrawFixed;

            //synchronizationContext = SynchronizationContext.Current;

            ColoredItem itp = new ColoredItem
            {
                Color = Color.Gray,
                Text = "---primary servers----"
            };
            ColoredItem its = new ColoredItem
            {
                Color = Color.Gray,
                Text = "---secondary servers----"
            };
            lb_pri_db.Items.Add(itp);
            lb_sec_db.Items.Add(its);

            selected_databases = new List<LSDB>();

            //Thread workingThread = new Thread(new ParameterizedThreadStart(update_ui))
            //{ IsBackground = true };

            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            m_oWorker.ProgressChanged += new ProgressChangedEventHandler
                   (m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                   (m_oWorker_RunWorkerCompleted);
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;

            m_oWorker_restore = new BackgroundWorker();
            m_oWorker_restore.DoWork += new DoWorkEventHandler(m_oWorker_restore_DoWork);
            m_oWorker_restore.ProgressChanged += new ProgressChangedEventHandler
                   (m_oWorker_restore_ProgressChanged);
            m_oWorker_restore.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                   (m_oWorker_restore_RunWorkerCompleted);
            m_oWorker_restore.WorkerReportsProgress = true;
            m_oWorker_restore.WorkerSupportsCancellation = true;

            //new Thread(() => update_ui("")) { IsBackground = true }.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected_databases.Clear();
            lb_pri_db.Items.Clear();
            lb_sec_db.Items.Clear();

            test_connection(tb_primary.Text);
            test_connection(tb_secondary.Text);
        }

        //public void Subscriber(SqlCommandWithProgress pub)
        //{
        //    //id = ID;
        //    // Subscribe to the event using C# 2.0 syntax
        //    pub.RaiseCustomEvent += HandleCustomEvent;
        //}

        //void HandleCustomEvent(object sender, CustomEventArgs e)
        //{
        //    lbl_output.Text = e.Message;
        //   //Console.WriteLine(id + " received this message: {0}", e.Message);
        //}
        //public void update_ui(string msg)
        //{
        //    //while(true)
        //    //{
        //    //    Thread.Sleep(4000);

        //        try
        //        {
        //        SqlCommandWithProgress pub = new SqlCommandWithProgress();
        //        this.Subscriber(pub);
        //           // Subscriber sub1 = new Subscriber("sub1", pub);

        //        synchronizationContext.Post(new SendOrPostCallback(o =>
        //            {
        //                lbl_output.Text =  (string)o;
        //            }), msg);
        //            //lbl_output.Text = msg_ls_primary_server.ToString();
        //        }
        //        catch(Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }

        //    //}

        //}

        public void test_connection(String srv_name)
        {
            // Build connection string
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = srv_name.Trim(); // tb_primary.Text.Trim();   // update me
            builder.IntegratedSecurity = true;
            //builder.UserID = "sa";              // update me
            //builder.Password = "your_password";      // update me
            builder.InitialCatalog = "master";
            // Connect to SQL
            Console.Write("Connecting to SQL Server ... ");

            //Get sql command text

            string commandText;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            using (Stream s = thisAssembly.GetManifestResourceStream(
                  "LogShippingSetup.SCRIPTS.TestConnection.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    commandText = sr.ReadToEnd();
                }
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("open connection.");

                    // Create a sample database
                    Console.Write("Testing connection ... ");
                    String sql = commandText; // "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (srv_name == tb_primary.Text)
                                {
                                    //DataRow dr = lb_pri_db.Rows[i];
                                    //ListViewItem listitem = new ListViewItem(dr["desc"].ToString());
                                    //listitem.SubItems.Add(dr["desc"].ToString());
                                    //listitem.SubItems.Add(dr["enchimento"].ToString());
                                    //lb_pri_db.Items.AddRange(new object[] {
                                    //reader.GetString(0)+","+reader.GetString(1)});
                                    ColoredItem coloredItem;

                                    if (reader.GetString(1) == "LSP" || reader.GetString(1) == "LSS")
                                    {
                                        coloredItem = new ColoredItem { Color = Color.Green, Text = reader.GetString(0) };
                                    }
                                    else
                                    {
                                        coloredItem = new ColoredItem { Color = Color.Black, Text = reader.GetString(0) };
                                    }

                                    lb_pri_db.Items.Add(coloredItem);
                                }
                                if (srv_name == tb_secondary.Text)
                                {
                                    ColoredItem coloredItem;

                                    if (reader.GetString(1) == "LSP" || reader.GetString(1) == "LSS")
                                    {
                                        coloredItem = new ColoredItem { Color = Color.Green, Text = reader.GetString(0) };
                                    }
                                    else
                                    {
                                        coloredItem = new ColoredItem { Color = Color.Black, Text = reader.GetString(0) };
                                    }

                                    lb_sec_db.Items.Add(coloredItem);
                                    //lb_sec_db.Items.Add(reader.GetString(0) + " " + reader.GetString(1));
                                }
                                //Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }

                        //MessageBox.Show("Connection successful");
                        //command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.ToString());
            }
        }

        /*run creation log shipping job objects on primary server*/

        public void create_ls_on_primary(String srv_name, string pri_server, string sec_server, string databases_str, string freq_subday_interval_template, string backup_retention_period)
        {
            // Build connection string
            builder = new SqlConnectionStringBuilder
            {
                DataSource = srv_name.Trim(), // tb_primary.Text.Trim();   // update me
                IntegratedSecurity = true,
                //builder.UserID = "sa";              // update me
                //builder.Password = "your_password";      // update me
                InitialCatalog = "master"
            };

            string commandText;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            using (Stream s = thisAssembly.GetManifestResourceStream(
                  "LogShippingSetup.SCRIPTS.RunPrimary.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    commandText = sr.ReadToEnd();
                }
            }

            string cmd_new = commandText.Replace("primary_server_template", pri_server).Replace("'freq_subday_interval_template'", freq_subday_interval_template).Replace("secondary_server_template", sec_server).Replace("('database_ls_template')", databases_str).Replace("'backup_retention_period_template'", backup_retention_period);

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("open connection.");

                    // Create a sample database
                    Console.Write("Testing connection ... ");
                    String sql = cmd_new; // "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.ToString());
            }
        }

        /*run creation log shipping job objects on secondary server*/

        public void create_ls_on_secondary(String srv_name, string pri_server, string sec_server, string file_retention_period_template, string freq_subday_interval_template, string databases_str)
        {
            // Build connection string
            builder = new SqlConnectionStringBuilder
            {
                DataSource = srv_name.Trim(), // tb_primary.Text.Trim();   // update me
                IntegratedSecurity = true,
                //builder.UserID = "sa";              // update me
                //builder.Password = "your_password";      // update me
                InitialCatalog = "master"
            };

            string commandText;
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            using (Stream s = thisAssembly.GetManifestResourceStream(
                  "LogShippingSetup.SCRIPTS.RunSecondary.sql"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    commandText = sr.ReadToEnd();
                }
            }

            string cmd_new = commandText.Replace("primary_server_template", pri_server).Replace("secondary_server_template", sec_server).Replace("'file_retention_period_template'", file_retention_period_template).Replace("'freq_subday_interval_template'", freq_subday_interval_template).Replace("('database_ls_template')", databases_str);

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("open connection.");

                    // Create a sample database
                    Console.Write("Testing connection ... ");
                    String sql = cmd_new; // "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string list_selected_db_pri = "";
            //list_selected_db_pri += lb_pri_db.SelectedItem.ToString();
            string command_sql = "";

            //string command_backup_init = "";

            //string command_restore_init = "";

            foreach (ColoredItem item in lb_pri_db.SelectedItems)
            {
                //    for (int iter=0; iter < lb_pri_db.SelectedItems.Count; iter++ )
                //{
                LSDB lsdb = new LSDB
                {
                    database_name = item.Text.ToString(),
                    src_share_path = tb_src_share.Text,
                    dest_share_path = tb_dest_share.Text,
                    primary_server = tb_primary.Text.Trim(),
                    secondary_server = tb_secondary.Text.Trim()
                };

                selected_databases.Add(lsdb);

                list_selected_db_pri += item.Text.ToString() + "\r\n";

                if (lb_pri_db.SelectedItems.IndexOf(item) == lb_pri_db.SelectedItems.Count - 1)
                {
                    command_sql += "('" + item.Text.ToString() + "','" + tb_loc_fld.Text + "','" + tb_src_share.Text + "','" + tb_dest_share.Text + "'," + tb_time_offset.Text + ")";
                    //command_backup_init += "BACKUP DATABASE ["+ item.Text.ToString() + "] TO DISK = ''" + tb_src_share.Text+"\\FULL_INIT_"+ item.Text.ToString()+".bak'' WITH CHECKSUM, FORMAT, INIT, STATS = 10; ";
                    //command_restore_init += "RESTORE DATABASE [" +item.Text.ToString() + "] FROM DISK = ''" + tb_src_share.Text + "\\FULL_INIT_" + item.Text.ToString() + ".bak''  WITH CHECKSUM, MOVE ["+ item.Text.ToString() + "] TO ''"+tb_mdf_path.Text+"\\"+ item.Text.ToString() + "_Data.mdf'', MOVE ''"+item.Text.ToString() +"_Log'' TO ''"+tb_ldf_path.Text+"\\" + item.Text.ToString() + "_Log.ldf'', NORECOVERY, REPLACE, STATS = 10; ";
                }
                else
                {
                    command_sql += "('" + item.Text.ToString() + "','" + tb_loc_fld.Text + "','" + tb_src_share.Text + "','" + tb_dest_share.Text + "'," + tb_time_offset.Text + "),";
                    //command_backup_init += "BACKUP DATABASE [" + item.Text.ToString() + "] TO DISK = ''" + tb_src_share.Text + "\\FULL_INIT_" + item.Text.ToString() + ".bak'' WITH CHECKSUM, FORMAT, INIT, STATS = 10; ";
                    //command_restore_init += "RESTORE DATABASE [" + item.Text.ToString() + "] FROM DISK = ''" + tb_src_share.Text + "\\FULL_INIT_" + item.Text.ToString() + ".bak''  WITH CHECKSUM, MOVE [" + item.Text.ToString() + "] TO ''" + tb_mdf_path.Text + "\\" + item.Text.ToString() + "_Data.mdf'', MOVE ''" + item.Text.ToString() + "_Log'' TO ''" + tb_ldf_path.Text + "\\" + item.Text.ToString() + "_Log.ldf'', NORECOVERY, REPLACE, STATS = 10; ";
                }
                //}
            }

            if (list_selected_db_pri != "")
            {
                DialogResult dialogResult = MessageBox.Show("Для следующих баз будет настроен LogShipping\r\n Будут созданы задания в службе SQL Agent \r\n" + list_selected_db_pri, "Alert!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    create_ls_on_primary(tb_primary.Text.Trim(), tb_primary.Text.Trim(), tb_secondary.Text.Trim(), command_sql, tb_freq_subday.Text.Trim(), tb_file_ret_pri.Text);

                    create_ls_on_secondary(tb_secondary.Text.Trim(), tb_primary.Text.Trim(), tb_secondary.Text.Trim(), tb_file_ret.Text.Trim(), tb_freq_subday.Text.Trim(), command_sql);
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одну базу для создания задния Log Shipping");
            }
            //var lst = lb_pri_db.SelectedItems.Cast<DataRowView>();
            //foreach (var item in lst)
            //{
            //    MessageBox.Show(item.Row[0].ToString());// Or Row[1]...
            //}

            //MessageBox.Show(list_selected_db_pri);
        }

        private void lb_pri_db_DrawItem(object sender, DrawItemEventArgs e)
        {
            var isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            Color foreColor;
            //if (!this.ColorsByIndex.TryGetValue(e.Index, out foreColor))
            //{
            foreColor = isItemSelected ? SystemColors.HighlightText : this.lb_pri_db.ForeColor;
            //}

            var backColor = isItemSelected ? SystemColors.Highlight : this.lb_pri_db.BackColor;
            using (var backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            var item = this.lb_pri_db.Items[e.Index] as ColoredItem;
            var itemText = this.lb_pri_db.GetItemText(item.Text);
            const TextFormatFlags formatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, itemText, e.Font, e.Bounds, item.Color, formatFlags);
            ////var item = lb_pri_db.Items[e.Index] as ColoredItem;

            ////if (item != null)
            ////{
            ////    e.Graphics.DrawString(
            ////        item.Text,
            ////        e.Font,
            ////        new SolidBrush(item.Color),
            ////        e.Bounds);
            ////}

            //int index = e.Index;
            //Graphics g = e.Graphics;

            //foreach (int selectedIndex in this.lb_pri_db.SelectedIndices)
            //{
            //    if (index == selectedIndex)
            //    {
            //        // Draw the new background colour
            //        e.DrawBackground();
            //        g.FillRectangle(new SolidBrush(Color.Blue), e.Bounds);
            //    }
            //}

            //// Get the item details
            //Font font = lb_pri_db.Font;
            //Color colour = lb_pri_db.ForeColor;
            //string text = lb_pri_db.Items[index].ToString();

            //// Print the text
            //g.DrawString(text, font, new SolidBrush(Color.Black), (float)e.Bounds.X, (float)e.Bounds.Y);
            //e.DrawFocusRectangle();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void lb_sec_db_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            var isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            Color foreColor;
            //if (!this.ColorsByIndex.TryGetValue(e.Index, out foreColor))
            //{
            foreColor = isItemSelected ? SystemColors.HighlightText : this.lb_sec_db.ForeColor;
            //}

            var backColor = isItemSelected ? SystemColors.Highlight : this.lb_sec_db.BackColor;
            using (var backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            var item = this.lb_sec_db.Items[e.Index] as ColoredItem;
            var itemText = this.lb_sec_db.GetItemText(item.Text);
            const TextFormatFlags formatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, itemText, e.Font, e.Bounds, item.Color, formatFlags);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            selected_databases.Clear();

            foreach (ColoredItem item in lb_pri_db.SelectedItems)
            {
                //    for (int iter=0; iter < lb_pri_db.SelectedItems.Count; iter++ )
                //{
                LSDB lsdb = new LSDB
                {
                    database_name = item.Text.ToString(),
                    src_share_path = tb_src_share.Text,
                    dest_share_path = tb_dest_share.Text,
                    primary_server = tb_primary.Text.Trim(),
                    secondary_server = tb_secondary.Text.Trim()
                };

                selected_databases.Add(lsdb);
            }

            ////LogShipping ls = new LogShipping();

            //////string output =
            ////await ls.backup_init_primary(selected_databases, tb_primary.Text.Trim());

            button3.Enabled = false;

            m_oWorker.RunWorkerAsync();

            //if (output != "")
            //{
            //    MessageBox.Show(output);
            //}
        }

        //=================================RESTORE===================================
        //===========================================================================

        private async void button4_Click(object sender, EventArgs e)
        {
            selected_databases.Clear();

            foreach (ColoredItem item in lb_pri_db.SelectedItems)
            {
                //    for (int iter=0; iter < lb_pri_db.SelectedItems.Count; iter++ )
                //{
                LSDB lsdb = new LSDB
                {
                    database_name = item.Text.ToString(),
                    src_share_path = tb_src_share.Text,
                    dest_share_path = tb_dest_share.Text,
                    primary_server = tb_primary.Text.Trim(),
                    secondary_server = tb_secondary.Text.Trim()
                };

                selected_databases.Add(lsdb);
            }

            button4.Enabled = false;

            m_oWorker_restore.RunWorkerAsync();
            //LogShipping ls = new LogShipping();

            ////string output =
            //await ls.restore_init_secondary(selected_databases,tb_primary.Text.Trim(), tb_secondary.Text.Trim(),tb_src_share.Text,tb_mdf_path.Text,tb_ldf_path.Text);

            //if (output != "")
            //{
            //    MessageBox.Show(output);
            //}
        }

        private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            //NOTE : Never play with the UI thread here...

            selected_databases.Clear();

            foreach (ColoredItem item in lb_pri_db.SelectedItems)
            {
                //    for (int iter=0; iter < lb_pri_db.SelectedItems.Count; iter++ )
                //{
                LSDB lsdb = new LSDB
                {
                    database_name = item.Text.ToString(),
                    src_share_path = tb_src_share.Text,
                    dest_share_path = tb_dest_share.Text,
                    primary_server = tb_primary.Text.Trim(),
                    secondary_server = tb_secondary.Text.Trim()
                };

                selected_databases.Add(lsdb);
            }

            m_oWorker.ReportProgress(0);

            LogShipping ls = new LogShipping();

            //string output =
            //await

            ls.backup_init_primary(selected_databases, tb_primary.Text.Trim());

            if (m_oWorker.CancellationPending)
            {
                // Set the e.Cancel flag so that the WorkerCompleted event
                // knows that the process was cancelled.
                e.Cancel = true;
                m_oWorker.ReportProgress(0);
                return;
            }
            //Report 100% completion on operation completed
            m_oWorker.ReportProgress(100);
        }

        private void m_oWorker_restore_DoWork(object sender, DoWorkEventArgs e)
        {
            selected_databases.Clear();

            foreach (ColoredItem item in lb_pri_db.SelectedItems)
            {
                //    for (int iter=0; iter < lb_pri_db.SelectedItems.Count; iter++ )
                //{
                LSDB lsdb = new LSDB
                {
                    database_name = item.Text.ToString(),
                    src_share_path = tb_src_share.Text,
                    dest_share_path = tb_dest_share.Text,
                    primary_server = tb_primary.Text.Trim(),
                    secondary_server = tb_secondary.Text.Trim()
                };

                selected_databases.Add(lsdb);
            }

            LogShipping ls = new LogShipping();

            //string output =
            // await ls.restore_init_secondary(selected_databases, tb_primary.Text.Trim(), tb_secondary.Text.Trim(), tb_src_share.Text, tb_mdf_path.Text, tb_ldf_path.Text);

            m_oWorker_restore.ReportProgress(0);

            ls.restore_init_secondary(selected_databases, tb_primary.Text.Trim(), tb_secondary.Text.Trim(), tb_src_share.Text, tb_mdf_path.Text, tb_ldf_path.Text);

            if (m_oWorker_restore.CancellationPending)
            {
                // Set the e.Cancel flag so that the WorkerCompleted event
                // knows that the process was cancelled.
                e.Cancel = true;
                m_oWorker_restore.ReportProgress(0);
                return;
            }
            //Report 100% completion on operation completed
            m_oWorker_restore.ReportProgress(100);
        }

        private void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lbl_output.Text = "Processing......" + e.ProgressPercentage.ToString() + "%";
        }

        private void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.
            if (e.Cancelled)
            {
                lbl_output.Text = "Task Cancelled.";
            }

            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                lbl_output.Text = "Error while performing background operation.";
            }
            else
            {
                // Everything completed normally.
                lbl_output.Text = "Task Completed...";
            }

            //Change the status of the buttons on the UI accordingly
            button3.Enabled = true;
        }

        private void m_oWorker_restore_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lbl_output.Text = "Processing......" + e.ProgressPercentage.ToString() + "%";
        }

        private void m_oWorker_restore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.
            if (e.Cancelled)
            {
                lbl_output.Text = "Task Cancelled.";
            }

            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                lbl_output.Text = "Error while performing background operation.";
            }
            else
            {
                // Everything completed normally.
                lbl_output.Text = "Restore Task Completed...";
            }

            //Change the status of the buttons on the UI accordingly
            button4.Enabled = true;
        }
    }

    public class ColoredItem
    {
        public string Text;
        public Color Color;
    };

    //class Subscriber
    //{
    //    //private string id;
    //    public Subscriber(SqlCommandWithProgress pub)
    //    {
    //        // Subscribe to the event using C# 2.0 syntax
    //        pub.RaiseCustomEvent += HandleCustomEvent;
    //    }

    //    // Define what actions to take when the event is raised.
    //    void HandleCustomEvent(object sender, CustomEventArgs e)
    //    {
    //        lbl_output.Text = e.Message;
    //        //Console.WriteLine(id + " received this message: {0}", e.Message);
    //    }
    //}
}