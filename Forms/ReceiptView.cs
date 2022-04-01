using MongoDB.Bson;
using System.Data;

namespace AccounterLite.Forms
{
    public partial class ReceiptView : Form
    {

        bool printIng, printPacking, printHeader, printEndTable;
        int totalnumber, itemperpage, Pcknumber, Fnnumber, sheaderpg;

        string rcode, rtype;
        double ingWeight;
        long unitCount;

        DataBase.SQLQuery sqlQ;
        BsonDocument Receipt, UnitCost, Costs;

        void intCmp() { if (sqlQ == null) sqlQ = new(); }

        public ReceiptView(string rCode, string rType)
        {


            InitializeComponent();
            intCmp();
            rcode = rCode;
            rtype = rType;

        }


        void getReceiptData()
        {
            Receipt = sqlQ.selectMDataByCode("Receipts_" + rtype, rcode);
        }
        void getUnitCost()
        {
            UnitCost = sqlQ.selectMDataByCode("UnitCosts", rcode);
        }
        void getCosts()
        {
            Costs = sqlQ.selectMDataByType("Costs", rtype);
        }
        BsonValue getUnitName(string code) { return sqlQ.selectMDataByCode("Raw_Materials", double.Parse(code))["Name"]; }
        BsonValue getUnitPrice(string code) { return sqlQ.selectMDataByCode("Raw_Materials", double.Parse(code))["Finalprice"]; }

        double gd(object var) { return double.Parse(var.ToString()); }



        string sF(object var)
        {
            return String.Format("{0:0.00}", gd(var));
        }


        void FinalTableRow(object var, object var2, object var3)
        {
            dataGridView2.Rows.Add(var, sF(var2), sF(var3));
        }

        void FinalTableRow(object var, object var2, object var3, object var4)
        {
            dataGridView2.Rows.Add(var, sF(var2), sF(var3), sF(var4));
        }
        double wToT(object var) // weight to total
        {
            return gd(var) * ingWeight;
        }

        double wToU(object var) // weight to total
        {
            return (gd(var) * ingWeight) / unitCount;
        }

        void addFinalTable(double pingkcost, double ingkcost)
        {
            double prcost = pingkcost + ingkcost + gd(UnitCost["DirektIscilik"]) + gd(Costs["EndirektIscilik"]) + gd(Costs["GenelUretimGider"]);
            double ttcost = prcost + gd(Costs["PazarlamaSatisGider"]) + gd(Costs["SatisIscilik"]) + gd(Costs["GenelYonetimGider"]) + gd(Costs["IdariIscilik"]);
            double bayisatis = ttcost * gd(Costs["Marj"]);

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Add("vname", "vname");
            dataGridView2.Columns.Add("tvalue", "tvalue");
            dataGridView2.Columns.Add("kvalue", "kvalue");

            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView2.Columns[1].Width = 65;
            dataGridView2.Columns[2].Width = 65;

            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            FinalTableRow("MALZEME TOPLAMI", wToT(pingkcost + ingkcost), pingkcost + ingkcost);
            FinalTableRow("DİREKT İŞÇİLİK", wToT(UnitCost["DirektIscilik"]), gd(UnitCost["DirektIscilik"]));
            FinalTableRow("ENDİREKT İŞÇİLİK", wToT(Costs["EndirektIscilik"]), gd(Costs["EndirektIscilik"]));
            FinalTableRow("GENEL ÜRETİM GİDERİ", wToT(Costs["GenelUretimGider"]), gd(Costs["GenelUretimGider"]));
            FinalTableRow("ÜRETİM MALİYETİ", wToT(prcost), prcost);
            FinalTableRow("PAZARLAMA SATIŞ GİDERİ", wToT(Costs["PazarlamaSatisGider"]), gd(Costs["PazarlamaSatisGider"]));
            FinalTableRow("SATIŞ İŞÇİLİK", wToT(Costs["SatisIscilik"]), gd(Costs["SatisIscilik"]));
            FinalTableRow("GENEL YÖNETİM GİDERİ", wToT(Costs["GenelYonetimGider"]), gd(Costs["GenelYonetimGider"]));
            FinalTableRow("İDARİ İŞÇİLİK", wToT(Costs["IdariIscilik"]), gd(Costs["IdariIscilik"]));
            FinalTableRow("TOPLAM MALİYET", wToT(ttcost), ttcost);
            FinalTableRow("BAYİ SATIŞ FİYATI", wToT(bayisatis), bayisatis);

            dataGridView2.Rows[0].DefaultCellStyle.BackColor = Color.Coral;
            dataGridView2.Rows[4].DefaultCellStyle.BackColor = Color.Coral;
            dataGridView2.Rows[9].DefaultCellStyle.BackColor = Color.Coral;

        }

