using System.Data;

namespace AccounterLite.UserControls
{
    public partial class UCRawmats : UserControl
    {
        DataBase.SQLQuery sqlQ;
        Excel.ImportData importData;

        void intCmp() { if (sqlQ == null) sqlQ = new(); }
        public void filldgv() { dataGridView1.DataSource = sqlQ.getMData("Rawmats"); }
        public UCRawmats()
        {
            InitializeComponent();
            intCmp();
            filldgv();
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            importData = new("RAWMATS");
            importData.FormClosed += new FormClosedEventHandler(Form_Closed);
            importData.Show();
        }

        void Form_Closed(object sender, FormClosedEventArgs e)
        {
            filldgv();
        }

        private void txtB_Search_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
                string.Format("Code LIKE '{0}%' OR Code LIKE '% {0}%'", txtB_Search.Text);
        }
    }
}
