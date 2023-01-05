using Microsoft.Data.SqlClient;
using RecipeApi.Data;
using RecipeApi.Models;

namespace RecipeApi.Controllers;

public class UserRepository : IUserRepository
{
    private RecipeContext _context;

    public UserRepository(RecipeContext context)
    {
        _context = context;
    }

    public RecipeResponse GetFavouriteRecipe(int recipeId)
    {
        var recipeInDb = _context.Recipe.FirstOrDefault(r => r.Id == recipeId);

        if (recipeInDb is null)
        {
            return new RecipeResponse();
        }

        return new RecipeResponse
        {
            Title = recipeInDb.Title,
            Ingredients = CreateArray(recipeInDb.Ingredients),
            Instructions = CreateArray(recipeInDb.Instructions),
            ImageUrl = recipeInDb.ImageUrl,
            ImageAltText = recipeInDb.ImageAltText,
            CookingTime = recipeInDb.CookingTime,
            Nutrition = recipeInDb.Nutrition,
            RecipeId = recipeInDb.Id
        };
    }

    public List<RecipeResponse> GetFavsByUserId(int userId)
    {
        var favourites = from r in _context.RecipeUser
            where r.UserId == userId
            select r.RecipeId;
        var result = new List<RecipeResponse>();
        foreach (var i in favourites.ToList())
        {
            var resultRecipe = _context.Recipe.Find(i);
            result.Add(new RecipeResponse
            {
                Title = resultRecipe.Title,
                ImageUrl = resultRecipe.ImageUrl,
                Ingredients = CreateArray(resultRecipe.Ingredients),
                Instructions = CreateArray(resultRecipe.Instructions),
                ImageAltText = resultRecipe.ImageAltText,
                Nutrition = resultRecipe.Nutrition,
                CookingTime = resultRecipe.CookingTime,
                RecipeId = resultRecipe.Id,
                UserId = userId
            });
        }

        return result;
    }

    public async void AddRecipeToUser(User user, string title)
    {
        var recipe = await _context.Recipe.FindAsync(RecipeIdExists(title));
        recipe.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public bool RecipeExistsInFav(string title, int userId)
    {
        var recipeInDb = _context.Recipe.FirstOrDefault(r => r.Title == title);
        var recipeAlreadyFav = from ru in _context.RecipeUser
            where ru.UserId == userId && recipeInDb != null && recipeInDb.Id == ru.RecipeId
            select ru;
        return recipeAlreadyFav.ToList().Count > 0;
    }

    public async Task<RecipeResponse> AddRecipe(RecipeDto recipeDto)
    {
        var userToAdd = GetUser(recipeDto.UserId);
        var resultRecipe = new Recipe
        {
            Title = recipeDto.Title,
            ImageUrl = recipeDto.ImageUrl,
            Ingredients = FlattenArray(recipeDto.Ingredients),
            Instructions = FlattenArray(recipeDto.Instructions),
            ImageAltText = recipeDto.ImageAltText,
            Nutrition = recipeDto.Nutrition,
            CookingTime = recipeDto.CookingTime,
        };
        resultRecipe.Users.Add(userToAdd);
        _context.Recipe.Add(resultRecipe);
        await _context.SaveChangesAsync();

        return new RecipeResponse
        {
            Title = resultRecipe.Title,
            ImageUrl = resultRecipe.ImageUrl,
            Ingredients = CreateArray(resultRecipe.Ingredients),
            Instructions = CreateArray(resultRecipe.Instructions),
            ImageAltText = resultRecipe.ImageAltText,
            Nutrition = resultRecipe.Nutrition,
            CookingTime = resultRecipe.CookingTime,
            UserId = recipeDto.UserId,
            RecipeId = resultRecipe.Id
        };
    }
    
    public async Task<User> AddNewUser(UserDto userDto)
    {
        var userToAdd = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            ImageUrl = userDto.ImageUrl
        };
        _context.User.Add(userToAdd);
        await _context.SaveChangesAsync();
        return userToAdd;
    }

    public bool UserExists(int id)
    {
        return _context.User.FirstOrDefault(u => u.Id == id) is not null;
    }

    public User UserByEmail(string email)
    {
        var result = _context.User.FirstOrDefault(u => u.Email == email);
        return result ?? new User();
    }

    public int RecipeIdExists(string title)
    {
        var recipe = _context.Recipe.FirstOrDefault(r => r.Title == title);
        return recipe?.Id ?? -1;
    }

    public User? GetUser(int id)
    {
        return _context.User.FirstOrDefault(u => u.Id == id);
    }

    public ICollection<User> GetUsers()
    {
        return _context.User.ToList();
    }

    public Recipe? GetRecipe(int id)
    {
        return _context.Recipe.FirstOrDefault(u => u.Id == id);
    }

    public async Task<bool> DeleteFavourite(int userId, int recipeId)
    {
        var userToChange = _context.User.FirstOrDefault(u => u.Id == userId);
        userToChange.Recipes.Remove(_context.Recipe.Find(recipeId));
        _context.RecipeUser.Remove(new RecipeUser { UserId = userId, RecipeId = recipeId });
        await _context.SaveChangesAsync();
        return userToChange is not null;
    }

    public List<NoteResponse> GetNotes(int userId, int recipeId)
    {
        var notes = from n in _context.Note
            where n.Recipe.Id == recipeId && n.User.Id == userId
            let noteResponse = new NoteResponse
            {
                NoteId = n.Id,
                NoteText = n.NoteText,
                UserId = n.User.Id,
                RecipeId = n.Recipe.Id
            }
            select noteResponse;
        var result = notes.ToList();
        return result;
    }

    public NoteResponse? GetNote(int noteId)
    {
        var note = _context.Note.FirstOrDefault(n => n.Id == noteId);
        return new NoteResponse
        {
            NoteId = note.Id,
            NoteText = note.NoteText,
            Date = note.CreateDate
        };
    }

    public async Task<NoteResponse> AddNote(NoteDto noteDto, User user, Recipe recipe)
    {
        var noteToAdd = new Note
        {
            NoteText = noteDto.NoteText,
            User = user,
            Recipe = recipe
        };
        _context.Note.Add(noteToAdd);
        await _context.SaveChangesAsync();
        return new NoteResponse
        {
            NoteText = noteDto.NoteText,
            UserId = noteDto.UserId,
            RecipeId = noteDto.RecipeId,
            Date = noteToAdd.CreateDate,
            NoteId = noteToAdd.Id
        };
    }

    public async Task<bool> DeleteNote(int noteId)
    {
        var note = _context.Note.FirstOrDefault(n => n.Id == noteId);
        if (note is not null)
        {
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    public static string FlattenArray(string[] input)
    {
        var result = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (i == 0)
            {
                result = input[i];
            }
            else
            {
                result += $"|{input[i]}";
            }
        }

        return result;
    }

    public static string[] CreateArray(string input)
    {
        if (input == "")
        {
            return Array.Empty<string>();
        }

        return input.Split("|");
    }
}