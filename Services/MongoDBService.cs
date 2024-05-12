using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Services;

public class MongoDBService
{
    private readonly IMongoCollection<Purchase> PurchaseCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBsettings)
    {
        MongoClient client = new MongoClient(mongoDBsettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBsettings.Value.DatabaseName);
        PurchaseCollection = database.GetCollection<Purchase>(mongoDBsettings.Value.CollectionName);
    }

    public async Task CreateNewPurchase(Purchase obj)
    {
        await PurchaseCollection.InsertOneAsync(obj);
        return;
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesAsync()
    {
        return await PurchaseCollection.Find(new BsonDocument()).ToListAsync();
    }

    // public async Task<List<Username>> GetSpecificUser(string id)
    // {
    //     return await UsernameCollection.Find(x => x.Id == id).ToListAsync();
    // }

    // public async Task Updateasync(string id, Username username)
    // {
    //     await UsernameCollection.ReplaceOneAsync(x => x.Id == id, username);
    // }

    // public async Task DeleteAsync(string id)
    // {
    //     FilterDefinition<Username> filter = Builders<Username>.Filter.Eq("Id", id);
    //     await UsernameCollection.DeleteOneAsync(filter);
    // }

}

