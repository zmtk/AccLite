using ExcelDataReader;
using System.Data;

namespace AccounterLite.Excel
{
    public partial class ImportData : Form
    {
        string type = "null";
        DataBase.SQLQuery sqlQ;
        void initCmp() { if (sqlQ == null) sqlQ = new(); }



        public class Ingredient
        {
            public string rCode { get; set; }
            public string Code { get; set; }
            public string Unit { get; set; }
            public double Value { get; set; }

        }


        public ImportData(string type)
        {
            this.type = type;
            initCmp();
            InitializeComponent();
        }

        bool sNull(object var) { return string.IsNullOrWhiteSpace(var.ToString()); } //check String if null or empty
        bool sDouble(object var) { return double.TryParse(var.ToString(), out _); } //check String if double

        bool rawmatsCheckTypes(object code, object name, object price, object curr)
        {
            if (sNull(code) || sNull(name) || sNull(price) || sNull(curr))
                return false;

            if (!long.TryParse(code.ToString(), out long Code))
                return false;

            if (!double.TryParse(price.ToString(), out _))
                return false;

            if (!(curr.ToString().Contains("EUR") || curr.ToString().Contains("TL") || curr.ToString().Contains("USD")))
                return false;

            return true;
        }

        bool unitcostsCheckTypes(object code, object name, object UnitCount, object Unit, object DirektIscilik)
        {

            if (sNull(code) || sNull(name) || sNull(UnitCount) || sNull(Unit) || sNull(DirektIscilik))
                return false;

            if (!double.TryParse(UnitCount.ToString(), out _) || !double.TryParse(DirektIscilik.ToString(), out _))
                return false;

            if (!(Unit.ToString().Equals("KG") || Unit.ToString().Equals("AD")))
                return false;

            return true;
        }

        bool noCostData(string code) { return (sqlQ.selectMDataByCode("UnitCosts", code) == null); }
        bool noIngData(string code) { return (sqlQ.selectMDataByCode("Raw_Materials", long.Parse(code)) == null); }

        bool receiptsCheckTypes(object type, object code, object name, object unit, object value)
        {
            if (sNull(type) || sNull(code) || sNull(name) || sNull(unit) || sNull(value))
                return false;
            if (!sDouble(type) || !sDouble(value))
                return false;
            if (!(unit.ToString().Equals("KG") || unit.ToString().Equals("AD") || unit.ToString().Equals("LT") || unit.ToString().Equals("MT") || unit.ToString().Equals("TK")))
                return false;
            if (!(type.ToString() == "4" || type.ToString() == "0"))
                return false;

            return true;
        }

        string receiptCheckDatas(object type, object code)
        {
            if (type.ToString() == "4")
            {
                if (noCostData(code.ToString()))
                    return "noCostData";
            }
            else if (type.ToString() == "0")
            {
                if (noIngData(code.ToString()))
                {
                    return "noIngData";
                }
            }
            return "AllGood";
        }

