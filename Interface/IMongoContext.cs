﻿using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace inventoryApiDotnet.Interface
{
    public interface IMongoContext : IDisposable
    {
        void AddCommand(Func<Task> func);
        Task<int> SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}