using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Trello_PMS_ConsoleAPP;
using ConsoleApp45;

namespace ConsoleApp45
{
	public class Program
	{
		private static readonly string apiKey = "dcaaf3d11fa71ea7d5233ae6667c9d39";
		private static readonly string token = "ATTA7ad970655f68f5a1ab0c242309f895dd8768599caf3868bc9745c60d8a1e3543F0B0B0DA";

		public  static async Task TaskAsync(string[] args)
		{
			var boardId = "6716146a7bc2754fcaea2686"; 

			Board board = await FetchBoard(boardId);
			List<Card> cards = await FetchCards(boardId);
			List<Member> members = await FetchMembers(boardId);

			Console.WriteLine("Cards \n");
			foreach(var i in cards)
			{
				Console.WriteLine(i.idBoard);
			}

			var options = new JsonSerializerOptions
			{
				WriteIndented = true 
			};

			
			string jsonString = JsonSerializer.Serialize(members, options);

			Console.WriteLine(jsonString);

			RequiredProjectJSON projectResponse = new RequiredProjectJSON{
                id = board.id,
                description = board.desc,
                endDate = board.endDate.ToString(),
                startDate = board.startDate.ToString(),
                name = board.name,
                owner = board.Owner,
                priority = board.priority,
                status = board.status,
                tags = board.Tags,
                tasks = cards,

            };

			string jsonResponse = JsonSerializer.Serialize(projectResponse, new JsonSerializerOptions { WriteIndented = true });
			Console.WriteLine(jsonResponse);
		}

		private static async Task<Board> FetchBoard(string boardId)
		{
			using var httpClient = new HttpClient();
			var response = await httpClient.GetStringAsync($"https://api.trello.com/1/boards/{boardId}?key={apiKey}&token={token}");
			return JsonSerializer.Deserialize<Board>(response);
		}

		private static async Task<List<Card>> FetchCards(string boardId)
		{
		    using var httpClient = new HttpClient();
			var response = await httpClient.GetStringAsync($"https://api.trello.com/1/boards/{boardId}/cards?key={apiKey}&token={token}");
			return JsonSerializer.Deserialize<List<Card>>(response);
		}

		private static async Task<List<Member>> FetchMembers(string boardId)
		{
            using var httpClient = new HttpClient();
			var response = await httpClient.GetStringAsync($"https://api.trello.com/1/boards/{boardId}/members?key={apiKey}&token={token}");
			return JsonSerializer.Deserialize<List<Member>>(response);
		}

	}

	public class Board
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? desc { get; set; }
		public bool? closed { get; set; }
        public string? startDate {get; set;}
        public string? endDate {get; set;}
        public string? status {get; set;}
        public string? priority {get; set;}
        public Member? Owner {get; set;}
        public List<string>? Tags {get; set;}
        public List<Dictionary<string, string>>? CustomFields {get; set;}
        public Task1? Tasks {get; set;}
	}

	public class Member
	{
		public string? id { get; set; }
		public string? fullName { get; set; }
		public string? email { get; set; }
		
	}
	public class CommentData
	{
		public List<Comment>? Comments { get; set; }
	}

	public class Comment
	{
		public string? id { get; set; }
		public string? idMemberCreator { get; set; }
		public Data? data { get; set; }
		public object? appCreator { get; set; }
		public string? type { get; set; }
		public DateTime? date { get; set; }
		public Limits? limits { get; set; }
		public Member? memberCreator { get; set; }
	}
	public class Limits
	{
		public Reactions? reactions { get; set; }
	}

	public class Reactions
	{
	}

	public class Data
	{
		public string? text { get; set; }
		public TextData? textData { get; set; }
		public Card? card { get; set; }
		public Board? board { get; set; }
		//public List list { get; set; }
	}
	public class TextData
	{
		public object? emoji { get; set; }
	}
	public class Attachment
	{
		public string? id { get; set; }
		public long? bytes { get; set; }
		public DateTime? date { get; set; }
		public string? edgeColor { get; set; }
		public string? idMember { get; set; }
		public bool? isUpload { get; set; }
		public string? mimeType { get; set; }
		public string? name { get; set; }
		public List<Preview>? previews { get; set; }
		public string? url { get; set; }
		public int? pos { get; set; }
		public string? fileName { get; set; }
	}

    
    	public class RequiredProjectJSON
	{
		public string? id { get; set; }
		public string? name { get; set; }
		public string? description { get; set; }
		public string? startDate { get; set; }
		public string? endDate { get; set; }
		public string? status { get; set; }
		public string? priority { get; set; }
		public Member? owner { get; set; }
		public List<string>? tags { get; set; }
		public List<Card>? tasks { get; set; }
	}
	public class Preview
	{
		public string? id { get; set; }
		public string? _id { get; set; }
		public bool? scaled { get; set; }
		public string? url { get; set; }
		public long? bytes { get; set; }
		public int? height { get; set; }
		public int? width { get; set; }
	}
	public class Card
	{
		public string? id { get; set; }
		public DateTime? dateLastActivity { get; set; }
		public string? desc { get; set; }
		public object? descData { get; set; }
		public DateTime? due { get; set; }
		public object? email { get; set; }
		public string? idBoard { get; set; }
		public List<string>? idChecklists { get; set; }
		public object? idAttachmentCover { get; set; }
		public string? name { get; set; }
		public string? shortLink { get; set; }
		public string? shortUrl { get; set; }
		public DateTime? start { get; set; }
		public Cover? cover { get; set; }
	}
	public class Cover
	{
		public object? idAttachment { get; set; }
		public string? color { get; set; }
		public object? idUploadedBackground { get; set; }
		public string? size { get; set; }
		public string? brightness { get; set; }
		public object? idPlugin { get; set; }
	}
	public class CustomField
	{
		public string? id { get; set; }
		public Display? display { get; set; }
		public List<Option>? options { get; set; }
	}
	public class Display
	{
		public bool? cardFront { get; set; }
	}

	public class Option
	{
		public string? id { get; set; }
		public string? idCustomField { get; set; }
		public Value? value { get; set; }
		public string? color { get; set; }
		public int? pos { get; set; }
	}
	public class Value
	{
		public string? text { get; set; }
	}
	public class Subtask
	{
		public string? id { get; set; }                     
		public string? title { get; set; }                  
		public string? status { get; set; }                 
		public List<Assignee>? assignees { get; set; }     
	}
	public class Assignee
	{
		public string? id { get; set; }                     
		public string? name { get; set; }                   
		public string? email { get; set; }                  
	}
}
