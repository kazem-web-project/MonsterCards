using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Interfaces.Server;
using MonsterCards.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Server
{
    internal class SessionHandler : IHttpEndpoint
    {

        public SessionHandler() { }
        static readonly object _lock = new object();

        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {
            CredentialRepository credentialRepository = new CredentialRepository();

            Credential credential = new Credential();
            // retrieve username
            credential.username = rq.Content.Split(',')[0].Split(':')[1];
            int len_username = credential.username.Length;
            credential.username = credential.username.Substring(1, len_username - 2);

            //  retrieve password
            credential.password = rq.Content.Split(',')[1].Split(':')[1];
            int len_pass = credential.password.Length;
            credential.password = credential.password.Substring(1, len_pass - 3);
            //this.is_loged(credential,rs); 
            // to set is_active to false

            //credentialRepository.disableNotUsedSessions();
            // if (credentialRepository.hasUserSession_is(credential, rs))
            if (credentialRepository.login(credential, rs))
            {
                // credentialRepository.setIsActiveTrue(credential, rs);
                credentialRepository.createStoreSession(credential, rs);
                UserRepository userRepository = new UserRepository();
                // critical sections
                User user = userRepository.retrieveUserInfoFromUsername(credential.username);
                lock (_lock)
                {
                    if (!battle.users.Contains(user) && user.name != "admin")
                    {
                        battle.users.Add(user);
                        if (battle.users.Count == 2) { battle.startGame = true; }

                    }                    
                }
                return true;

            }
            // working here!
            // credentialRepository.retrieveMaxBattleId();
            // credentialRepository.is_there_two_user_with_max_battle_id();
            /*
            if (credentialRepository.startConditionBool && credentialRepository.lastBattleId != 0)
            {
                List<User> users = credentialRepository.retriveLoggedUsers(rs);
                Console.WriteLine("BATTLE BEGINS..............................................");
                Battle newBattle = new Battle(users);

                
                User user1 = new User();
                user1.name = "altenhof";
                User user2 = new User();
                user2.name = "kienboec";
                
                List<User> usersToPlay = credentialRepository.retriveLoggedUsers(rs);
                Battle battle = new Battle(usersToPlay);
                battle.perpareUser();
                battle.battleStart(rs);

            }
        */
            return false;
        }
    }
}
