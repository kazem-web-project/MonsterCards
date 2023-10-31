using MonsterCards.Domain.Entities.MTCG;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces.MTCG
{
    internal interface ILoginable
    {
        public bool login();
        public Credential UserCredential { get; set; }
    }
}
