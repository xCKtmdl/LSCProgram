using LSC.Models;
using LSCProgram.RESTClient;
using LSCProgram.Services;
using Newtonsoft.Json.Linq;

/*
 * Aaron Gabriel
 * Life Science Project
 * note: I am aware there is no error checking in here.
 * I did the bare minimum to get the results and extra points.
 */

namespace LSC
{
    public class LSCProgram
    {
        private static Client _restClient;
        private static readonly List<List<int>> categoryMagazines = new List<List<int>>();
        public static async Task Main(string[] args)
        {
            Initialization();
            LSCService lscService = new LSCService();
            string token = await lscService.GetToken();
            var categories = await lscService.GetCategories(token);

            // This part is independent. This is where to get the extra points.
            // You can add all these get magazines per category into a Task,
            // and then run them in parallel.
            var getMagazinesTask = new List<Task>();
            foreach (string category in categories)
            {
                getMagazinesTask.Add(Task.Run(async () =>
                {
                    var magazines = await lscService.GetMagazines(token, category);
                    categoryMagazines.Add(magazines);
                }));
            }
            await Task.WhenAll(getMagazinesTask);

            List<string> subscribedToAllCategories = new List<string>();
            var subscribers = await lscService.GetSubscribers(token);

            foreach (Subscriber subscriber in subscribers)
            {
                bool isSubscribedToAllCategories = true;

                foreach (var magazines in categoryMagazines)
                {
                    if (!magazines.Any(x => subscriber.MagazineIds.Contains(x)))
                    {
                        isSubscribedToAllCategories = false;
                        break;
                    }
                }

                if (isSubscribedToAllCategories)
                {
                    subscribedToAllCategories.Add(subscriber.Id);
                }
            }

            var result = await lscService.PostResult(token, subscribedToAllCategories);
        }

        public static void Initialization()
        {
            JObject appSettings = JObject.Parse(File.ReadAllText("appsettings.json"));
            _restClient = new Client(appSettings["APIUrl"].ToString());
        }
    }
}


