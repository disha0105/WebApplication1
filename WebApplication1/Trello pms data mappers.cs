using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp45;
using Trello_PMS_ConsoleAPP;

namespace Trello_PMS_ConsoleAPP
{
	internal class Program
	{
		private static readonly HttpClient httpClient = new HttpClient();

		private static async Task Main(string[] args)
		{
			
			string apiKey = "dcaaf3d11fa71ea7d5233ae6667c9d39"; 
			string apiToken = "ATTA7ad970655f68f5a1ab0c242309f895dd8768599caf3868bc9745c60d8a1e3543F0B0B0DA"; 
			string baseUrl = "https://api.trello.com/1/";

			string boardId = await SelectBoardAsync(baseUrl, apiKey, apiToken);

			var projectJSON = await MapBoardDataToPMSAsync(baseUrl, boardId, apiKey, apiToken);
			string jsonString = System.Text.Json.JsonSerializer.Serialize(projectJSON, new JsonSerializerOptions { WriteIndented = true });

		}

		private static async Task<string> SelectBoardAsync(string baseUrl, string apiKey, string apiToken)
		{
			var response = await httpClient.GetAsync($"{baseUrl}members/me/boards?key={apiKey}&token={apiToken}");
            var res = await response.Content.ReadAsStringAsync();
			var boards = JsonConvert.DeserializeObject<List<TrelloBoard>>(res);
			// var boards = JsonConvert.DeserializeObject<object>(baseUrl);
			Console.WriteLine(boards);
			Console.WriteLine("\t\t\t\t\t WELCOME TO TRELLO - PMS DATA MIGRATION\n");
			Console.WriteLine("-> List of Boards present in your Trello Account\n");

			foreach (var board in boards)
			{
				Console.WriteLine($"\t\tId = {board.id} | Board Name - {board.name}");
			}


			while (true)
			{
				string boardId = Console.ReadLine();
				if (boards.Exists(b => b.id == boardId))
				{
					Console.WriteLine("-> Board Selected Successfully");
					return boardId;
				}
				Console.WriteLine("-> Please Enter a Valid Board Id");
				if(boardId == "exit")
					return null;
			}
		}

		public static async Task<RequiredProjectJSON> MapBoardDataToPMSAsync(string baseUrl, string boardId, string apiKey, string apiToken)
		{
            try{
                
                var response = await httpClient.GetAsync($"{baseUrl}boards/{boardId}/cards?filter=all&attachments=true&comments=true&members=true&customFieldItems=true&key={apiKey}&token={apiToken}");
                var res = await response.Content.ReadAsStringAsync();
                var cards = JsonConvert.DeserializeObject<List<TrelloCard>>(res);
                var rawcards = JsonConvert.DeserializeObject<List<object>>(res);
                
var settings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
};

var response4 = JsonConvert.SerializeObject(cards, settings);


                System.Console.WriteLine("Board data");
               Console.WriteLine( response4);
                // foreach(var a in rawcards)
                //    System.Console.WriteLine( a);

                var projectJSON = new RequiredProjectJSON();

                foreach (var card in cards)
                {
                    var task = new Task1
                    {
                        id = card.id,
                        title = card.name,
                        description = card.desc,
                        type = "Task",
                        status = card.due.HasValue ? "Pending" : "Completed",
                        priority = "NA" ,
                        startDate = DateTime.Now,
                        dueDate = card.due ?? DateTime.Now.AddDays(7),
                        assignees = MapAssignees(card.members),
                        comments = MapComments(card.comments),
                        attachments = MapAttachments(card.attachments),
                        customFields = MapCustomFields(card.customFieldItems)
                    };

                    projectJSON.tasks.Add(task);
                }

                return projectJSON;
            }
            catch{
                return null;
            }

		}

		private static List<Assignee> MapAssignees(List<TrelloMember> members) =>
			members.ConvertAll(member => new Assignee
			{
				id = member.id,
				name = member.fullName,
				email = member.email 
			});

		private static List<Comment1> MapComments(List<TrelloComment> comments) =>
			comments.ConvertAll(comment => new Comment1
			{
				id = comment.id.ToString(),
				text = comment.body,
				timestamp = comment.date.ToString(),
				author = new Author
				{
					id = comment.memberAuthor.id,
					name = comment.memberAuthor.fullName,
					email = comment.memberAuthor.email 
				}
			});

		private static List<Attachment1> MapAttachments(List<TrelloAttachment> attachments) =>
			attachments.ConvertAll(attachment => new Attachment1
			{
				id = attachment.id.ToString(),
				fileName = attachment.name,
				fileType = attachment.mimeType,
				uploadDate = attachment.date.ToString(),
				url = attachment.url
			});

		private static Dictionary<string, string> MapCustomFields(List<TrelloCustomFieldItem> customFieldItems)
		{
			var customFields = new Dictionary<string, string>();
			foreach (var item in customFieldItems)
			{
				customFields[item.id] = item.value; 
			}
			return customFields;
		}
	}

		public class TrelloBoard
	{
		public string? id { get; set; }
		public string? name { get; set; }
	}

	public class TrelloCard
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? desc { get; set; }
		public DateTime? due { get; set; }
		public List<TrelloMember> members { get; set; }
		public List<TrelloComment> comments { get; set; }
        public string Priority {get; set;}
		public List<TrelloAttachment> attachments { get; set; }
		public List<TrelloCustomFieldItem> customFieldItems { get; set; }
	}

	public class TrelloMember
	{
		public string? id { get; set; }
		public string? fullName { get; set; }
		public string? email { get; set; }
	}

	public class TrelloComment
	{
		public string? id { get; set; }
		public string? body { get; set; }
		public DateTime? date { get; set; }
		public TrelloMember? memberAuthor { get; set; }
	}

	public class TrelloAttachment
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? mimeType { get; set; }
		public long? Bytes { get; set; }
		public DateTime? date { get; set; }
		public string? url { get; set; }
	}

	public class TrelloCustomFieldItem
	{
		public string? id { get; set; }
		public string? value { get; set; } 
	}

	public class RequiredProjectJSON
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? description { get; set; }
		public DateTime? startDate { get; set; }
		public DateTime? endDate { get; set; }
		public string? status { get; set; }
		public string? priority { get; set; }
		public Owner? owner { get; set; }
		public List<string>? tags { get; set; }
		public List<Task1>? tasks { get; set; }
	}

	public class Owner
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? email { get; set; }
	}

	public class Task1
	{
		public string? id { get; set; }
		public string? title { get; set; }
		public string? description { get; set; }
		public string? type { get; set; }
		public string? status { get; set; }
		public string? priority { get; set; }
		public DateTime? startDate { get; set; }
		public DateTime? dueDate { get; set; }
		public List<Assignee>? assignees { get; set; }
		public List<Comment1>? comments { get; set; }
		public List<Attachment1>? attachments { get; set; }
		public Dictionary<string, string>? customFields { get; set; }
	}

	public class Assignee
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? email { get; set; }
	}

	public class Comment1
	{
		public string? id { get; set; }
		public string? text { get; set; }
		public string? timestamp { get; set; }
		public Author? author { get; set; }
	}

	public class Author
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? email { get; set; }
	}

	public class Attachment1
	{
		public string? id { get; set; }
		public string? fileName { get; set; }
		public string? fileType { get; set; }
		public long? fileSize { get; set; }
		public string? uploadDate { get; set; }
		public string? url { get; set; }
	}
}

