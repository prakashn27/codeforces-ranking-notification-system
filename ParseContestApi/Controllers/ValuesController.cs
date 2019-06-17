using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Linq;
using ServiceStack;
using ServiceStack.Text;
using ParseContestApi.Models;
// public class GithubRepository
// {
//     public string Name { get; set; }
//     public string Description { get; set; }
//     public string Url { get; set; }
//     public string Homepage { get; set; }
//     public string Language { get; set; }
//     public int Watchers { get; set; }
//     public int Forks { get; set; }

//     public override string ToString() => Name;
// }


public class ChangedRanks {
    public string Status {get; set;}
    public ChangedRank[] Result { get; set;} 
}

namespace ParseContestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class contestController : ControllerBase
    {
        // GET api/contest/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            // var client = new RestClient("https://codeforces.com");
            // var request = new RestRequest("api/contest.ratingChanges?contestId="+id);
            // var response = client.Get(request);
            // Console.WriteLine(response.Content);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var cfRanks = $"https://codeforces.com/api/contest.ratingChanges?contestId={id}"
                .GetJsonFromUrl(httpReq => httpReq.UserAgent = "Gistlyn")
                .FromJson<ChangedRanks>();
                // .OrderByDescending(x => x.Watchers)
                // .Take(5)
                // .ToList();

            // "Top 5 {0} Github Repositories:".Print(cfRanks);
            // cfRanks.PrintDump();

            "first rank {0} ".Print(cfRanks.Result[0]);
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbitmquser", Password = "DEBmbwkSrzy9D1T9cJfa" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Rank",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                    // var body = Encoding.UTF8.GetBytes(message);
                    foreach(ChangedRank cr in cfRanks.Result) {
                        var body = Encoding.UTF8.GetBytes(JsonSerializer.SerializeToString<ChangedRank>(cr));
                        channel.BasicPublish(exchange: "Ranker",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: body);
                        Console.WriteLine(" [x] Sent {0}", cr.Handle);
                    }
                    // channel.BasicPublish(exchange: "Ranker",
                    //                      routingKey: "",
                    //                      basicProperties: null,
                    //                      body: body);
                    // Console.WriteLine(" [x] Sent {0}", message);
                }
            }
            return id;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string id)
        {

        }
    }
}
