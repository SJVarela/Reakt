﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICrudService<T>
    {
        IEnumerable<T> Get();
        T Get(long id);
        T Create(T enity);
        T Update(T enity);
        void Delete(long id);
    }
}