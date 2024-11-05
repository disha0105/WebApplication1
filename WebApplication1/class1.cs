using System;
using System.Text.Json;
public class RequiredProjectJSON
{
	public string? Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public string? Status { get; set; }
	public string? Priority { get; set; }
	public Owner? Owner { get; set; }
	public List<string> Tags { get; set; }
	public List<Task1> Tasks { get; set; }
}

public class Owner
{
	public string? Id { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }
}

public class Task1
{
	public string? Id { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public string? Type { get; set; }
	public string? Status { get; set; }
	public string? Priority { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? DueDate { get; set; }
	public List<Assignee>? Assignees { get; set; }
	public List<Comment1>? Comments { get; set; }
	public List<Attachment1>? Attachments { get; set; }
	public Dictionary<string, string>? CustomFields { get; set; }
}

public class Assignee
{
	public string? Id { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }
}

public class Comment1
{
	public string? Id { get; set; }
	public string? Text { get; set; }
	public string? Timestamp { get; set; }
	public Author? Author { get; set; }
}

public class Author
{
	public string? Id { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }
}

public class Attachment1
{
	public string? Id { get; set; }
	public string? FileName { get; set; }
	public string? FileType { get; set; }
	public long? FileSize { get; set; }
	public string? UploadDate { get; set; }
	public string? Url { get; set; }
}
