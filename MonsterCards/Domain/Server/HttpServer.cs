using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Interfaces.Server;
using MonsterCards.Domain.Server;
using MonsterCards.Infrastructure.Persistance;
using System.Net;
using System.Net.Sockets;
using System.Transactions;


namespace MonsterCards.Domain.Entities.Server
{
    public class HttpServer
    {
        private readonly int port = 10001;
        private readonly IPAddress ip = IPAddress.Loopback;
        private TcpListener tcpListener;

        public Dictionary<string, IHttpEndpoint> Endpoints { get; private set; } = new Dictionary<string, IHttpEndpoint>();
        public HttpServer(IPAddress ip, int port) {
            this.port = port;
            this.ip = ip;

            tcpListener = new TcpListener(ip, port);
            //RegisterEndpoint("users", new CredentialRepository());
            RegisterEndpoint("users", new UserHandler());

            //RegisterEndpoint("sessions", new CredentialRepository());
            RegisterEndpoint("sessions", new SessionHandler());
            RegisterEndpoint("packages", new PackageHandler());
            RegisterEndpoint("transactions", new TransactionsHandler());
            RegisterEndpoint("cards", new CardHandler());
            RegisterEndpoint("deck", new DeckHandler());
            RegisterEndpoint("stats", new StatHandler());
            RegisterEndpoint("scoreboard", new ScoreboardHandler());
            RegisterEndpoint("battles", new BattlesHandler());
        }


        /*
private void RegisterEndpoint(string v, UserHandler userHandler)
{
   throw new NotImplementedException();
}
*/
        public void Run()
        {
            /* 
            StackRepository stackRepository = new StackRepository();        // UNCOMMENT
            stackRepository.initStackTable();                              // UNCOMMENT
            UserRepository userRepository = new UserRepository();
            User dummy = new User();
            dummy.user_id = 1;
            userRepository.fetchStackCards(dummy);
            User dummy = new User();
            dummy.user_id = 1;
            StackRepository stackRepository = new StackRepository();
            stackRepository.retrieveUserCards(dummy);
            User user1 = new User();
            user1.name = "altenhof";
            User user2 = new User();
            user2.name = "kienboec";
            List<User> users = new List<User>() { user1,user2};
            Battle battle = new Battle(users);
            battle.perpareUser();
            battle.battleStart();
            */


            tcpListener.Start();
            Battle battle = new Battle();
            while (true)
            {
                var clientSocket = tcpListener.AcceptTcpClient();
                var httpProcessor = new HttpProcessor(this, clientSocket);
                Thread newThread = new Thread(() => httpProcessor.Process(battle));
                newThread.Start();
            }
        }

        public void RegisterEndpoint(string path, IHttpEndpoint endpoint)
        {
            Endpoints.Add(path, endpoint);
        }


// / battle is the battles request


    }
}
