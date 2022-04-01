using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace AccounterLite.DataBase.SubQ
{
    internal class Costs
    {
        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        void mongoDBConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");

            database = dbClient.GetDatabase("Acc_Lite");
            collection = database.GetCollection<BsonDocument>("Costs");
        }
        void checkTable()
        {
            string[] rowTypes = new string[2] { "Domestic", "Export" };

            foreach (string s in rowTypes)
            {

                var filter = Builders<BsonDocument>.Filter.Eq("Type", s);
                bool doesExist = collection.Find(filter).FirstOrDefault() != null;
                if (!doesExist)
                {
                    var document = new BsonDocument
                    {
                         { "Marj", 0 },
                         { "EndirektIscilik", 0 },
                         { "GenelUretimGider", 0 },
                         { "PazarlamaSatisGider", 0 },
                         { "SatisIscilik", 0 },
                         { "GenelYonetimGider", 0 },
                         { "IdariIscilik", 0 },
                         { "Type", s}
                     };

                    collection.InsertOne(document);
                }
            }

        }

        public Costs()
        {
            if (dbClient == null) mongoDBConn();
            checkTable();
        }

        public DataTable getData()
        {
            DataTable dt = new();


            dt.Columns.Add("Marj");
            dt.Columns.Add("EndirektIscilik");
            dt.Columns.Add("GenelUretimGider");
            dt.Columns.Add("PazarlamaSatisGider");
            dt.Columns.Add("SatisIscilik");
            dt.Columns.Add("GenelYonetimGider");
            dt.Columns.Add("IdariIscilik");

            var filter = Builders<BsonDocument>.Filter.Eq("Type", "Domestic");
            var domestic = collection.Find(filter).FirstOrDefault();

            dt.Rows.Add(domestic["Marj"], domestic["EndirektIscilik"], domestic["GenelUretimGider"], domestic["PazarlamaSatisGider"],
                        domestic["SatisIscilik"], domestic["GenelYonetimGider"], domestic["IdariIscilik"]);


            filter = Builders<BsonDocument>.Filter.Eq("Type", "Export");
            var export = collection.Find(filter).FirstOrDefault();

            dt.Rows.Add(export["Marj"], export["EndirektIscilik"], export["GenelUretimGider"], export["PazarlamaSatisGider"],
                        export["SatisIscilik"], export["GenelYonetimGider"], export["IdariIscilik"]);


            return dt;

        }

        public void updateCosts(string type, decimal[] costsData)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("Type", type);

            var update = Builders<BsonDocument>.Update.Set("Marj", costsData[0])
                                                      .Set("EndirektIscilik", costsData[1])
                                                      .Set("GenelUretimGider", costsData[2])
                                                      .Set("PazarlamaSatisGider", costsData[3])
                                                      .Set("SatisIscilik", costsData[4])
                                                      .Set("GenelYonetimGider", costsData[5])
                                                      .Set("IdariIscilik", costsData[6]);

            collection.UpdateOne(filter, update);

        }
    }
}
