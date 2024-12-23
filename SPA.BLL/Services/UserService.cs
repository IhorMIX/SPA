using AutoMapper;
using SPA.BLL.Exceptions;
using SPA.BLL.Models;
using SPA.BLL.Services.Interfaces;
using SPA.DAL.Entity;
using SPA.DAL.Repositories.Interfaces;

namespace SPA.BLL.Services;

public class UserService(IUserRepository userRepository,  IMapper mapper) : IUserService
{
    public async Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(id, cancellationToken);
        if (userDb == null)
        {
            throw new UserNotFoundException($"User with this Id {id} not found");
        }
        var userModel = mapper.Map<UserModel>(userDb);
        return userModel;
    }

    public async Task<UserModel> CreateUserAsync(string username, string email, string homePage, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.CreateUserAsync(
            new User 
            { 
                UserName = username, 
                Email = email, 
                HomePage = homePage 
            }, 
            cancellationToken);

        var userModel = mapper.Map<UserModel>(userDb);
        return userModel;
    }

    public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (userDb is null)
        {
            throw new UserNotFoundException($"User with this Id {userId} not found");
        }
        await userRepository.DeleteUserAsync(userDb,cancellationToken);
    }
}