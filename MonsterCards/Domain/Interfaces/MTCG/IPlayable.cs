using MonsterCards.Domain.Entities.MTCG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Interfaces.MTCG
{
    internal interface IPlayable
    {
        public int coins { get; set; }
        public int stat { get; set; }
        public string name { get; set; }
        public int user_id { get; set; }
        public bool isregistered { get; set; }
        public List<Card> stack { get; set; }
        public List<Card> deck { get; set; }
    }
}
