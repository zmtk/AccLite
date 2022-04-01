using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace AccounterLite.DataBase.SubQ
{
    internal class Receipts
    {
        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;


        int aCount, uCount;

        void mongoDBConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            database = dbClient.GetDatabase("Acc_Lite");
        }

        public Receipts()
        {
            if (dbClient == null) mongoDBConn();
        }



        public DataTable getData(string type)
        {
            if (type == "ERECEIPTS")
                collection = database.GetCollection<BsonDocument>("Receipts_Export");
            if (type == "DRECEIPTS")
                collection = database.GetCollection<BsonDocument>("Receipts_Domestic");

            DataTable dt = new();

            dt.Columns.Add("CODE");
            dt.Columns.Add("NAME");

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var item in documents)
            {
                dt.Rows.Add(item["Code"], item["Name"]);
            }

            return dt;
        }

        DataGridView ReverseDGVRows(DataGridView dgv)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            rows.AddRange(dgv.Rows.Cast<DataGridViewRow>());
            rows.Reverse();
            dgv.Rows.Clear();
            dgv.Rows.AddRange(rows.ToArray());

            return dgv;
        }


        void addReceipt(BsonDocument receipt)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Code", receipt["Code"]);
            bool doesExist = collection.Find(filter).FirstOrDefault() != null;

            if (doesExist)
            {
                var update = Builders<BsonDocument>.Update.Set("Name", receipt["Name"])
                                                          .Set("Ingredients", receipt["Ingredients"])
                                                          .Set("PackIngredients", receipt["PackIngredients"]);
                collection.UpdateOne(filter, update);
                uCount++;

            }
            else
            {
                collection.InsertOne(receipt);
                aCount++;
            }

        }

        public void ImportXtoDb(string type, DataGridView dgv)
        {
            aCount = 0; uCount = 0;

            if (type == "ERECEIPTS")
                collection = database.GetCollection<BsonDocument>("Receipts_Export");
            if (type == "DRECEIPTS")
                collection = database.GetCollection<BsonDocument>("Receipts_Domestic");

            dgv = ReverseDGVRows(dgv);
            BsonArray ingArr = new(), packingArr = new();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                string rcode = row.Cells[0].Value.ToString(),
                       rtype = row.Cells[1].Value.ToString(),
                       code = row.Cells[2].Value.ToString(),
                       name = row.Cells[3].Value.ToString(),
                       unit = row.Cells[4].Value.ToString();

                double value = double.Parse(row.Cells[5].Value.ToString());
                if (rtype == "4")
                {
                    var receipt = new BsonDocument
                    {
                         { "Code", rcode },
                         { "Name", name },
                         { "Ingredients", ingArr },
                         { "PackIngredients", packingArr }
                    };

                    if (ingArr.Count == 0)
                    {
                        MessageBox.Show(rcode + " Kodlu Reçete İçeriği olmadığı için eklenemedi.");
                    }
                    else
                    {
                        addReceipt(receipt);
                    }
                    ingArr.Clear();
                    packingArr.Clear();
                }
                else if (rtype == "0")
                {
                    if (code[1] == '0')
                    {
                        ingArr.Add(new BsonDocument { { "code", code }, { "unit", unit }, { "value", value } });
                    }
                    else
                    {
                        packingArr.Add(new BsonDocument { { "code", code }, { "unit", unit }, { "value", value } });
                    }
                }


            }

            MessageBox.Show("Added : " + aCount + "\n" + "Updated : " + uCount);
        }

    }
}
