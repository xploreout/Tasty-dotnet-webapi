using Microsoft.AspNetCore.Mvc;
using RecipeApi.Models;

namespace RecipeApi.Controllers;

public interface IUserRepository
{
    public RecipeResponse GetFavouriteRecipe(int input);
    public bool UserExists(int input);
    public User? GetUser(int input);
    public ICollection<User> GetUsers();
    public Task<User> AddNewUser(UserDto userDto);
    public User UserByEmail(string email);
    public void AddRecipeToUser(User input, string title);
    public List<RecipeResponse> GetFavsByUserId(int userId);
    public int RecipeIdExists(string input);
    public bool RecipeExistsInFav(string title, int userId);
    public Recipe? GetRecipe(int id);
    public Task<RecipeResponse> AddRecipe(RecipeDto recipeDto);
    public Task<bool> DeleteFavourite(int userId, int recipeId);
    public Task<NoteResponse> AddNote(NoteDto noteDto, User user, Recipe recipe);
    public NoteResponse GetNote(int noteId);
    public List<NoteResponse> GetNotes(int userId, int recipeId);
    public Task<bool> DeleteNote(int noteId);
}