using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Data;
using RecipeApi.Models;

namespace RecipeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository _repo;

    public UserController(IUserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Route("favourite/recipe/{recipeId}")]
    public ActionResult<RecipeResponse> GetFavouriteRecipe(int recipeId)
    {
        var response = _repo.GetFavouriteRecipe(recipeId);
        if (response.Title is null)
        {
            return NotFound("Incorrect recipe Id.");
        }

        return Ok(response);
    }

    [HttpGet]
    [Route("favourites/{userId}")]
    public IActionResult GetFavourites(int userId)
    {
        if (!_repo.UserExists(userId))
        {
            return NotFound("User not found.");
        }

        var resultList = _repo.GetFavsByUserId(userId);
        return Ok(resultList);
    }

    [HttpPost]
    [Route("favourite")]
    public async Task<IActionResult> PostFavourite([FromBody] RecipeDto recipeDto)
    {
        var userId = recipeDto.UserId;
        var recipeTitle = recipeDto.Title;

        if (!_repo.UserExists(userId))
        {
            return NotFound("User not found");
        }

        if (_repo.RecipeExistsInFav(recipeTitle, userId))
        {
            return Conflict($"Item already in favourites for user {userId.ToString()}");
        }

        var userToAdd = _repo.GetUser(userId);
        var idExists = _repo.RecipeIdExists(recipeTitle);
        if (idExists > 0)
        {
            _repo.AddRecipeToUser(userToAdd, recipeTitle);
            var response = new RecipeResponse
            {
                Title = recipeTitle,
                RecipeId = idExists,
                Instructions = Array.Empty<string>(),
                Ingredients = Array.Empty<string>()
            };
            return Ok(response);
        }

        var recipeResponse = await _repo.AddRecipe(recipeDto);
        return CreatedAtAction(nameof(GetFavouriteRecipe),
            new { recipeId = recipeResponse.RecipeId },
            recipeResponse);
    }

    [HttpDelete]
    [Route("{userId}/{recipeId}")]
    public async Task<IActionResult> RemoveFavourite(int userId, int recipeId)
    {
        var deleted = await _repo.DeleteFavourite(userId, recipeId);
        return deleted ? NoContent() : BadRequest("Id not found.");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> AddUser(UserDto userDto)
    {
        var userToCheck = _repo.UserByEmail(userDto.Email);
        if (userToCheck.Name is not null)
        {
            return Ok(userToCheck);
        }

        var userToAdd = await _repo.AddNewUser(userDto);
        return Ok(userToAdd);
    }

    [HttpGet]
    [Route("notes/{userId}/{recipeId}")]
    public IActionResult GetNotes(int userId, int recipeId)
    {
        var user = _repo.GetUser(userId);
        var recipe = _repo.GetRecipe(recipeId);

        if (user is null || recipe is null)
        {
            return BadRequest($"Incorrect input for user {userId.ToString()} and recipe {recipeId.ToString()}");
        }
        
        return Ok(_repo.GetNotes(userId, recipeId));
    }

    [HttpGet]
    [Route("note/{noteId}")]
    public ActionResult<NoteResponse> GetNote(int noteId)
    {
        return Ok(_repo.GetNote(noteId));
    }

    [HttpDelete]
    [Route("note/{noteId}")]
    public async Task<ActionResult> RemoveNote(int noteId)
    {
        var deleted = await _repo.DeleteNote(noteId);
        return deleted ? NoContent() : BadRequest("Id not found.");
    }
    
    [HttpPost]
    [Route("note")]
    public async Task<IActionResult> PostNote([FromBody] NoteDto noteDto)
    {
        var user = _repo.GetUser(noteDto.UserId);
        var recipe = _repo.GetRecipe(noteDto.RecipeId);
        if (user is null || recipe is null)
        {
            return BadRequest($"Incorrect input for user {noteDto.UserId.ToString()} and recipe {noteDto.RecipeId.ToString()}");
        }

        var note = await _repo.AddNote(noteDto, user, recipe);

        return CreatedAtAction(nameof(GetNote), new { noteId = note.NoteId }, note);
    }

    [HttpGet]
    [Route("users")]
    public ICollection<User> GetUser()
    {
        return _repo.GetUsers();
    }
}