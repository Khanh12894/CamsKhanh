using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;
using XichLip.WebApi.Models.User;
using System.Data;
using WorkSimple.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using WorkSimple.Infrastructure.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace XichLip.WebApi.Repositories
{
    public class WSUserStore : IUserStore<UserModel>, IUserTwoFactorStore<UserModel>, 
            IUserPasswordStore<UserModel>, IUserEmailStore<UserModel>, IUserPhoneNumberStore<UserModel>,
        IUserValidator<UserModel>,IUserSecurityStampStore<UserModel>//, IUserRoleStore<UserModel>
    {
        private IWSUnitOfWork _wsUnitOfWork;
        private readonly ILogger<WSUserStore> _logger;

        public WSUserStore(IWSUnitOfWork wsUnitOfWork, ILogger<WSUserStore> logger)
        {
            _wsUnitOfWork = wsUnitOfWork;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", Utils.SerializeXml<UserModel>(user), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", user.UserId, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Save", param, commandType: CommandType.StoredProcedure);

                var userid = param.Get<long>("@UserId");
                user.UserId = userid;
                return IdentityResult.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
            }
        }

        public async Task<IdentityResult> DeleteAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", user.UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@StatusID", 0, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Delete", param, commandType: CommandType.StoredProcedure);

                var statusId = param.Get<long>("@StatusID");
                if (statusId > 0)
                    return IdentityResult.Success;
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = "There is an error when delete user. Please contact to adminstrator for more information." } });

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
            }
        }

        public async Task<UserModel> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_Get", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task<UserModel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserName", normalizedUserName, dbType: DbType.String, direction: ParameterDirection.Input);

                var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return result;

        }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
}

        public Task<string> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(UserModel user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(UserModel user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", Utils.SerializeXml<UserModel>(user), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", user.UserId, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Identity_Save", param, commandType: CommandType.StoredProcedure);

                var userid = param.Get<long>("@UserId");
                user.UserId = userid;
                return IdentityResult.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
            }
        }

        public Task SetEmailAsync(UserModel user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed ?? false);
        }

        public Task SetEmailConfirmedAsync(UserModel user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<UserModel> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", normalizedEmail, dbType: DbType.String, direction: ParameterDirection.Input);

                var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByEmail", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public Task<string> GetNormalizedEmailAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(UserModel user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(UserModel user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed??false);
        }

        public Task SetPhoneNumberConfirmedAsync(UserModel user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(UserModel user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled??false);
        }

        public Task SetPasswordHashAsync(UserModel user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        //public async Task AddToRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync(cancellationToken);
        //        var normalizedName = roleName.ToUpper();
        //        var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT [Id] FROM [ApplicationRole] WHERE [NormalizedName] = @{nameof(normalizedName)}", new { normalizedName });
        //        if (!roleId.HasValue)
        //            roleId = await connection.ExecuteAsync($"INSERT INTO [ApplicationRole]([Name], [NormalizedName]) VALUES(@{nameof(roleName)}, @{nameof(normalizedName)})",
        //                new { roleName, normalizedName });

        //        await connection.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM [UserModelRole] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}) " +
        //            $"INSERT INTO [UserModelRole]([UserId], [RoleId]) VALUES(@userId, @{nameof(roleId)})",
        //            new { userId = user.Id, roleId });
        //    }
        //}

        //public async Task RemoveFromRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync(cancellationToken);
        //        var roleId = await connection.ExecuteScalarAsync<int?>("SELECT [Id] FROM [ApplicationRole] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() });
        //        if (!roleId.HasValue)
        //            await connection.ExecuteAsync($"DELETE FROM [UserModelRole] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}", new { userId = user.Id, roleId });
        //    }
        //}

        //public async Task<IList<string>> GetRolesAsync(UserModel user, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync(cancellationToken);
        //        var queryResults = await connection.QueryAsync<string>("SELECT r.[Name] FROM [ApplicationRole] r INNER JOIN [UserModelRole] ur ON ur.[RoleId] = r.Id " +
        //            "WHERE ur.UserId = @userId", new { userId = user.Id });

        //        return queryResults.ToList();
        //    }
        //}

        //public async Task<bool> IsInRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var roleId = await connection.ExecuteScalarAsync<int?>("SELECT [Id] FROM [ApplicationRole] WHERE [NormalizedName] = @normalizedName", new { normalizedName = roleName.ToUpper() });
        //        if (roleId == default(int)) return false;
        //        var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM [UserModelRole] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}",
        //            new { userId = user.Id, roleId });

        //        return matchingRoles > 0;
        //    }
        //}

        //public async Task<IList<UserModel>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var queryResults = await connection.QueryAsync<UserModel>("SELECT u.* FROM [UserModel] u " +
        //            "INNER JOIN [UserModelRole] ur ON ur.[UserId] = u.[Id] INNER JOIN [ApplicationRole] r ON r.[Id] = ur.[RoleId] WHERE r.[NormalizedName] = @normalizedName",
        //            new { normalizedName = roleName.ToUpper() });

        //        return queryResults.ToList();
        //    }
        //}

        public void Dispose()
        {
            // Nothing to dispose.
        }

        public Task<IdentityResult> ValidateAsync(UserManager<UserModel> manager, UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(UserModel user, string stamp, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task<string> GetSecurityStampAsync(UserModel user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task AddToRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(UserModel user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserModel>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
