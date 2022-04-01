using System.Data;

namespace AccounterLite.UserControls
{
    public partial class UCMain : UserControl
    {
        DataBase.SQLQuery sqlQ;

        void intCmp() { if (sqlQ == null) sqlQ = new(); }
        public void filldata()
        {

            DataRow drow = sqlQ.getMData("Costs").Rows[0],   //domestic
                    erow = sqlQ.getMData("Costs").Rows[1];   //export 


            textBox1.Text = erow["Marj"].ToString().Replace(".", ",");
            textBox2.Text = erow["EndirektIscilik"].ToString().Replace(".", ",");
            textBox3.Text = erow["GenelUretimGider"].ToString().Replace(".", ",");
            textBox4.Text = erow["PazarlamaSatisGider"].ToString().Replace(".", ",");
            textBox5.Text = erow["SatisIscilik"].ToString().Replace(".", ",");
            textBox6.Text = erow["GenelYonetimGider"].ToString().Replace(".", ",");
            textBox7.Text = erow["IdariIscilik"].ToString().Replace(".", ",");

            textBox8.Text = drow["Marj"].ToString().Replace(".", ",");
            textBox9.Text = drow["EndirektIscilik"].ToString().Replace(".", ",");
            textBox10.Text = drow["GenelUretimGider"].ToString().Replace(".", ",");
            textBox11.Text = drow["PazarlamaSatisGider"].ToString().Replace(".", ",");
            textBox12.Text = drow["SatisIscilik"].ToString().Replace(".", ",");
            textBox13.Text = drow["GenelYonetimGider"].ToString().Replace(".", ",");
            textBox14.Text = drow["IdariIscilik"].ToString().Replace(".", ",");

        }

        public UCMain()
        {
            InitializeComponent();
            intCmp();
            filldata();
        }

        void editModeTrigger()
        {
            if (btn_Edit.Visible == false)
            {
                tableLayoutPanel4.Visible = false;
                btn_Edit.Visible = true;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                textBox10.ReadOnly = true;
                textBox11.ReadOnly = true;
                textBox12.ReadOnly = true;
                textBox13.ReadOnly = true;
                textBox14.ReadOnly = true;
                textBox1.BackColor = Color.LightCyan;
                textBox2.BackColor = Color.LightCyan;
                textBox3.BackColor = Color.LightCyan;
                textBox4.BackColor = Color.LightCyan;
                textBox5.BackColor = Color.LightCyan;
                textBox6.BackColor = Color.LightCyan;
                textBox7.BackColor = Color.LightCyan;
                textBox8.BackColor = Color.LightCyan;
                textBox9.BackColor = Color.LightCyan;
                textBox10.BackColor = Color.LightCyan;
                textBox11.BackColor = Color.LightCyan;
                textBox12.BackColor = Color.LightCyan;
                textBox13.BackColor = Color.LightCyan;
                textBox14.BackColor = Color.LightCyan;
            }
            else
            {
                tableLayoutPanel4.Visible = true;
                btn_Edit.Visible = false;
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
                textBox1.BackColor = Color.White;
                textBox2.BackColor = Color.White;
                textBox3.BackColor = Color.White;
                textBox4.BackColor = Color.White;
                textBox5.BackColor = Color.White;
                textBox6.BackColor = Color.White;
                textBox7.BackColor = Color.White;
                textBox8.BackColor = Color.White;
                textBox9.BackColor = Color.White;
                textBox10.BackColor = Color.White;
                textBox11.BackColor = Color.White;
                textBox12.BackColor = Color.White;
                textBox13.BackColor = Color.White;
                textBox14.BackColor = Color.White;
            }
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            editModeTrigger();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            editModeTrigger();
            filldata();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (checkTextBoxDatas())
            {

                decimal[] eCostArr = { cToDouble(textBox1.Text),
                                      cToDouble(textBox2.Text),
                                      cToDouble(textBox3.Text),
                                      cToDouble(textBox4.Text),
                                      cToDouble(textBox5.Text),
                                      cToDouble(textBox6.Text),
                                      cToDouble(textBox7.Text) };
                decimal[] dCostArr = { cToDouble(textBox8.Text),
                                      cToDouble(textBox9.Text),
                                      cToDouble(textBox10.Text),
                                      cToDouble(textBox11.Text),
                                      cToDouble(textBox12.Text),
                                      cToDouble(textBox13.Text),
                                      cToDouble(textBox14.Text) };
                try
                {
                    sqlQ.UpdateCosts("Export", eCostArr);
                    sqlQ.UpdateCosts("Domestic", dCostArr);
                }
                catch (Exception _) { }


                editModeTrigger();
                filldata();
            }
            else
            {
                MessageBox.Show("Girdiginiz Degerleri Kontrol Edin.", "HATALI GIRDI");
            }

        }

        decimal cToDouble(object var) { return decimal.Parse(var.ToString()); } //check String if null or empty

        bool cDouble(object var) { return !double.TryParse(var.ToString(), out _); }

        bool checkTextBoxDatas()
        {
            if (cDouble(textBox1.Text) ||
                cDouble(textBox2.Text) ||
                cDouble(textBox3.Text) ||
                cDouble(textBox4.Text) ||
                cDouble(textBox5.Text) ||
                cDouble(textBox6.Text) ||
                cDouble(textBox7.Text) ||
                cDouble(textBox8.Text) ||
                cDouble(textBox9.Text) ||
                cDouble(textBox10.Text) ||
                cDouble(textBox11.Text) ||
                cDouble(textBox12.Text) ||
                cDouble(textBox13.Text) ||
                cDouble(textBox14.Text))
                return false;

            return true;
        }
    }
}
