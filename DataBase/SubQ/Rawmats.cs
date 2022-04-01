using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace AccounterLite.DataBase.SubQ
{
    internal class Rawmats
    {

        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        SQLQuery sqlQ;
        double Eur = 0, Usd = 0;


        void mongoDBConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            database = dbClient.GetDatabase("Acc_Lite");
            collection = database.GetCollection<BsonDocument>("Raw_Materials");
        }


        public Rawmats()
        {
            if (dbClient == null) mongoDBConn();
            if (sqlQ == null) sqlQ = new();

        }

        void updatexR()
        {
            if (Eur == 0 || Usd == 0)
            {
                Eur = double.Parse(sqlQ.getMData("XRates").Rows[0]["Eur"].ToString());
                Usd = double.Parse(sqlQ.getMData("XRates").Rows[0]["Usd"].ToString());

            }

        }

        public void updatePrices()
        {
            updatexR();

            var filter = Builders<BsonDocument>.Filter.Ne("curr", "TL");

            var documents = collection.Find(filter).ToList();

            foreach (var doc in documents)
            {
                updatePrice(doc["Code"].ToString(), doc["Unitprice"].ToString(), doc["Curr"].ToString());
            }
        }

        void updatePrice(string code, string bprice, string currtype)
        {

            double finalprice = double.Parse(bprice);

            if (currtype == "EUR")
                finalprice = double.Parse(bprice) * Eur;
            if (currtype == "USD")
                finalprice = double.Parse(bprice) * Usd;

            var filter = Builders<BsonDocument>.Filter.Eq("Code", code);

            var update = Builders<BsonDocument>.Update.Set("Finalprice", finalprice);

            collection.UpdateOne(filter, update);
        }

        public DataTable getData()
        {
            DataTable dt = new();

            dt.Columns.Add("CODE");
            dt.Columns.Add("NAME");
            dt.Columns.Add("UNITPRICE");
            dt.Columns.Add("CURRENCY");
            dt.Columns.Add("FINALPRICE");

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var item in documents)
            {
                dt.Rows.Add(item["Code"], item["Name"], item["Unitprice"], item["Curr"], item["Finalprice"]);
            }

            return dt;
        }

        double gd(object var) { return double.Parse(var.ToString()); }
        string sF(object var) { return String.Format("{0:0.00}", gd(var)); }
        public void ImportXtoDb(DataGridView dgv)  //mongo
        {
            int aCount = 0, uCount = 0;

            updatexR();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                string name = row.Cells[1].Value.ToString(),
                       curr = row.Cells[3].Value.ToString();

                long code = long.Parse(row.Cells[0].Value.ToString());
                double unitprice = double.Parse(row.Cells[2].Value.ToString()),
                       finalprice = unitprice;

                if (curr == "EUR")
                    finalprice = unitprice * Eur;
                if (curr == "USD")
                    finalprice = unitprice * Usd;




                var filter = Builders<BsonDocument>.Filter.Eq("Code", code);
                var rawMaterial = collection.Find(filter).FirstOrDefault();

                if (rawMaterial == null)
                {
                    var document = new BsonDocument
                    {
                        { "Code", code },
                        { "Name", name },
                        { "Curr", curr },
                        { "Unitprice", sF(unitprice) },
                        { "Finalprice", sF(finalprice) }
                    };

                    collection.InsertOne(document);
                    aCount++;
                }
                else
                {
                    var update = Builders<BsonDocument>.Update.Set("Name", name)
                                                              .Set("Curr", curr)
                                                              .Set("Unitprice", sF(unitprice))
                                                              .Set("Finalprice", sF(finalprice));

                    collection.UpdateOne(filter, update);

                    uCount++;
                }

            }

            MessageBox.Show("Added : " + aCount + "\n" + "Updated : " + uCount);
        }
    }
}
