using MonsterCards.Domain.Entities.MTCG;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces.Persistance
{
    internal interface IUserRepository
    {
        bool Add(User user);
    }
}
