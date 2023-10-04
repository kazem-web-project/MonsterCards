using MonsterCards.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Entities
{
    internal class SpellCard : Card
    {
        public SpellCard(string name, int damage, ElementType elementType) : base(name, damage, elementType)
        {
        }
    }
}
