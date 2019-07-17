// This is just the Repository Interface, while the Repository Implementation will be done in AuthRepository.cs

using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        // we return a task of User, called method Register 
        // which passes model User and string type password

        Task<User> Login(string username, string password);
        // we return a task of User,
        // This is the info that user needs to supply us 
        // to check their username and passw against what's in the database

        Task<bool> UserExists(string username);
        // Return a task of boolean ( yes or no)
        // which takes strin type username
        // to check to see if that username is already taken in our database
    }
}