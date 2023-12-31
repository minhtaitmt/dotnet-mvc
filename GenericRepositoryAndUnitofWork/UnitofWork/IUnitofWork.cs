﻿using GenericRepositoryAndUnitofWork.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryAndUnitofWork.UnitofWork
{
    public interface IUnitofWork
    {
        IBookRepository BookRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IUserRepository UserRepository { get; }
        IAuthRepository AuthRepository { get; }
        void SaveChanges();
        IDbContextTransaction BeginTransaction();
    }
}
