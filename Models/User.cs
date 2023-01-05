using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Models;

public class User
{
    public User()
    {
        Recipes = new HashSet<Recipe>();
    }
    
    [Key] 
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public List<Note>? Notes { get; set; }
    public virtual ICollection<Recipe>? Recipes { get; set; }
    public List<RecipeUser> RecipeUsers { get; set; }
}