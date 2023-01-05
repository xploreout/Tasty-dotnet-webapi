using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApi.Models;

public class RecipeUser
{
    [Key, Column(Order = 1)]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    
    [Key, Column(Order = 2)]
    public int UserId { get; set; }
    public User User { get; set; }
}