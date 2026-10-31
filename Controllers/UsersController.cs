using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

// Defines the namespace for this controller
namespace UserManagementAPI.Controllers;

// Marks this class as an API controller
[ApiController]

// Sets the route to api/users
// [controller] automatically becomes "users"
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    // In-memory list used to store users while the application is running
    // Data will be be lost when the application stops
    private static readonly List<User> Users = [];

    // GET: api/users
    // Returns all users
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        // Return all users in the list
        return Ok(Users);
    }

    // GET: api/users/1
    // Returns a specific user by ID
    [HttpGet("{id:int}")]
    public ActionResult<User> GetById(int id)
    {
        // Find the first user whose Id matches the requested Id
        var user = Users.FirstOrDefault(user => user.Id == id);

        // Return 404 if no user is found
        // Otherwise return 200 with the user data
        return user is null ? NotFound() : Ok(user);
    }

    // POST: api/users
    // Creates a new user
    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        if (user == null)
            return BadRequest();

        // Generate a new Id
        // Start at 1 if the list is empty
        var id = Users.Count == 0
            ? 1
            : Users.Max(existingUser => existingUser.Id) + 1;

        // Create a new user object
        var createdUser = new User
        {
            Id = id,
            UserName = user.UserName,
            UserEmail = user.UserEmail
        };

        // Add the new user to the list
        Users.Add(createdUser);

        // Return HTTP 201 (Created)
        // Includes a link to retrieve the newly created user
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdUser.Id },
            createdUser);
    }

    // PUT: api/users/1
    // Updates an existing user
    [HttpPut("{id:int}")]
    public ActionResult<User> Update(int id, User user)
    {
        // Find the index of the user with the matching Id
        var index = Users.FindIndex(existingUser => existingUser.Id == id);

        // Return 404 if the user doesn't exist
        if (index < 0)
        {
            return NotFound();
        }

        // Create an updated user object
        var updatedUser = new User
        {
            Id = id,
            UserName = user.UserName,
            UserEmail = user.UserEmail
        };

        // Replace the existing user with the updated user
        Users[index] = updatedUser;

        // Return 200 OK with the updated user
        return Ok(updatedUser);
    }

    // DELETE: api/users/1
    // Removes a user by ID
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        // Find the index of the user
        var index = Users.FindIndex(user => user.Id == id);

        // Return 404 if the user doesn't exist
        if (index < 0)
        {
            return NotFound();
        }

        // Remove the user from the list
        Users.RemoveAt(index);

        // Return 204 No Content to indicate successful deletion
        return NoContent();
    }

    //test exception handler
    [HttpGet("error")]
    public IActionResult TestError()
    {
        throw new Exception("Test exception");
    }
}


// Microsoft Copilot was used to generate the initial CRUD controller structure.
// The generated code was reviewed, simplified, and documented to improve readability
// and provide explanations for each API endpoint and operation.
//Copilot aided in error checking and code improvements after initial code creating.