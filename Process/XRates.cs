using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml;

namespace AccounterLite.Process
{
    internal class XRates
    {

        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        DataBase.SQLQuery sqlQ;
        DataBase.SubQ.Rawmats sqRawmats;



        void mongoDbConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            database = dbClient.GetDatabase("Acc_Lite");
            collection = database.GetCollection<BsonDocument>("XRates");

        }

        public XRates()
        {
            if (sqlQ == null) sqlQ = new();
            if (dbClient == null) mongoDbConn();
        }

        string getcDate()
        {

            try
            {
                string tcmb = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(tcmb);

                return xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return "0";
        }

        void UpdateRawMatPrices()
        {
            if (sqRawmats == null) sqRawmats = new();
            sqRawmats.updatePrices();
        }

        void UpdateXrates()
        {
            try
            {
                string tcmb = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(tcmb);

                string Date = xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value.ToString();
                string getusd = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/ForexSelling").InnerXml.ToString().Replace(".", ",");
                string geteur = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/ForexSelling").InnerXml.ToString().Replace(".", ",");

                sqlQ.updateXr(double.Parse(geteur), double.Parse(getusd), Date);

                UpdateRawMatPrices();
            }
            catch (Exception ex) { MessageBox.Show("Kur Guncellenirken Bir Sorun Oluştu. " + "\n" + ex.Message); }


        }

        public void Update()
        {
            int i = 1;
            var filter = Builders<BsonDocument>.Filter.Eq("id", 1);
            bool doesExist = collection.Find(filter).FirstOrDefault() != null;

            if (doesExist)
            {

                string pDate = collection.Find(filter).FirstOrDefault()["Date"].ToString();
                string cDate = getcDate();

                if (cDate != "0")
                {
                    if (pDate != cDate)
                    {
                        UpdateXrates();
                    }
                }
                else
                {
                    MessageBox.Show("Kur degerlerine ulasilamadi");
                }
            }
            else
            {
                UpdateXrates();
            }

        }
    }
}
