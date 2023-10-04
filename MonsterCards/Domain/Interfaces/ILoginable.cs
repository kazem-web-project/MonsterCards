using MonsterCards.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces
{
    internal interface ILoginable
    {
        public bool login();
        public Credential UserCredential { get; set; }
    }
}
