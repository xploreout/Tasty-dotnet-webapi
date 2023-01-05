namespace RecipeApi.Models;

public class RecipeResponse
{
    public string Title { get; set; }
    public string[] Ingredients { get; set; }
    public string[] Instructions { get; set; }
    public int RecipeId { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public int? CookingTime { get; set; }
    public string? Nutrition { get; set; }
    public int? UserId { get; set; }
}
