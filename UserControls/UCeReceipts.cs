using System.ComponentModel;
using System.Data;

namespace AccounterLite.UserControls
{
    public partial class UCeReceipts : UserControl
    {
        DataBase.SQLQuery sqlQ;
        Excel.ImportData importData;
        Excel.ExportSalePrices exportSaleData;
        Forms.ReceiptView receiptView;
        void intCmp() { if (sqlQ == null) sqlQ = new(); }
        public void filldgv()
        {
            dataGridView1.DataSource = sqlQ.getMData("EReceipts");
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }
        public UCeReceipts()
        {
            InitializeComponent();
            intCmp();
            filldgv();
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            importData = new("ERECEIPTS");
            importData.FormClosed += new FormClosedEventHandler(Form_Closed);
            importData.Show();
        }

        void Form_Closed(object sender, FormClosedEventArgs e)
        {
            filldgv();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string RCode = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim();

            receiptView = new Forms.ReceiptView(RCode, "Export");
            receiptView.Show();

        }

        private void btn_ExportSalePrices_Click(object sender, EventArgs e)
        {
            exportSaleData = new Excel.ExportSalePrices("Export");

            Cursor.Current = Cursors.WaitCursor;

            exportSaleData.Show();

            Cursor.Current = Cursors.Default;
        }

        private void txtB_Search_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
            string.Format("Code LIKE '{0}%' OR Code LIKE '% {0}%'", txtB_Search.Text);
        }
    }
}
