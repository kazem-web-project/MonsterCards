using MonsterCards.Domain.Enums.Server;
using System.Text;
using HttpMethod = MonsterCards.Domain.Enums.Server.HttpMethod;

namespace MonsterCards.Domain.Entities.Server
{
    public class HttpRequest
    {
        private StreamReader reader;

        public HttpMethod Method { get; set; }= HttpMethod.GET;
        public string[] Path { get; set; } = new string[0];
        public string HttpVersion { get; set; } = "";
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> QueryParams = new Dictionary<string, string>();
        public string? Content { get; private set; }

        public HttpRequest(StreamReader reader)
        {
            this.reader = reader;
        }
        public void Parse()
        {
            string? line = reader.ReadLine();
            Console.WriteLine(line);
            string[]? firstLineParts = line?.Split(' ');
            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), firstLineParts?[0] ?? "GET" );
            string[]? pathAndQuery = firstLineParts?[1].Split('?') ?? new string[0];
            Path = pathAndQuery?[0].Split("/") ?? new string[0];
            if(pathAndQuery.Length>1)
            {
                string[]? queryParams = pathAndQuery?[1].Split('=');
                
                // foreach (var queryParam in queryParams)
                for (int i =0;i< queryParams.Length;i+=2)
                {
                    // string[]? queryParamParts = queryParams.ToString().Split("=");
                    if(queryParams.Length>=1)
                    {

                        QueryParams[queryParams[0]] = (queryParams?.Length == 2) ? queryParams[1] : "";

                    }
                }
            }
            HttpVersion = firstLineParts[2] ?? "";

            int content_length = 0;
            while ((line=reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (line=="")
                {
                    break;
                }

                string[]? parts = line.Split(":");
                if (parts.Length == 2)
                {
                    Headers[parts[0]]  = parts[1].Trim();
                    if (parts[0]== "Content-Length")
                    {
                        content_length = int.Parse(parts[1].Trim());
                    }
                }
            }

            if (content_length>0 && Headers.ContainsKey("Content-Type"))
            {
                var data = new StringBuilder(200);
                char[] chars = new char[1024];
                int bytesReadTotal = 0;
                while (bytesReadTotal<content_length)
                {
                    try
                    {
                        var bytesRead = reader.Read(chars, 0, 1024);
                        bytesReadTotal += bytesRead;
                        if (bytesRead == 0) break;
                        data.Append(chars, 0, bytesRead);
                    }
                    catch(IOException) { break; }// TODO. Implement exception
                }
                Content = data.ToString();
                Console.WriteLine(data.ToString());
            }
        }
        public override string ToString()
        {
            
            string myHeader = ">>>>>>Start of headers:\n";
            foreach (var header in Headers)
            {
                myHeader += header.Key.ToString() + ": ";
                myHeader += header.Value.ToString() +"\n";
            }
            myHeader+= "\n<<<<<End of Headers!\r\n";
            return "--------Mehod: "+ this.Method.ToString() + "\tPaht: " + this.Path[1] + " \nheader :\n" +myHeader+ "\n Content: " +  this.Content + "\n" ;
        } 
    }
}
