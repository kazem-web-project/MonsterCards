using MonsterCards.Domain.Entities.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces.MTCG
{
    internal interface IRegistrable
    {
        public bool register(Credential credential);
    }
}
