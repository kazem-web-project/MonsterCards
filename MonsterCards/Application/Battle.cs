using MonsterCards.Domain.Entities.MTCG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MonsterCards.Infrastructure.Persistance;

namespace MonsterCards.Application
{
    internal class Battle
    {

        public User user1 { get; set; }
        public User user2 { get; set; }
        public List<User> users { get; set; }
        public bool twoPlayerLogged { get; set; } = false;
        public Battle(User user1, User user2)
        {
            this.user1 = user1;
            this.user2 = user2;
            this.users = new List<User>() { user1, user2 };
            twoPlayerLogged = true;
            prepareUserData();

        }
        public Battle(List<User> usersinput)
        {
            this.user1 = usersinput.ElementAt(0);
            this.user2 = usersinput.ElementAt(1);
            this.users = new List<User>() { user1, user2 };
            twoPlayerLogged = true;
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
                // retrieve user information first!
                // User newUser = new User();
                // user.user_id = 1;
                
                stackRepository.retrieveUserCards(user);

            }

        }
        public bool checkUsersLogged() { return twoPlayerLogged; }

        internal void battleStart()
        {
            this.users.ElementAt(0).loadDeckCardsFromStack();
            this.users.ElementAt(1).loadDeckCardsFromStack();
            this.users[0].battle(this.users.ElementAt(0), this.users.ElementAt(1));
        }
    }
}
