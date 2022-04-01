using ClosedXML.Excel;
using MongoDB.Bson;
using System.Data;


namespace AccounterLite.Excel
{
    public partial class ExportSalePrices : Form
    {
        DataBase.SQLQuery sqlQ;
        string rtype;
        void intCmp() { if (sqlQ == null) sqlQ = new(); }

        BsonValue getUnitPrice(string code) { return sqlQ.selectMDataByCode("Raw_Materials", double.Parse(code))["Finalprice"]; }
        BsonValue getUnitCount(string code) { return sqlQ.selectMDataByCode("UnitCosts", code)["UnitCount"]; }

        double gd(object var) { return double.Parse(var.ToString()); }
        string sF(object var) { return String.Format("{0:0.00}", gd(var)); }
        DataTable prepareData()
        {
            BsonDocument Costs = sqlQ.selectMDataByType("Costs", rtype);

            double CostsTotal = gd(Costs["EndirektIscilik"]) + gd(Costs["GenelUretimGider"]) + gd(Costs["PazarlamaSatisGider"])
                              + gd(Costs["SatisIscilik"]) + gd(Costs["GenelYonetimGider"]) + gd(Costs["IdariIscilik"]);
            double Marj = gd(Costs["Marj"]);


            DataTable dt = new DataTable();
            dt.Columns.Add("Code");
            dt.Columns.Add("Name");
            dt.Columns.Add("TCOST");
            dt.Columns.Add("WCOST");
            dt.Columns.Add("UCOST");
            dt.Columns.Add("TPRICE");
            dt.Columns.Add("WPRICE");
            dt.Columns.Add("UPRICE");



            var receipts = sqlQ.getCollection("Receipts_" + rtype);

            foreach (var receipt in receipts)
            {
                BsonArray Ings = receipt["Ingredients"].AsBsonArray,
                          PackIngs = receipt["PackIngredients"].AsBsonArray;

                double ingtcost = 0, pingtcost = 0;

                double ingWeight = 0;
                long unitCount = 0;

                BsonDocument UnitCost = sqlQ.selectMDataByCode("UnitCosts", receipt["Code"].ToString());

                if (UnitCost["Unit"] == "AD")
                    unitCount = long.Parse(UnitCost["UnitCount"].ToString());

                foreach (BsonDocument item in Ings)
                {
                    ingWeight += gd(item["value"]);
                }

                foreach (BsonDocument item in Ings)
                {
                    item.Add("unitprice", getUnitPrice(item["code"].ToString()));
                    item.Add("cost", gd(item["unitprice"]) * gd(item["value"]));
                    item.Add("tcost", gd(item["cost"]) * 1.03);

                    ingtcost += gd(item["tcost"]);
                }

                foreach (BsonDocument item in PackIngs)
                {
                    item.Add("unitprice", getUnitPrice(item["code"].ToString()));
                    item.Add("cost", gd(item["unitprice"]) * gd(item["value"]));
                    item.Add("tcost", gd(item["cost"]) * 1.03);

                    pingtcost += gd(item["tcost"]);
                }

                double tcost = ingtcost + pingtcost;
                double kacost = (CostsTotal + gd(UnitCost["DirektIscilik"])) * ingWeight;
                tcost += kacost;

                if (UnitCost["Unit"] == "AD")
                {
                    dt.Rows.Add(receipt["Code"], receipt["Name"], sF(tcost), sF(tcost / ingWeight), sF(tcost / unitCount), sF(tcost * Marj), sF(tcost / ingWeight * Marj), sF(tcost / unitCount * Marj));
                }
                else
                {
                    dt.Rows.Add(receipt["Code"], receipt["Name"], sF(tcost), sF(tcost / ingWeight), "-", sF(tcost), sF(tcost / ingWeight), "-");

                }


            }

            return dt;
        }

        public ExportSalePrices(string type)
        {
            rtype = type;
            InitializeComponent();
            intCmp();

        }

        private void ExportSalePrices_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = prepareData();

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                column.Width = 65;
            }

            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btn_ExportToExcel_Click(object sender, EventArgs e)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Bayi Satış Fiyatları");

                worksheet.Cell("A1").Value = "Code";
                worksheet.Cell("B1").Value = "Name";
                worksheet.Cell("C1").Value = "TCOST";
                worksheet.Cell("D1").Value = "WCOST";
                worksheet.Cell("E1").Value = "UCOST";
                worksheet.Cell("F1").Value = "TPRICE";
                worksheet.Cell("G1").Value = "WPRICE";
                worksheet.Cell("H1").Value = "UPRICE";

                int excelIndex = 2;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    worksheet.Cell("A" + excelIndex).Value = row.Cells["Code"].Value;
                    worksheet.Cell("B" + excelIndex).Value = row.Cells["Name"].Value;
                    worksheet.Cell("C" + excelIndex).Value = row.Cells["TCOST"].Value;
                    worksheet.Cell("D" + excelIndex).Value = row.Cells["WCOST"].Value;
                    worksheet.Cell("E" + excelIndex).Value = row.Cells["UCOST"].Value;
                    worksheet.Cell("F" + excelIndex).Value = row.Cells["TPRICE"].Value;
                    worksheet.Cell("G" + excelIndex).Value = row.Cells["WPRICE"].Value;
                    worksheet.Cell("H" + excelIndex).Value = row.Cells["UPRICE"].Value;

                    excelIndex++;
                }

                worksheet.Columns().AdjustToContents();

                try
                {
                    workbook.SaveAs(getExportDir());
                    MessageBox.Show("Veriler Başarıyla Aktarıldı.");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("it is being used"))
                    {
                        MessageBox.Show("Lütfen önce kaydetmeye çalıştığınız dosyayı kapatın.");
                    }

                }
            }

        }

        string getExportDir()
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Bayi Satış Fiyatları";
            saveFileDialog1.FileName = "Bayi Satış Fiyatları";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "Excel Worksheets|*.xlsx";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }


            return "Bayi Satış Fiyatları.xlsx";

        }
    }


}
