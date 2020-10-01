﻿using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudService<Comment>
    {
        void Like(long id);
    }
}
