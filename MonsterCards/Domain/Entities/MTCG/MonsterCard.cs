using MonsterCards.Domain.Enums.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Entities.MTCG
{
    internal class MonsterCard : Card
    {
        public MonsterCard(string name, int damage, ElementType elementType) : base(name, damage, elementType)
        {
        }
    }
}
