using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Models;

public class Recipe
{
    public Recipe()
    {
        Users = new HashSet<User>();
    }
    
    [Key] 
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public string? ImageAltText { get; set; }
    public string? Nutrition { get; set; }
    public int? CookingTime { get; set; }
    public List<Note>? Notes { get; set; }
    public virtual ICollection<User>? Users { get; set; }
    public List<RecipeUser> RecipeUsers { get; set; }

}