namespace AccounterLite.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Rawmats = new System.Windows.Forms.Button();
            this.btn_Costs = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_Home = new System.Windows.Forms.Button();
            this.btn_dReceipts = new System.Windows.Forms.Button();
            this.btn_eReceipts = new System.Windows.Forms.Button();
            this.pnl_Container = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Rawmats
            // 
            this.btn_Rawmats.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_Rawmats.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_Rawmats.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btn_Rawmats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Rawmats.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_Rawmats.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Rawmats.Location = new System.Drawing.Point(3, 71);
            this.btn_Rawmats.Name = "btn_Rawmats";
            this.btn_Rawmats.Size = new System.Drawing.Size(239, 60);
            this.btn_Rawmats.TabIndex = 0;
            this.btn_Rawmats.Text = "RAW MATERIALS";
            this.btn_Rawmats.UseVisualStyleBackColor = false;
            this.btn_Rawmats.Click += new System.EventHandler(this.btn_Rawmats_Click);
            // 
            // btn_Costs
            // 
            this.btn_Costs.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_Costs.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_Costs.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btn_Costs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Costs.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_Costs.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Costs.Location = new System.Drawing.Point(3, 137);
            this.btn_Costs.Name = "btn_Costs";
            this.btn_Costs.Size = new System.Drawing.Size(239, 60);
            this.btn_Costs.TabIndex = 0;
            this.btn_Costs.Text = "COSTS";
            this.btn_Costs.UseVisualStyleBackColor = false;
            this.btn_Costs.Click += new System.EventHandler(this.btn_Costs_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnl_Container, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1264, 606);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.btn_Home);
            this.panel1.Controls.Add(this.btn_Rawmats);
            this.panel1.Controls.Add(this.btn_dReceipts);
            this.panel1.Controls.Add(this.btn_eReceipts);
            this.panel1.Controls.Add(this.btn_Costs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 600);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 342);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(233, 47);
            this.dataGridView1.TabIndex = 0;
            // 
            // btn_Home
            // 
            this.btn_Home.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_Home.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_Home.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btn_Home.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Home.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_Home.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Home.Location = new System.Drawing.Point(3, 5);
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.Size = new System.Drawing.Size(239, 60);
            this.btn_Home.TabIndex = 0;
            this.btn_Home.Text = "HOME";
            this.btn_Home.UseVisualStyleBackColor = false;
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // btn_dReceipts
            // 
            this.btn_dReceipts.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_dReceipts.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_dReceipts.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btn_dReceipts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_dReceipts.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_dReceipts.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_dReceipts.Location = new System.Drawing.Point(3, 270);
            this.btn_dReceipts.Name = "btn_dReceipts";
            this.btn_dReceipts.Size = new System.Drawing.Size(239, 60);
            this.btn_dReceipts.TabIndex = 0;
            this.btn_dReceipts.Text = "RÇT_DOMESTIC";
            this.btn_dReceipts.UseVisualStyleBackColor = false;
            this.btn_dReceipts.Click += new System.EventHandler(this.btn_dReceipts_Click);
            // 
            // btn_eReceipts
            // 
            this.btn_eReceipts.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_eReceipts.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_eReceipts.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.btn_eReceipts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_eReceipts.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_eReceipts.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_eReceipts.Location = new System.Drawing.Point(3, 203);
            this.btn_eReceipts.Name = "btn_eReceipts";
            this.btn_eReceipts.Size = new System.Drawing.Size(239, 60);
            this.btn_eReceipts.TabIndex = 0;
            this.btn_eReceipts.Text = "RÇT_EXPORT";
            this.btn_eReceipts.UseVisualStyleBackColor = false;
            this.btn_eReceipts.Click += new System.EventHandler(this.btn_eReceipts_Click);
            // 
            // pnl_Container
            // 
            this.pnl_Container.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnl_Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Container.Location = new System.Drawing.Point(254, 3);
            this.pnl_Container.Name = "pnl_Container";
            this.pnl_Container.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.pnl_Container.Size = new System.Drawing.Size(1007, 600);
            this.pnl_Container.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 606);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(1280, 645);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccounterLite";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btn_Rawmats;
        private Button btn_Costs;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Panel pnl_Container;
        private Button btn_eReceipts;
        private Button btn_Home;
        private DataGridView dataGridView1;
        private Button btn_dReceipts;
    }
}