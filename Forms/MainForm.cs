namespace AccounterLite.Forms
{
    public partial class MainForm : Form
    {
        UserControls.UCMain uCMain;
        UserControls.UCCosts uCCosts;
        UserControls.UCRawmats uCRawmats;
        UserControls.UCeReceipts uCeReceipts;
        UserControls.UCdReceipts uCdReceipts;

        string CurrentUC = "Main";
        DataBase.SQLQuery sqlQ;
        Process.XRates xR;

        public MainForm()
        {
            initcs();
            InitializeComponent();
        }


        void fillxR()
        {
            dataGridView1.DataSource = sqlQ.getMData("XRates");
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            createUserControls();
            fillxR();
        }

        void initcs()
        {


            if (xR == null)
                xR = new();

            xR.Update();

            if (sqlQ == null)
                sqlQ = new();


        }

        void createUserControls()
        {
            if (uCMain == null) uCMain = new();
            if (uCCosts == null) uCCosts = new();
            if (uCRawmats == null) uCRawmats = new();
            if (uCeReceipts == null) uCeReceipts = new();
            if (uCdReceipts == null) uCdReceipts = new();

            pnl_Container.Controls.Add(uCMain);
            pnl_Container.Controls.Add(uCCosts);
            pnl_Container.Controls.Add(uCRawmats);
            pnl_Container.Controls.Add(uCeReceipts);
            pnl_Container.Controls.Add(uCdReceipts);

            uCMain.Dock = DockStyle.Fill;
            uCCosts.Dock = DockStyle.Fill;
            uCRawmats.Dock = DockStyle.Fill;
            uCeReceipts.Dock = DockStyle.Fill;
            uCdReceipts.Dock = DockStyle.Fill;

            uCMain.Visible = true;
            uCCosts.Visible = false;
            uCRawmats.Visible = false;
            uCeReceipts.Visible = false;
            uCdReceipts.Visible = false;

        }

        void switchControlsF(string control, bool type)
        {
            switch (control)
            {
                case "Main": uCMain.Visible = type; break;
                case "Costs": uCCosts.Visible = type; break;
                case "Rawmats": uCRawmats.Visible = type; break;
                case "eReceipts": uCeReceipts.Visible = type; break;
                case "dReceipts": uCdReceipts.Visible = type; break;

            }
        }

        void switchControls(string newC, string currentC)
        {
            if (newC != currentC)
            {
                switchControlsF(currentC, false);
                switchControlsF(newC, true);
                CurrentUC = newC;
            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            switchControls("Main", CurrentUC);
        }
        private void btn_Rawmats_Click(object sender, EventArgs e)
        {
            switchControls("Rawmats", CurrentUC);
        }
        private void btn_Costs_Click(object sender, EventArgs e)
        {
            switchControls("Costs", CurrentUC);
        }
        private void btn_eReceipts_Click(object sender, EventArgs e)
        {
            switchControls("eReceipts", CurrentUC);
        }

        private void btn_dReceipts_Click(object sender, EventArgs e)
        {
            switchControls("dReceipts", CurrentUC);

        }

    }
}
