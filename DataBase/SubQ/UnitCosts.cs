using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;


namespace AccounterLite.DataBase.SubQ
{
    internal class UnitCosts
    {
        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        void mongoDBConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");

            database = dbClient.GetDatabase("Acc_Lite");
            collection = database.GetCollection<BsonDocument>("UnitCosts");
        }

        public UnitCosts()
        {
            if (dbClient == null) mongoDBConn();
        }

        public DataTable getData()
        {
            DataTable dt = new();

            dt.Columns.Add("CODE");
            dt.Columns.Add("NAME");
            dt.Columns.Add("UNITCOUNT");
            dt.Columns.Add("UNIT");
            dt.Columns.Add("DIREKTISCILIK");

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var item in documents)
            {
                dt.Rows.Add(item["Code"], item["Name"], item["UnitCount"], item["Unit"], item["DirektIscilik"]);
            }

            return dt;
        }

        double gd(object var) { return double.Parse(var.ToString()); }
        string sF(object var) { return String.Format("{0:0.00}", gd(var)); }

        public void ImportXtoDb(DataGridView dgv)
        {
            int aCount = 0, uCount = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                string code = row.Cells[0].Value.ToString(),
                       name = row.Cells[1].Value.ToString(),
                       unit = row.Cells[3].Value.ToString(),
                       UnitCount = row.Cells[2].Value.ToString() ,
                       DirektIscilik = sF(row.Cells[4].Value.ToString());

                var filter = Builders<BsonDocument>.Filter.Eq("Code", code);
                bool doesExist = collection.Find(filter).FirstOrDefault() != null;

                if (doesExist)
                {
                    var update = Builders<BsonDocument>.Update.Set("Name", name)
                                                              .Set("UnitCount", UnitCount)
                                                              .Set("Unit", unit)
                                                              .Set("DirektIscilik", DirektIscilik);

                    collection.UpdateOne(filter, update);

                    uCount++;
                }
                else
                {
                    var document = new BsonDocument
                    {
                        { "Code", code },
                        { "Name", name },
                        { "UnitCount", UnitCount },
                        { "Unit", unit },
                        { "DirektIscilik", DirektIscilik }
                    };

                    collection.InsertOne(document);
                    aCount++;
                }

            }

            MessageBox.Show("Added : " + aCount + "\n" + "Updated : " + uCount);
        }



    }
}
