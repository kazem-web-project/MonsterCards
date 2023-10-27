using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Router
    {
        internal  void getRoute(string uri, string authorization)
        {
            // if (string.IsNullOrEmpty(authorization)) {  404}
            Console.WriteLine("Get request to: " + uri);            
            if (uri.Contains('?'))
            {
                var parameter= uri.Split("?")[1];
                var parameterKey= parameter.Split("=")[0];
                var parameterValue = parameter.Split("=")[1];  
                
                Console.WriteLine("THERE ARE SOME PARAMETERS: " 
                    + parameter + "\n"+ 
                    parameterKey + ":"
                    + parameterValue);
            }
            Console.WriteLine("Authorization: "+ authorization);
            // implement the request handlers:
        }
        internal  void postRoute(string uri,string authorization,string contentType, StringBuilder bodyStringBuilder)
        {
            Console.WriteLine("string uri,= " + uri + "\nauthorization= " + authorization + ", \ncontentType= " + contentType + ", \nbodyStringBuilder= " + bodyStringBuilder.ToString());

        }
        internal  void putRoute(string uri, string authorization, string contentType, StringBuilder bodyStringBuilder)
        {
            Console.WriteLine("string uri,= " + uri + "\nauthorization= " + authorization + ", \ncontentType= " + contentType + ", \nbodyStringBuilder= " + bodyStringBuilder.ToString());

        }
        internal  void deleteRoute(string uri, string authorization)
        {            
            Console.WriteLine("Delete request to: " + uri);
            Console.WriteLine("Authorization: " + authorization);
            // implement the request handlers:

        }
    }
}
