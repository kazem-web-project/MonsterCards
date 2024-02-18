using MonsterCards.Domain.Enums.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Entities.MTCG
{
    [Serializable]
    public class SpellCard : Card
    {
        public SpellCard(string name, double damage, ElementType elementType) : base(name, damage, elementType)
        {
        }

        public SpellCard(string name, double damage, ElementType elementType, string id) : base(id, name, damage, elementType)
        {
        }


        public SpellCard(string id, string name, double damage) : base(id, name, damage)
        {

        }
        public SpellCard()
        {
        }
    }
}
