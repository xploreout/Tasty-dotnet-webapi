namespace RecipeApi.Models;

public class NoteResponse
{
    public string NoteText { get; set; }
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public DateTime Date { get; set; }
    public int NoteId { get; set; }
}