        void addFinalTableWithUnit(double pingkcost, double ingkcost)
        {
            double prcost = pingkcost + ingkcost + gd(UnitCost["DirektIscilik"]) + gd(Costs["EndirektIscilik"]) + gd(Costs["GenelUretimGider"]);
            double ttcost = prcost + gd(Costs["PazarlamaSatisGider"]) + gd(Costs["SatisIscilik"]) + gd(Costs["GenelYonetimGider"]) + gd(Costs["IdariIscilik"]);
            double bayisatis = ttcost * gd(Costs["Marj"]);

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Add("vname", "vname");
            dataGridView2.Columns.Add("tvalue", "tvalue");
            dataGridView2.Columns.Add("kvalue", "kvalue");
            dataGridView2.Columns.Add("uvalue", "uvalue");

            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView2.Columns[1].Width = 65;
            dataGridView2.Columns[2].Width = 65;
            dataGridView2.Columns[3].Width = 65;
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            FinalTableRow("MALZEME TOPLAMI", wToT(pingkcost + ingkcost), pingkcost + ingkcost, wToU(pingkcost + ingkcost));
            FinalTableRow("DİREKT İŞÇİLİK", wToT(UnitCost["DirektIscilik"]), gd(UnitCost["DirektIscilik"]), wToU(UnitCost["DirektIscilik"]));
            FinalTableRow("ENDİREKT İŞÇİLİK", wToT(Costs["EndirektIscilik"]), gd(Costs["EndirektIscilik"]), wToU(Costs["EndirektIscilik"]));
            FinalTableRow("GENEL ÜRETİM GİDERİ", wToT(Costs["GenelUretimGider"]), gd(Costs["GenelUretimGider"]), wToU(Costs["GenelUretimGider"]));
            FinalTableRow("ÜRETİM MALİYETİ", wToT(prcost), prcost, wToU(prcost));
            FinalTableRow("PAZARLAMA SATIŞ GİDERİ", wToT(Costs["PazarlamaSatisGider"]), gd(Costs["PazarlamaSatisGider"]), wToU(Costs["PazarlamaSatisGider"]));
            FinalTableRow("SATIŞ İŞÇİLİK", wToT(Costs["SatisIscilik"]), gd(Costs["SatisIscilik"]), wToU(Costs["SatisIscilik"]));
            FinalTableRow("GENEL YÖNETİM GİDERİ", wToT(Costs["GenelYonetimGider"]), gd(Costs["GenelYonetimGider"]), wToU(Costs["GenelYonetimGider"]));
            FinalTableRow("İDARİ İŞÇİLİK", wToT(Costs["IdariIscilik"]), gd(Costs["IdariIscilik"]), wToU(Costs["IdariIscilik"]));
            FinalTableRow("TOPLAM MALİYET", wToT(ttcost), ttcost, wToU(ttcost));
            FinalTableRow("BAYİ SATIŞ FİYATI", wToT(bayisatis), bayisatis, wToU(bayisatis));

            dataGridView2.Rows[0].DefaultCellStyle.BackColor = Color.Coral;
            dataGridView2.Rows[4].DefaultCellStyle.BackColor = Color.Coral;
            dataGridView2.Rows[9].DefaultCellStyle.BackColor = Color.Coral;

        }

