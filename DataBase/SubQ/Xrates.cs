using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace AccounterLite.DataBase.SubQ
{
    internal class Xrates
    {
        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;


        public Xrates()
        {
            if (dbClient == null) mongoDBConn();
        }


        void mongoDBConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            database = dbClient.GetDatabase("Acc_Lite");

            collection = database.GetCollection<BsonDocument>("XRates");
        }

        double gd(object var) { return double.Parse(var.ToString()); }
        string sF(object var) { return String.Format("{0:0.0000}", gd(var)); }
        public void UpdateXr(double Eur, double Usd, string Date)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("id", 1);
            bool doesExist = collection.Find(filter).FirstOrDefault() != null;

            if (doesExist)
            {
                var update = Builders<BsonDocument>.Update.Set("Eur", sF(Eur))
                                                          .Set("Usd", sF(Usd))
                                                          .Set("Date", Date);

                collection.UpdateOne(filter, update);
            }
            else
            {
                var document = new BsonDocument
                {
                    { "id" , 1 },
                    { "Eur" , sF(Eur) },
                    { "Usd" , sF(Usd) },
                    { "Date", Date },
                };
                collection.InsertOne(document);
            }

        }


        public DataTable getData()
        {
            DataTable dt = new();

            dt.Columns.Add("Usd");
            dt.Columns.Add("Eur");
            dt.Columns.Add("Date");

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var item in documents)
            {
                dt.Rows.Add(item["Eur"], item["Usd"], item["Date"]);
            }

            return dt;
        }
    }
}
