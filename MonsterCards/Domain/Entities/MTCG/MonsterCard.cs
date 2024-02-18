using MonsterCards.Domain.Enums.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Entities.MTCG
{
    [Serializable]
    public class MonsterCard : Card
    {
        public MonsterCard(string name, double damage, ElementType elementType) : base(name, damage, elementType)
        {
        }

        public MonsterCard(string name, double damage, ElementType elementType, string id) : base(id, name, damage, elementType)
        {

        }


        public MonsterCard(string id,string name, double damage) :base(id, name, damage)
        {

        }
        protected MonsterCard()
        {
        }
    }
}