        private void btn_Print_Click(object sender, EventArgs e)
        {

            printIng = printHeader = printEndTable = true;  //

            totalnumber = itemperpage = Pcknumber = Fnnumber = 0;
            sheaderpg = 1;

            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                    MessageBox.Show("Yazdirma islemi basarili.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private int drawFinalTable(int y, DataGridViewRow row, System.Drawing.Printing.PrintPageEventArgs e)
        {

            Font cellFont = new Font("Arial", 8);
            Font boldcellFont = new Font("Arial", 8, FontStyle.Bold);
            Font scellfont = new Font("Arial", 7, FontStyle.Italic);
            Font typeFont = cellFont;





            if (row.Cells["vname"].Value.ToString().Contains("MALZEME") ||
                row.Cells["vname"].Value.ToString().Contains("MALİYET"))
            {
                typeFont = boldcellFont;
                y += 2;

                SolidBrush grayBrush = new SolidBrush(Color.Gainsboro);
                if (UnitCost["Unit"] != "AD")
                {
                    Rectangle rect = new Rectangle(85, y - 1, 745, 15);
                    e.Graphics.FillRectangle(grayBrush, rect);

                }
                else
                {
                    Rectangle rect = new Rectangle(85, y - 1, 735, 15);
                    e.Graphics.FillRectangle(grayBrush, rect);

                }
            }

            RectangleF[] cellRect = new RectangleF[6];

            int cellX = 460;
            for (int i = 0; i < 6; i++)
            {
                if (UnitCost["Unit"] != "AD")
                    cellX += 10;
                cellRect[i] = new RectangleF(cellX, y, 60, (float)28);
                cellX += 60;
            }

            var format = new StringFormat() { Alignment = StringAlignment.Far };


            e.Graphics.DrawString(row.Cells["vname"].Value.ToString(), typeFont, Brushes.Black, 90, y);
            e.Graphics.DrawString(row.Cells["tvalue"].Value.ToString(), typeFont, Brushes.Black, cellRect[3], format);
            e.Graphics.DrawString(row.Cells["kvalue"].Value.ToString(), typeFont, Brushes.Black, cellRect[4], format);


            if (UnitCost["Unit"] == "AD")
                e.Graphics.DrawString(row.Cells["uvalue"].Value.ToString(), typeFont, Brushes.Black, cellRect[5], format);

            y += 17;
            typeFont = scellfont;

            return y;

        }

        private int drawRow(int y, DataGridViewRow row, System.Drawing.Printing.PrintPageEventArgs e)
        {

            Font cellFont = new Font("Arial", 8);
            Font typeFont = cellFont;
            Font boldcellFont = new Font("Arial", 8, FontStyle.Bold);

            int cellX = 460;

            RectangleF nameRect = new RectangleF(90, y, 250, (float)28);

            RectangleF[] cellRect = new RectangleF[6];

            for (int i = 0; i < 6; i++)
            {
                if (UnitCost["Unit"] != "AD")
                    cellX += 10;
                cellRect[i] = new RectangleF(cellX, y, 60, (float)28);
                cellX += 60;
            }
            var format = new StringFormat() { Alignment = StringAlignment.Far };

            if (String.IsNullOrWhiteSpace(row.Cells["Code"].ToString()))
                typeFont = boldcellFont;


            e.Graphics.DrawString(row.Cells["Code"].Value.ToString(), typeFont, Brushes.Black, 30, y);
            e.Graphics.DrawString(row.Cells["Name"].Value.ToString(), typeFont, Brushes.Black, nameRect);
            e.Graphics.DrawString(row.Cells["Value"].Value.ToString(), typeFont, Brushes.Black, 340, y);
            e.Graphics.DrawString(row.Cells["Unit"].Value.ToString(), typeFont, Brushes.Black, 435, y);
            e.Graphics.DrawString(row.Cells["UnitPrice"].Value.ToString(), typeFont, Brushes.Black, cellRect[0], format);
            e.Graphics.DrawString(row.Cells["Cost"].Value.ToString(), typeFont, Brushes.Black, cellRect[1], format);
            e.Graphics.DrawString(row.Cells["WasteCost"].Value.ToString(), typeFont, Brushes.Black, cellRect[2], format);
            e.Graphics.DrawString(row.Cells["TotalCost"].Value.ToString(), typeFont, Brushes.Black, cellRect[3], format);
            e.Graphics.DrawString(row.Cells["WCost"].Value.ToString(), typeFont, Brushes.Black, cellRect[4], format);

            if (UnitCost["Unit"] == "AD")
                e.Graphics.DrawString(row.Cells["UnitCost"].Value.ToString(), typeFont, Brushes.Black, cellRect[5], format);

            SizeF stringSize = e.Graphics.MeasureString(row.Cells["Name"].Value.ToString(), typeFont);

            if (stringSize.Width > 249)
            {
                y += 11;
                itemperpage++;

            }
            y += 20;

            typeFont = cellFont;

            return y;

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            DataTable printData;
            Font cellHeader = new Font("Arial", 9, FontStyle.Bold);


            Font cellFont = new Font("Arial", 8);
            Font boldcellFont = new Font("Arial", 8, FontStyle.Bold);
            Font scellfont = new Font("Arial", 7, FontStyle.Italic);
            Font lineFont = new Font("Arial", 11, FontStyle.Bold);

            Font typeFont = cellFont;


            string prCode = rcode,
                   prName = Receipt["Name"].ToString();

            string line = "_____________________________________________________________________________________________________";

            int y = 15;




            if (printHeader)
            {
                RectangleF[] cellRect = new RectangleF[6];
                RectangleF[] cellRectT = new RectangleF[2];

                int cellX = 460;
                for (int i = 0; i < 6; i++)
                {
                    if (!dataGridView1.Columns.Contains("UnitCost"))
                        cellX += 10;
                    cellRect[i] = new RectangleF(cellX, 90, 60, (float)28);


                    cellX += 60;
                }

                if (!dataGridView1.Columns.Contains("UnitCost"))
                {
                    cellX = 600;
                }
                else
                {
                    cellX = 580;
                }
                for (int i = 0; i < 2; i++)
                {
                    if (!dataGridView1.Columns.Contains("UnitCost"))
                        cellX += 10;
                    cellRectT[i] = new RectangleF(cellX, 70, 60, (float)28);

                    cellX += 60;
                }

                var format = new StringFormat() { Alignment = StringAlignment.Far };

                e.Graphics.DrawString(prCode + " - " + prName, lineFont, Brushes.Black, 50, 35);

                e.Graphics.DrawString("KODU", cellHeader, Brushes.Black, 30, 90);
                e.Graphics.DrawString("Ürün Adı", cellHeader, Brushes.Black, 90, 90);
                e.Graphics.DrawString("Miktar", cellHeader, Brushes.Black, 340, 90);
                e.Graphics.DrawString("Birim", cellHeader, Brushes.Black, 430, 90);
                e.Graphics.DrawString("B.Fiyat", cellHeader, Brushes.Black, cellRect[0], format);
                e.Graphics.DrawString("Maliyet", cellHeader, Brushes.Black, cellRect[1], format);

                e.Graphics.DrawString("3% Fire", cellHeader, Brushes.Black, cellRectT[0], format);
                e.Graphics.DrawString("Maliyeti", cellHeader, Brushes.Black, cellRect[2], format);

                e.Graphics.DrawString("Toplam", cellHeader, Brushes.Black, cellRectT[1], format);
                e.Graphics.DrawString("Maliyet", cellHeader, Brushes.Black, cellRect[3], format);

                e.Graphics.DrawString("1 Kg", cellHeader, Brushes.Black, cellRect[4], format);

                if (dataGridView1.Columns.Contains("UnitCost"))
                    e.Graphics.DrawString("1 Adet", cellHeader, Brushes.Black, cellRect[5], format);


                e.Graphics.DrawString(line, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 25, 100);
                y += 110;
                printHeader = false;
            }
            else
            {

                string sheader = prCode + " - " + prName + " - " + (++sheaderpg).ToString();

                SizeF sheaderSize = new SizeF();
                sheaderSize = e.Graphics.MeasureString(sheader, scellfont);

                e.Graphics.DrawString(sheader, DefaultFont, Brushes.Black, 790 - sheaderSize.Width, y);
                e.Graphics.DrawString(line, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 25, y + 15);

                y += 40;

            }



            // declare  one variable for height measurement

            if (printIng)
            {
                while (totalnumber < dataGridView1.Rows.Count) // check the number of items
                {

                    y = drawRow(y, dataGridView1.Rows[totalnumber], e);

                    totalnumber += 1; //increment count by 1
                    if (itemperpage < 46) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

                    }

                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again

                    }
                }
                printIng = false;
            }

            if (printEndTable)
            {

                while (Fnnumber < dataGridView2.Rows.Count) // check the number of items
                {

                    //increment count by 1
                    if (itemperpage > 35 && Fnnumber == 0) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        return;//It will call PrintPage event again        
                    }
                    else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added
                    }

                    y = drawFinalTable(y, dataGridView2.Rows[Fnnumber], e);
                    Fnnumber += 1;


                }

