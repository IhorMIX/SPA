using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using SPA.BLL.Exceptions;
using SPA.BLL.Models;
using SPA.BLL.Services.Interfaces;
using SPA.DAL.Entity;
using SPA.DAL.Helpers;
using SPA.DAL.Repositories.Interfaces;
using ReflectionHelper = SPA.BLL.Helpers.ReflectionHelper;

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

    public async Task CreateUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetAll()
            .FirstOrDefaultAsync(i => i.UserName == user.UserName,
                cancellationToken);
        
        if (userDb is not null && userDb.UserName == user.UserName)
            throw new AlreadyLoginException("Login is already used by another user");
        
        var userDbModel = mapper.Map<User>(user);
        userDbModel.Password = PasswordHelper.HashPassword(userDbModel.Password);
        await userRepository.CreateUserAsync(userDbModel, cancellationToken);
        user.Id = userDbModel.Id;
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
    
    public async Task<UserModel> UpdateUserAsync(int id, UserModel user, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(id, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with this Id {id} not found");
        
        userDb!.Password = string.IsNullOrEmpty(user.Password)
            ? userDb.Password
            : PasswordHelper.HashPassword(user.Password);
        
        foreach (var propertyMap in ReflectionHelper.WidgetUtil<UserModel, User>.PropertyMap)
        {
            var userProperty = propertyMap.Item1;
            var userDbProperty = propertyMap.Item2;
        
            var userSourceValue = userProperty.GetValue(user);
            var userTargetValue = userDbProperty.GetValue(userDb);
        
            if (userSourceValue != null && !ReferenceEquals(userSourceValue, "") && !userSourceValue.Equals(userTargetValue))
            {
                userDbProperty.SetValue(userDb, userSourceValue);
            }
        }
        
        await userRepository.UpdateUserAsync(userDb, cancellationToken);
        var userModel = mapper.Map<UserModel>(userDb);
        return userModel;
    }
    public async Task<UserModel> GetUserByRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetAll()
            .FirstOrDefaultAsync(i =>
                    i.AuthorizationInfo != null &&
                    i.AuthorizationInfo.RefreshToken == refreshToken,
                cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with this refresh token {refreshToken} not found");

        if (userDb.AuthorizationInfo is not null && userDb.AuthorizationInfo.ExpiredDate <= DateTime.Now.AddDays(-1))
            throw new TimeoutException();

        var userModel = mapper.Map<UserModel>(userDb);
        return userModel;
    }

    public async Task<UserModel?> GetUserByLoginAndPasswordAsync(string userName, string password,
        CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetAll().FirstOrDefaultAsync(i => i.UserName == userName, cancellationToken);
        if (userDb is null)
            throw new WrongLoginOrPasswordException("Wrong login or password");

        if (!PasswordHelper.VerifyHashedPassword(userDb!.Password, password))
        {
            throw new WrongLoginOrPasswordException("Wrong login or password");
        }

        var userModel = mapper.Map<UserModel>(userDb);
        return userModel;
    }

    public async Task AddAuthorizationValueAsync(UserModel user, string refreshToken,
        DateTime? expiredDate = null,
        CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetByIdAsync(user.Id, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with this Id {user.Id} not found");

        if (userDb!.AuthorizationInfo is not null && userDb.AuthorizationInfo.ExpiredDate <= DateTime.Now.AddDays(-1))
            await LogOutAsync(user.Id, cancellationToken);

        userDb.AuthorizationInfo = new AuthorizationInfo
        {
            RefreshToken = refreshToken,
            ExpiredDate = expiredDate
        };
        await userRepository.UpdateUserAsync(userDb, cancellationToken);
    }

    public async Task<UserModel?> GetUserByLogin(string userName, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetAll()
            .FirstOrDefaultAsync(i => i.UserName == userName, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with this login {userName} not found");

        var userModel = mapper.Map<UserModel>(userDb);
        return userModel;
    }

    public async Task LogOutAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userDb = await userRepository.GetAll().Include(r => r.AuthorizationInfo).FirstOrDefaultAsync(r => r.Id == userId, cancellationToken);
        if (userDb is null)
            throw new UserNotFoundException($"User with this Id {userId} not found");

        if (userDb.AuthorizationInfo is not null)
        {
            userDb.AuthorizationInfo = null;
            await userRepository.UpdateUserAsync(userDb, cancellationToken);
        }
        else throw new NullReferenceException($"User with this token not found");
    }
}    