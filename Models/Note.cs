using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Models;

public class Note
{
    [Key] public int Id { get; set; }
    public string NoteText { get; set; }
    public Recipe Recipe { get; set; }
    public User User { get; set; }
    public DateTime CreateDate => DateTime.Now;
};