                printEndTable = false;
            }

        }





        private void ReceiptView_Load(object sender, EventArgs e)
        {
            getReceiptData();
            getUnitCost();
            getCosts();

            lbl_Rcode.Text = Receipt["Code"] + " - " + Receipt["Name"] + " - " + UnitCost["UnitCount"] + UnitCost["Unit"];

            if (UnitCost["Unit"] == "KG")
                ReceiptWithNoUnit();
            if (UnitCost["Unit"] == "AD")
                ReceiptWithUnit();

        }
        private void ReceiptWithUnit()
        {
            BsonArray Ings = Receipt["Ingredients"].AsBsonArray,
                      PackIngs = Receipt["PackIngredients"].AsBsonArray;

            double ingcost = 0, pingcost = 0,
                     ingwcost = 0, pingwcost = 0,
                     ingtcost = 0, pingtcost = 0,
                     ingkcost = 0, pingkcost = 0,
                     ingucost = 0, pingucost = 0;

            ingWeight = 0;
            unitCount = long.Parse(UnitCost["UnitCount"].ToString());

            foreach (BsonDocument item in Ings)
            {
                ingWeight += gd(item["value"]);
            }

            foreach (BsonDocument item in Ings)
            {
                item.Add("name", getUnitName(item["code"].ToString()));
                item.Add("unitprice", getUnitPrice(item["code"].ToString()));
                item.Add("cost", gd(item["unitprice"]) * gd(item["value"]));
                item.Add("wcost", gd(item["cost"]) * 3 / 100);
                item.Add("tcost", gd(item["cost"]) * 1.03);
                item.Add("kcost", gd(item["tcost"]) / ingWeight);
                item.Add("ucost", gd(item["tcost"]) / unitCount);

                ingcost += gd(item["cost"]);
                ingwcost += gd(item["wcost"]);
                ingtcost += gd(item["tcost"]);
                ingkcost += gd(item["kcost"]);
                ingucost += gd(item["ucost"]);

            }
            foreach (BsonDocument item in PackIngs)
            {
                item.Add("name", getUnitName(item["code"].ToString()));
                item.Add("unitprice", getUnitPrice(item["code"].ToString()));
                item.Add("cost", gd(item["unitprice"]) * gd(item["value"]));
                item.Add("wcost", gd(item["cost"]) * 3 / 100);
                item.Add("tcost", gd(item["cost"]) * 1.03);
                item.Add("kcost", gd(item["tcost"]) / ingWeight);
                item.Add("ucost", gd(item["tcost"]) / unitCount);

                pingcost += gd(item["cost"]);
                pingwcost += gd(item["wcost"]);
                pingtcost += gd(item["tcost"]);
                pingkcost += gd(item["kcost"]);
                pingucost += gd(item["ucost"]);

            }


            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("Code", "KOD");
                dataGridView1.Columns.Add("Name", "NAME");
                dataGridView1.Columns.Add("Value", "MIKTAR");
                dataGridView1.Columns.Add("Unit", "BIRIM");
                dataGridView1.Columns.Add("UnitPrice", "B.FIYAT");
                dataGridView1.Columns.Add("Cost", "MALIYET");
                dataGridView1.Columns.Add("WasteCost", "3% FIRE" + "\n" + "MALIYET");
                dataGridView1.Columns.Add("TotalCost", "TOPLAM" + "\n" + "MALIYET");
                dataGridView1.Columns.Add("WCost", "1 KG");
                dataGridView1.Columns.Add("UnitCost", "ADET");

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;

                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.Width = 65;
                }
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }



            dataGridView1.Rows.Clear();

            for (int v = 0; v < Ings.Count; v++)
            {
                int x = Ings.Count - 1 - v;
                dataGridView1.Rows.Add(Ings[x]["code"], Ings[x]["name"], sF(Ings[x]["value"]), Ings[x]["unit"], sF(Ings[x]["unitprice"]), sF(Ings[x]["cost"]), sF(Ings[x]["wcost"]), sF(Ings[x]["tcost"]), sF(Ings[x]["kcost"]), sF(Ings[x]["ucost"]));
            }

            dataGridView1.Rows.Add("", "", sF(ingWeight), "", "", sF(ingcost), sF(ingwcost), sF(ingtcost), sF(ingkcost), sF(ingucost));
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Coral;

            for (int v = 0; v < PackIngs.Count; v++)
            {
                int x = PackIngs.Count - 1 - v;
                dataGridView1.Rows.Add(PackIngs[x]["code"], PackIngs[x]["name"], sF(PackIngs[x]["value"]), PackIngs[x]["unit"], sF(PackIngs[x]["unitprice"]), sF(PackIngs[x]["cost"]), sF(PackIngs[x]["wcost"]), sF(PackIngs[x]["tcost"]), sF(PackIngs[x]["kcost"]), sF(PackIngs[x]["ucost"]));
            }
            dataGridView1.Rows.Add("", "", "", "", "", sF(pingcost), sF(pingwcost), sF(pingtcost), sF(pingkcost), sF(pingucost));
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Coral;


            addFinalTableWithUnit(pingkcost, ingkcost);
        }

        private void ReceiptWithNoUnit()
        {
            BsonArray Ings = Receipt["Ingredients"].AsBsonArray,
                      PackIngs = Receipt["PackIngredients"].AsBsonArray;

            double ingcost = 0, pingcost = 0,
                     ingwcost = 0, pingwcost = 0,
                     ingtcost = 0, pingtcost = 0,
                     ingkcost = 0, pingkcost = 0;

            ingWeight = 0;

            foreach (BsonDocument item in Ings)
            {
                ingWeight += gd(item["value"]);
            }

            foreach (BsonDocument item in Ings)
            {
                item.Add("name", getUnitName(item["code"].ToString()));
                item.Add("unitprice", getUnitPrice(item["code"].ToString()));
                item.Add("cost", gd(item["unitprice"]) * gd(item["value"]));
                item.Add("wcost", gd(item["cost"]) * 3 / 100);
                item.Add("tcost", gd(item["cost"]) * 1.03);
                item.Add("kcost", gd(item["tcost"]) / ingWeight);

                ingcost += gd(item["cost"]);
                ingwcost += gd(item["wcost"]);
                ingtcost += gd(item["tcost"]);
                ingkcost += gd(item["kcost"]);

            }
            foreach (BsonDocument item in PackIngs)
            {
                item.Add("name", getUnitName(item["code"].ToString()));
                item.Add("unitprice", getUnitPrice(item["code"].ToString()));
                item.Add("cost", gd(item["unitprice"]) * gd(item["value"]));
                item.Add("wcost", gd(item["cost"]) * 3 / 100);
                item.Add("tcost", gd(item["cost"]) * 1.03);
                item.Add("kcost", gd(item["tcost"]) / ingWeight);

                pingcost += gd(item["cost"]);
                pingwcost += gd(item["wcost"]);
                pingtcost += gd(item["tcost"]);
                pingkcost += gd(item["kcost"]);
            }


            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("Code", "KOD");
                dataGridView1.Columns.Add("Name", "NAME");
                dataGridView1.Columns.Add("Value", "MIKTAR");
                dataGridView1.Columns.Add("Unit", "BIRIM");
                dataGridView1.Columns.Add("UnitPrice", "B.FIYAT");
                dataGridView1.Columns.Add("Cost", "MALIYET");
                dataGridView1.Columns.Add("WasteCost", "3% FIRE" + "\n" + "MALIYET");
                dataGridView1.Columns.Add("TotalCost", "TOPLAM" + "\n" + "MALIYET");
                dataGridView1.Columns.Add("WCost", "1 KG");

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;

                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.Width = 65;
                }
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }



            dataGridView1.Rows.Clear();

            for (int v = 0; v < Ings.Count; v++)
            {
                int x = Ings.Count - 1 - v;
                dataGridView1.Rows.Add(Ings[x]["code"], Ings[x]["name"], sF(Ings[x]["value"]), Ings[x]["unit"], sF(Ings[x]["unitprice"]), sF(Ings[x]["cost"]), sF(Ings[x]["wcost"]), sF(Ings[x]["tcost"]), sF(Ings[x]["kcost"]));
            }
            ;
            dataGridView1.Rows.Add("", "", sF(ingWeight), "", "", sF(ingcost), sF(ingwcost), sF(ingtcost), sF(ingkcost));
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Coral;

            for (int v = 0; v < PackIngs.Count; v++)
            {
                int x = PackIngs.Count - 1 - v;
                dataGridView1.Rows.Add(PackIngs[x]["code"], PackIngs[x]["name"], sF(PackIngs[x]["value"]), PackIngs[x]["unit"], sF(PackIngs[x]["unitprice"]), sF(PackIngs[x]["cost"]), sF(PackIngs[x]["wcost"]), sF(PackIngs[x]["tcost"]), sF(PackIngs[x]["kcost"]));
            }

            dataGridView1.Rows.Add("", "", "", "", "", sF(pingcost), sF(pingwcost), sF(pingtcost), sF(pingkcost));
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Coral;


            addFinalTable(pingkcost, ingkcost);
        }
    }
}
