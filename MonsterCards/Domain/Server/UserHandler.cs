using MonsterCards.Application;
using MonsterCards.Domain.Entities.MTCG;
using MonsterCards.Domain.Entities.Server;
using MonsterCards.Domain.Interfaces.Persistance;
using MonsterCards.Domain.Interfaces.Server;
using MonsterCards.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterCards.Domain.Server
{
    internal class UserHandler : IHttpEndpoint
    {
        public bool is_logged { get; set; } = false;
        public bool startConditionBool { get; set; } = false;
        public UserHandler() { }
        public bool HandleRequest(HttpRequest rq, HttpResponse rs, Battle battle)
        {
            if (rq.Path.Length == 3)
            {
                if (rq.Method.Equals(Enums.Server.HttpMethod.GET))
                {
                    PackageRepository packageRepository = new PackageRepository();
                    UserRepository userRepository = new UserRepository();
                    if (rq.Headers.ContainsKey("Authorization"))
                    {
                        string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                        string username = result.Split('-')[0];
                        if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])) && rq.Path[2] == username)
                        {

                            rs.ResponseMessage = userRepository.retrieveUserInfoFromUsernameForUser(username);
                            rs.ResponseCode = 200;
                            return true;
                        }
                        else
                        {
                            rs.ResponseMessage = "You are not allowed to change the information of other users!";
                            rs.ResponseCode = 200;
                            return true;
                        }
                    }
                    rs.ResponseMessage = "Please log in";
                    rs.ResponseCode = 200;
                    return true;
                }
                else if (rq.Method.Equals(Enums.Server.HttpMethod.PUT))
                {
                    if (rq.Headers.ContainsKey("Authorization"))
                    {
                        PackageRepository packageRepository = new PackageRepository();
                        UserRepository userRepository = new UserRepository();
                        string result = retrieveUserSessionFromHeader(rq.Headers["Authorization"]);
                        string username = result.Split('-')[0];
                        if (packageRepository.isUserLogedIn(retrieveUserSessionFromHeader(rq.Headers["Authorization"])) && rq.Path[2] == username)
                        {
                            UserDTO? userData = JsonSerializer.Deserialize<UserDTO>(rq.Content);
                            rs.ResponseMessage = userRepository.editUserInfoFromUsernameForUser(username, userData);
                            rs.ResponseCode = 200;
                            return true;
                        }
                        else
                        {
                            rs.ResponseMessage = "You are not allowed to change the information of other users!";
                            rs.ResponseCode = 200;
                            return true;
                        }
                    }
                    return true;
                }
            }

            CredentialRepository credentialRepository = new CredentialRepository();
            if (rq.Method.Equals(Enums.Server.HttpMethod.POST))
            {

                Credential credential = new Credential();
                // retrieve username
                credential.username = rq.Content.Split(',')[0].Split(':')[1];
                int len_username = credential.username.Length;
                credential.username = credential.username.Substring(1, len_username - 2);

                //  retrieve password
                credential.password = rq.Content.Split(',')[1].Split(':')[1];
                int len_pass = credential.password.Length;
                credential.password = credential.password.Substring(1, len_pass - 3);

                int newUserId = credentialRepository.Add(credential, rs);
                // TODO delete this
                StackRepository stackRepository = new StackRepository();
                stackRepository.initUserStackTable(newUserId);
                // creation of the user:
                rs.ResponseCode = 200;

                rs.Content = "User " + credential.username + " created successfully";
            }
            else if (rq.Method.Equals(Enums.Server.HttpMethod.GET) && rq.Headers.ContainsKey("X-Request-ID") && credentialRepository.is_loged(rq.Headers["X-Request-ID"], rs))
            {
                Credential credential = new Credential();
                credential.username = rq.Path[2];

                credentialRepository.login(credential, rs);
                //this.is_loged(credential,rs);
                rs.ResponseMessage = "The user exists!";
                rs.ResponseCode = 200;
            }
            else if (rq.Method.Equals(Enums.Server.HttpMethod.PUT) && rq.Headers.ContainsKey("X-Request-ID") && credentialRepository.is_loged(rq.Headers["X-Request-ID"], rs))
            {
                Credential credential = new Credential();
                // retrieve username
                credential.username = rq.Content.Split(',')[0].Split(':')[1];
                int len_username = credential.username.Length;
                credential.username = credential.username.Substring(1, len_username - 2);

                //  retrieve password
                credential.password = rq.Content.Split(',')[1].Split(':')[1];
                int len_pass = credential.password.Length;
                credential.password = credential.password.Substring(1, len_pass - 3);

                if (credentialRepository.is_logged || credential.username == "Admin")
                {
                    credentialRepository.Update(credential, rs);
                }
                else
                {
                    rs.ResponseMessage = "Please login to change the user data!";
                    rs.ResponseCode = 411;
                }
            }
            return true;

        }
        public string retrieveUserSessionFromHeader(string headerValue)
        {
            int cnt = headerValue.Length;
            return headerValue.Substring(headerValue.IndexOf(' ') + 1);
        }
    }
}

