namespace RecipeApi.Models;

public class NoteDto
{
    public string NoteText { get; set; }
    public int UserId { get; set; }
    public int RecipeId { get; set; }
}