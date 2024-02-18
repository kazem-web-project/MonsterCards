using MonsterCards.Domain.Entities.MTCG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MonsterCards.Infrastructure.Persistance;
using MonsterCards.Domain.Entities.Server;

namespace MonsterCards.Application
{
    public class Battle
    {

        public List<User> users { get; set; }
        public bool startGame { get; set; } = false;

        public Battle()
        {
            this.users = new List<User>() ;
        }

        public Battle(User user1, User user2)
        {
            this.users = new List<User>() { user1, user2 };
            startGame = true;
            prepareUserData();

        }
        public Battle(List<User> usersinput)
        {
            this.users =  usersinput ;
            startGame = true;
            prepareUserData();
        }
        public void prepareUserData()
        {
            UserRepository userRepository = new UserRepository();
            foreach (var user in users)
            {
                userRepository.retrieveUserInfo(user);
            }
        }
        public void perpareUser()
        {
            StackRepository stackRepository = new StackRepository();
            foreach(var user in users)
            {
                stackRepository.retrieveUserCards(user);
            }

        }
        public bool checkStartGame() { return startGame; }

        public string retrieveScoreBoard()
        {
            string resutl = "********************Scoreboard:******************** ";
            if (this.users.Count != 0)
            {
                for (int i= 0; i < this.users.Count; i++)
                {                    
                    resutl += this.users.ElementAt(i).ToString();
                    if(i!= this.users.Count -1) resutl += "---------------------The opponent is:---------------------";
                }
            }
            return resutl;
        }

        public string  battleStart()
        {
            this.users.ElementAt(0).loadDeckCardsFromStack();
            this.users.ElementAt(1).loadDeckCardsFromStack();
            return this.users[0].battle(this.users.ElementAt(0), this.users.ElementAt(1));
        }

        internal string retrieveStats()
        {
            string resutl = "********************Stats:******************** ";
            if (this.users.Count != 0)
            {
                foreach (var item in this.users)
                {
                    resutl += "The user: " + item.name + " has stat ";
                    resutl += item.stat + "****";                   
                }
            }
            return resutl;
        }
    }
}
