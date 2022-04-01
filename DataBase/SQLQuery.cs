using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace AccounterLite.DataBase
{
    internal class SQLQuery
    {
        SubQ.Rawmats sqRawmats;
        SubQ.UnitCosts sqUnitCosts;
        SubQ.Receipts sqReceipts;
        SubQ.Costs sqCosts;
        SubQ.Xrates sqXrates;

        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        void mongoDbConn()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            database = dbClient.GetDatabase("Accounter_Lite");
        }

        public SQLQuery()
        {
            if (dbClient == null) mongoDbConn();
        }

        public void updateXr(double Eur, double Usd, string Date)
        {
            if (sqXrates == null)
                sqXrates = new();

            sqXrates.UpdateXr(Eur, Usd, Date);
        }
        public List<BsonDocument> getCollection(string table)
        {
            collection = database.GetCollection<BsonDocument>(table);

            return collection.Find(new BsonDocument()).ToList();
        }
        public BsonDocument selectMDataByCode(string table, object code)
        {
            collection = database.GetCollection<BsonDocument>(table);
            var filter = Builders<BsonDocument>.Filter.Eq("Code", code);

            return collection.Find(filter).FirstOrDefault();
        }

        public BsonDocument selectMDataByType(string table, object type)
        {
            collection = database.GetCollection<BsonDocument>(table);
            var filter = Builders<BsonDocument>.Filter.Eq("Type", type);

            return collection.Find(filter).FirstOrDefault();
        }
        public DataTable getMData(string table)
        {
            if (table == "Rawmats")
            {
                if (sqRawmats == null)
                    sqRawmats = new();

                return sqRawmats.getData();
            }

            if (table == "UnitCosts")
            {
                if (sqUnitCosts == null)
                    sqUnitCosts = new();

                return sqUnitCosts.getData();
            }
            if (table == "Costs")
            {
                if (sqCosts == null)
                    sqCosts = new();

                return sqCosts.getData();
            }
            if (table == "XRates")
            {
                if (sqXrates == null)
                    sqXrates = new();

                return sqXrates.getData();
            }
            if (table == "EReceipts")
            {
                if (sqReceipts == null)
                    sqReceipts = new();

                return sqReceipts.getData("ERECEIPTS");
            }
            if (table == "DReceipts")
            {
                if (sqReceipts == null)
                    sqReceipts = new();

                return sqReceipts.getData("DRECEIPTS");
            }
            return null;

        }

        public void ImportXtoDb(string type, DataGridView dgv)
        {
            if (type != "null")
            {
                if (type == "RAWMATS")
                {
                    if (sqRawmats == null) sqRawmats = new();
                    sqRawmats.ImportXtoDb(dgv);
                }
                if (type == "UNITCOSTS")
                {
                    if (sqUnitCosts == null) sqUnitCosts = new();
                    sqUnitCosts.ImportXtoDb(dgv);
                }
                if (type == "ERECEIPTS" || type == "DRECEIPTS")
                {
                    if (sqReceipts == null) sqReceipts = new();
                    sqReceipts.ImportXtoDb(type, dgv);
                }
            }
        }

        public void UpdateCosts(string type, decimal[] costsData)
        {
            if (sqCosts == null)
                sqCosts = new();

            sqCosts.updateCosts(type, costsData);

        }


    }
}
