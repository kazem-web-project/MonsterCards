using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Entities.MTCG
{
    internal class Deck
    {
        public List<Card> cardsDeck { get; set; } = new List<Card>();
    }
}
