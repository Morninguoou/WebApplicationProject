using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using marian_onsite.Models;

namespace marian_onsite.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<UserDatabaseSettings> userDatabaseSettings)
        {
            var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(userDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(user => true).ToListAsync();

        public async Task<User?> GetAsync(string id)
        {
            return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> FindOneByEmailAsync(string email) =>
            await _usersCollection.Find<User>(user => user.Email == email).FirstOrDefaultAsync();

        public async Task<User?> FindOneByUsernameAsync(string username) =>
            await _usersCollection.Find<User>(user => user.Username == username).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser)
        {
            await _usersCollection.InsertOneAsync(newUser);
        }

        public async Task UpdateAsync(string id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(user => user.Id == id, updatedUser);
        }

        public async Task DeleteAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(user => user.Id == id);
        }

        public async Task<List<User>> FindFollowersAsync(string userId)
        {
            return await _usersCollection.Find(user => user.Following.Contains(userId)).ToListAsync();
        }

        public async Task<List<User>> FindFollowingAsync(string userId)
        {
            var user = await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
            return await _usersCollection.Find(user => user.Followers.Contains(userId)).ToListAsync();
        }

        public async Task AddFollowerAsync(string userId, string followerId)
        {
            var user = await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
            user.Followers.Add(followerId);
            await _usersCollection.ReplaceOneAsync(user => user.Id == userId, user);
        }

        public async Task AddFollowingAsync(string userId, string followingId)
        {
            var user = await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
            user.Following.Add(followingId);
            await _usersCollection.ReplaceOneAsync(user => user.Id == userId, user);
        }

        public async Task RemoveFollowerAsync(string userId, string followerId)
        {
            var user = await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
            user.Followers.Remove(followerId);
            await _usersCollection.ReplaceOneAsync(user => user.Id == userId, user);
        }

        public async Task RemoveFollowingAsync(string userId, string followingId)
        {
            var user = await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
            user.Following.Remove(followingId);
            await _usersCollection.ReplaceOneAsync(user => user.Id == userId, user);
        }
    }
}