        void fillColumns(DataTable Data)
        {
            dataGridView1.Columns.Clear();

            if (type != "null")
            {
                if (type == "RAWMATS")
                {
                    dataGridView1.Columns.Add("Code", "CODE");
                    dataGridView1.Columns.Add("Name", "NAME");
                    dataGridView1.Columns.Add("Buyprice", "BUYPRICE");
                    dataGridView1.Columns.Add("Currency", "CURRENCY");


                    dataGridView2.Columns.Add("Code", "CODE");
                    dataGridView2.Columns.Add("Name", "NAME");
                    dataGridView2.Columns.Add("Buyprice", "BUYPRICE");
                    dataGridView2.Columns.Add("Currency", "CURRENCY");

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    foreach (DataGridViewColumn column in dataGridView2.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }


                    foreach (DataRow row in Data.Rows)
                    {
                        if (rawmatsCheckTypes(row[0], row[1], row[2], row[3]))
                        {
                            dataGridView1.Rows.Add(row[0], row[1], row[2], row[3]);
                        }
                        else
                        {
                            dataGridView2.Rows.Add(row[0], row[1], row[2], row[3]);
                        }
                    }
                }
                else if (type == "UNITCOSTS")
                {
                    dataGridView1.Columns.Add("Code", "CODE");
                    dataGridView1.Columns.Add("Name", "NAME");
                    dataGridView1.Columns.Add("UnitCount", "UNITCOUNT");
                    dataGridView1.Columns.Add("Unit", "UNIT");
                    dataGridView1.Columns.Add("DirektIscilik", "DIREKTISCILIK");

                    dataGridView2.Columns.Add("Code", "CODE");
                    dataGridView2.Columns.Add("Name", "NAME");
                    dataGridView2.Columns.Add("UnitCount", "UNITCOUNT");
                    dataGridView2.Columns.Add("Unit", "UNIT");
                    dataGridView2.Columns.Add("DirektIscilik", "DIREKTISCILIK");

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    foreach (DataGridViewColumn column in dataGridView2.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    var lastcol = Data.Columns.Count - 1;

                    foreach (DataRow row in Data.Rows)
                    {
                        if (row[lastcol - 1].ToString().Contains("ADET"))
                            row[lastcol - 1] = "AD";

                        if (unitcostsCheckTypes(row[0], row[1], row[lastcol - 2], row[lastcol - 1], row[lastcol]))
                        {
                            dataGridView1.Rows.Add(row[0], row[1], row[lastcol - 2], row[lastcol - 1], row[lastcol]);
                        }
                        else
                        {
                            dataGridView2.Rows.Add(row[0], row[1], row[lastcol - 2], row[lastcol - 1], row[lastcol]);
                        }
                    }

                }
                else if (type == "ERECEIPTS" || type == "DRECEIPTS")
                {
                    List<string> noCostData = new();
                    Dictionary<string, string> noIngData = new();

                    dataGridView1.Columns.Add("MCode", "MCode");
                    dataGridView1.Columns.Add("Type", "TYPE");
                    dataGridView1.Columns.Add("Code", "CODE");
                    dataGridView1.Columns.Add("Name", "NAME");
                    dataGridView1.Columns.Add("Unit", "UNIT");
                    dataGridView1.Columns.Add("Value", "Value");

                    dataGridView2.Columns.Add("MCode", "MCode");
                    dataGridView2.Columns.Add("Type", "TYPE");
                    dataGridView2.Columns.Add("Code", "CODE");
                    dataGridView2.Columns.Add("Name", "NAME");
                    dataGridView2.Columns.Add("Unit", "UNIT");
                    dataGridView2.Columns.Add("Value", "Value");

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    foreach (DataGridViewColumn column in dataGridView2.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    int x = 0;

                    foreach (DataRow row in Data.Rows)
                    {
                        if (row[5].ToString().Contains("ADET"))
                            row[5] = "AD";
                        if (receiptsCheckTypes(row[2], row[3], row[4], row[5], row[6]))
                        {
                            if (receiptCheckDatas(row[2], row[3]) == "AllGood")
                            {
                                //AllGood Devam
                                dataGridView1.Rows.Add(row[0], row[2], row[3], row[4], row[5], row[6]);

                            }
                            else if (receiptCheckDatas(row[2], row[3]) == "noCostData")
                            {
                                if (!noCostData.Contains(row[3].ToString()))
                                {
                                    noCostData.Add(row[3].ToString());
                                }
                            }
                            else if (receiptCheckDatas(row[2], row[3]) == "noIngData")
                            {

                                if (!noIngData.ContainsKey(row[0].ToString()))
                                {
                                    noIngData.Add(row[0].ToString(), row[3].ToString());
                                }
                                else
                                {
                                    if (!noIngData[row[0].ToString()].Contains(row[3].ToString()))
                                    {
                                        noIngData[row[0].ToString()] = noIngData[row[0].ToString()] + " - " + row[3].ToString();
                                    }
                                }

                            }

                        }
                        else
                        {
                            dataGridView2.Rows.Add(row[0], row[2], row[3], row[4], row[5], row[6]);
                        }
                    }


                    for (int v = 0; v < dataGridView1.Rows.Count; v++)
                    {
                        if (noIngData.ContainsKey(dataGridView1[0, v].Value as string) || noCostData.Contains(dataGridView1[0, v].Value as string))
                        {
                            if (noIngData.ContainsKey(dataGridView1[0, v].Value as string))
                                dataGridView2.Rows.Add("NOINGDATA", dataGridView1[0, v].Value.ToString(), "EKSIK HAMMADDE VAR: " + noIngData[dataGridView1[0, v].Value as string]);
                            if (noCostData.Contains(dataGridView1[0, v].Value as string))
                                dataGridView2.Rows.Add("NOCOSTDATA", dataGridView1[0, v].Value.ToString(), "Direkt Iscilik Degeri Yok.");
                            dataGridView1.Rows.RemoveAt(v);
                            v--;

                        }


                    }


                }
            }

            if (dataGridView2.Rows.Count != 0)
            {
                panel5.Visible = true;
                dataGridView2.Height = (dataGridView2.Rows.Count * 25) + 25;
                if (dataGridView2.Height > 200)
                    dataGridView2.Height = 200;
                panel5.Height = dataGridView2.Height + 40;
            }

            if (dataGridView1.Rows.Count != 0)
            {
                panel2.Visible = true;
                panel2.Height = dataGridView1.Height + 40;
            }

            label3.Text = "PROGRAMA EKLENEMEYEN VERILER: " + dataGridView2.Rows.Count.ToString();
            label5.Text = "PROGRAMA EKLENECEK OLAN VERILER: " + dataGridView1.Rows.Count.ToString();
        }

        private void readDataFromExcel(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet();
                        fillColumns(result.Tables[0]);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lütfen yüklemek istediğiniz dosyanın açık olmadığından emin olun");
            }

            //  checkData();
        }

        private void btn_ImportData_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Worksheets|*.xls;*.xlsx";
            openFileDialog.RestoreDirectory = true;


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                readDataFromExcel(openFileDialog.FileName);

                Cursor.Current = Cursors.Default;

                btn_Cancel.Visible = true;
                btn_AddToDb.Visible = true;

                btn_ImportData.Visible = false;
            }


        }

        private void ImportData_Load(object sender, EventArgs e)
        {
            label1.Text = "IMPORT " + type;
        }


        void clearDgv()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            panel2.Visible = false;
            panel5.Visible = false;

            label3.Text = "PROGRAMA EKLENEMEYEN VERILER";
            label5.Text = "PROGRAMA EKLENECEK OLAN VERILER";
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            clearDgv();

            btn_Cancel.Visible = false;
            btn_AddToDb.Visible = false;
            btn_ImportData.Visible = true;

        }

        private void btn_AddToDb_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
            }

            sqlQ.ImportXtoDb(type, dataGridView1);

            foreach (Control c in this.Controls)
            {
                c.Enabled = true;
            }

            Cursor.Current = Cursors.Default;
            this.Close();

        }
    }
}
