﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
