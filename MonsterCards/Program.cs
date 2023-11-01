using MonsterCards.Domain.Entities;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Enums;
using MonsterCards.Infrastructure.Persistance;
namespace MonsterCards.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Application.BusinessLogic.playGame();
            //MonplayGame();



            CredentialRepository repoCred = new CredentialRepository("Host=localhost;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True");
            repoCred.Add(new Domain.Entities.MTCG.Credential() { Username = "user1" ,Password = "password" });
            Credential c2 = new Credential() { Username = "user1", Password = "password" };
            
            Console.WriteLine(repoCred.login(c2));
        }    
    }
}