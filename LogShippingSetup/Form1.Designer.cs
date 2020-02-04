namespace LogShippingSetup
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_primary = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_secondary = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lb_pri_db = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_sec_db = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_loc_fld = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_file_ret_pri = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_ldf_path = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_mdf_path = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_freq_subday = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_file_ret = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_time_offset = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_dest_share = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_src_share = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lbl_output = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_primary
            // 
            this.tb_primary.Location = new System.Drawing.Point(12, 17);
            this.tb_primary.Name = "tb_primary";
            this.tb_primary.Size = new System.Drawing.Size(235, 20);
            this.tb_primary.TabIndex = 0;
            this.tb_primary.Text = "sql-cluster2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "PrimaryServerInstance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SecondaryServerInstance";
            // 
            // tb_secondary
            // 
            this.tb_secondary.Location = new System.Drawing.Point(12, 43);
            this.tb_secondary.Name = "tb_secondary";
            this.tb_secondary.Size = new System.Drawing.Size(235, 20);
            this.tb_secondary.TabIndex = 2;
            this.tb_secondary.Text = "sql-d03n16\\sql_cluster2";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Location = new System.Drawing.Point(12, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "GetDatabasesList";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(123, 345);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "GenerateJobs";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lb_pri_db
            // 
            this.lb_pri_db.FormattingEnabled = true;
            this.lb_pri_db.Location = new System.Drawing.Point(6, 19);
            this.lb_pri_db.Name = "lb_pri_db";
            this.lb_pri_db.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lb_pri_db.Size = new System.Drawing.Size(230, 264);
            this.lb_pri_db.TabIndex = 6;
            this.lb_pri_db.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lb_pri_db_DrawItem);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lb_pri_db);
            this.groupBox1.Location = new System.Drawing.Point(416, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 322);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Primary server databases";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 291);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Green items with LS enabled";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lb_sec_db);
            this.groupBox2.Location = new System.Drawing.Point(664, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(242, 322);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Secondary server databases";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Green items with LS enabled";
            // 
            // lb_sec_db
            // 
            this.lb_sec_db.FormattingEnabled = true;
            this.lb_sec_db.Location = new System.Drawing.Point(6, 19);
            this.lb_sec_db.Name = "lb_sec_db";
            this.lb_sec_db.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lb_sec_db.Size = new System.Drawing.Size(230, 264);
            this.lb_sec_db.TabIndex = 6;
            this.lb_sec_db.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lb_sec_db_DrawItem_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Path .trn folder local";
            // 
            // tb_loc_fld
            // 
            this.tb_loc_fld.Location = new System.Drawing.Point(6, 19);
            this.tb_loc_fld.Name = "tb_loc_fld";
            this.tb_loc_fld.Size = new System.Drawing.Size(272, 20);
            this.tb_loc_fld.TabIndex = 10;
            this.tb_loc_fld.Text = "\\\\backup01\\sql$\\SQL-CLUSTER2\\USERS\\TRN";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_file_ret_pri);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.tb_ldf_path);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.tb_mdf_path);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.tb_freq_subday);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.tb_file_ret);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.tb_time_offset);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.tb_dest_share);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.tb_src_share);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tb_loc_fld);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(398, 265);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "LS Settings";
            // 
            // tb_file_ret_pri
            // 
            this.tb_file_ret_pri.Location = new System.Drawing.Point(6, 122);
            this.tb_file_ret_pri.Name = "tb_file_ret_pri";
            this.tb_file_ret_pri.Size = new System.Drawing.Size(272, 20);
            this.tb_file_ret_pri.TabIndex = 26;
            this.tb_file_ret_pri.Text = "2880";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(284, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "File retention pri";
            // 
            // tb_ldf_path
            // 
            this.tb_ldf_path.Location = new System.Drawing.Point(6, 227);
            this.tb_ldf_path.Name = "tb_ldf_path";
            this.tb_ldf_path.Size = new System.Drawing.Size(272, 20);
            this.tb_ldf_path.TabIndex = 24;
            this.tb_ldf_path.Text = "F:\\Logs\\SQL-Cluster2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(284, 230);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Restore path .ldf";
            // 
            // tb_mdf_path
            // 
            this.tb_mdf_path.Location = new System.Drawing.Point(6, 201);
            this.tb_mdf_path.Name = "tb_mdf_path";
            this.tb_mdf_path.Size = new System.Drawing.Size(272, 20);
            this.tb_mdf_path.TabIndex = 22;
            this.tb_mdf_path.Text = "G:\\DB\\SQL-Cluster2";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(284, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Restore path .mdf";
            // 
            // tb_freq_subday
            // 
            this.tb_freq_subday.Location = new System.Drawing.Point(6, 175);
            this.tb_freq_subday.Name = "tb_freq_subday";
            this.tb_freq_subday.Size = new System.Drawing.Size(272, 20);
            this.tb_freq_subday.TabIndex = 20;
            this.tb_freq_subday.Text = "20";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(284, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Freq subday interval";
            // 
            // tb_file_ret
            // 
            this.tb_file_ret.Location = new System.Drawing.Point(6, 149);
            this.tb_file_ret.Name = "tb_file_ret";
            this.tb_file_ret.Size = new System.Drawing.Size(272, 20);
            this.tb_file_ret.TabIndex = 18;
            this.tb_file_ret.Text = "180";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(284, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "File retention sec";
            // 
            // tb_time_offset
            // 
            this.tb_time_offset.Location = new System.Drawing.Point(6, 96);
            this.tb_time_offset.Name = "tb_time_offset";
            this.tb_time_offset.Size = new System.Drawing.Size(272, 20);
            this.tb_time_offset.TabIndex = 16;
            this.tb_time_offset.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(284, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Start time offset";
            // 
            // tb_dest_share
            // 
            this.tb_dest_share.Location = new System.Drawing.Point(6, 71);
            this.tb_dest_share.Name = "tb_dest_share";
            this.tb_dest_share.Size = new System.Drawing.Size(272, 20);
            this.tb_dest_share.TabIndex = 14;
            this.tb_dest_share.Text = "\\\\fs-logs\\logshipping$\\SQL-D03N16_SQL_CLUSTER2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(284, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Dest path .trn share";
            // 
            // tb_src_share
            // 
            this.tb_src_share.Location = new System.Drawing.Point(6, 45);
            this.tb_src_share.Name = "tb_src_share";
            this.tb_src_share.Size = new System.Drawing.Size(272, 20);
            this.tb_src_share.TabIndex = 12;
            this.tb_src_share.Text = "\\\\backup01\\sql$\\SQL-CLUSTER2\\USERS\\TRN";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(284, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Src path .trn share";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(234, 345);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "BackupInit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(345, 345);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "RestoreInit";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lbl_output
            // 
            this.lbl_output.Location = new System.Drawing.Point(12, 380);
            this.lbl_output.Name = "lbl_output";
            this.lbl_output.Size = new System.Drawing.Size(891, 27);
            this.lbl_output.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 413);
            this.Controls.Add(this.lbl_output);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_secondary);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_primary);
            this.Name = "Form1";
            this.Text = "LSSetup";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_primary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_secondary;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lb_pri_db;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lb_sec_db;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_loc_fld;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_src_share;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_dest_share;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_time_offset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_freq_subday;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_file_ret;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_ldf_path;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb_mdf_path;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lbl_output;
        private System.Windows.Forms.TextBox tb_file_ret_pri;
        private System.Windows.Forms.Label label13;
    }
}

