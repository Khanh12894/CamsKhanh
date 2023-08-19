// Generated date: 2022/11/21
#region Using

using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XichLip.WebApi.Interfaces;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.SmsOTP;
using XichLip.WebApi.Models.User;
using XichLip.WebApi.Models.UserTokens;
using XichLip.WebApi.Utilities;
using WorkSimple.Infrastructure.Interfaces;
using WorkSimple.Infrastructure.Utils;

#endregion Using
namespace XichLip.WebApi.Repositories
{
    /// <summary>
    /// UserRepositories
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2022/11/21
    /// Updated By: 
    /// Updated Time:
    public class UserRepositories : IUserRepositories
    {
        private IWSUnitOfWork _wsUnitOfWork;
        private readonly ILogger<UserRepositories> _logger;

        public UserRepositories(IWSUnitOfWork wsUnitOfWork, ILogger<UserRepositories> logger)
        {
            _wsUnitOfWork = wsUnitOfWork;
            _logger = logger;
        }
        public async Task<BaseListModel<UserModel>> GetList(UserRequestModel req, long curentUserId)
        {
            try
            {
                var model = new BaseListModel<UserModel>();
                var param = new DynamicParameters();
                param.Add("@UserName", req.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@FullName", req.FullName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Keyword", req.Keyword, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@PageIndex", req.PageIndex, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@PageSize", req.PageSize, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Sort", req.Sort, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Direction", req.Direction, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TotalRecord", req.PageSize, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    model.ListModels = (await result.ReadAsync<UserModel>()).ToList();
                    //if (model.ListModels != null && model.ListModels.Count > 0)
                    //{
                    //    var listParents = result.Read<UserModel>().ToList();
                    //    foreach (var org in model.ListModels)
                    //    {
                    //        org.ParentObj = listParents.Where(w => w.UserId == org.ParentId).FirstOrDefault();
                    //    }
                    //}
                }

                var totalRecord = param.Get<long>("@TotalRecord");
                model.Paging = new PagerModel(req.PageIndex, req.PageSize, totalRecord);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }
        public async Task<UserModel> Get(int id, long curentUserId)
        {
            try
            {
                var model = new UserModel();
                var param = new DynamicParameters();
                param.Add("@UserId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_Get", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UserModel>()).FirstOrDefault();
                }
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }
        public async Task<int> Save(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", Utils.SerializeXml<UserModel>(model), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", model.UserId, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);
                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_UserProfile_Save", param, commandType: CommandType.StoredProcedure);

                var userid = param.Get<long>("@UserId");
                model.UserId = userid;
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", Utils.SerializeXml<UserModel>(user), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", user.UserId, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Get", param, commandType: CommandType.StoredProcedure);

                var userid = param.Get<long>("@UserId");
                user.UserId = userid;
                return user.UserId.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return "";
            }
        }

        public async Task<string> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserName", user.UserName, dbType: DbType.String, direction: ParameterDirection.Input);

                var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return result.UserName;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task SetUserNameAsync(UserModel user, string userName, CancellationToken cancellationToken)
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
                //return IdentityResult.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                //return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
            }
        }

        public async Task<string> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserName", user.UserName, dbType: DbType.String, direction: ParameterDirection.Input);

                var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return result.NormalizedUserName;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task SetNormalizedUserNameAsync(UserModel user, string normalizedName, CancellationToken cancellationToken)
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
                //return IdentityResult.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                //return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
            }
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

        public async Task<IdentityResult> UpdateAsync(UserModel user, CancellationToken cancellationToken)
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
                return IdentityResult.Failed(new IdentityError[] { });

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



        public async Task<int> FindByNameSSOAsync(string normalizedUserName)
        {
            try
            {
                int model = 0;
                var param = new DynamicParameters();
                param.Add("@UserName", normalizedUserName, dbType: DbType.String, direction: ParameterDirection.Input);

                //var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetByNameSSO_Client", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<int>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }

                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        
            public async Task<UserModel> FindByNameCommunicationAsync(UserModel model)
        {
            try
            {
                var modelnew = new UserModel();
                var param = new DynamicParameters();
                param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@CommunicationTypeId", model.CommunicationTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@id", model.id, dbType: DbType.Int32, direction: ParameterDirection.Input);
        
                //var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetByNameCommunication", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UserModel>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }

                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<UserModel> FindByNameAsync(string normalizedUserName)
        {
            try
            {
                var model = new UserModel();
                var param = new DynamicParameters();
                param.Add("@UserName", normalizedUserName, dbType: DbType.String, direction: ParameterDirection.Input);

                //var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetByName", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UserModel>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }

                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<UserModel> FindByNameAsync2(string normalizedUserName)
        {
            try
            {
                var model = new UserModel();
                var param = new DynamicParameters();
                param.Add("@UserName", normalizedUserName, dbType: DbType.String, direction: ParameterDirection.Input);

                //var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetByName2", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UserModel>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }

                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<int> SaveRefreshToken(UserTokensModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", Utils.SerializeXml<UserTokensModel>(model), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", model.UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Tokens_Save", param, commandType: CommandType.StoredProcedure);

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }
        public async Task<UserTokensModel> GetRefreshToken(UserTokensModel model)
        {
            try
            {
                var modelDetail = new UserTokensModel();
                var param = new DynamicParameters();
                param.Add("@Name", model.Name, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@RefreshToken", model.RefreshToken, dbType: DbType.String, direction: ParameterDirection.Input);

                modelDetail = _wsUnitOfWork.Context.Connection.Query<UserTokensModel>("sec_User_Tokens_GetRefreshToken", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return modelDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task<int> ChangeNewPassword(long userId, string pass)
        {
            try
            {
                var passwordHash = Utils.GetHashPassword(pass);
                var param = new DynamicParameters();
                param.Add("@PasswordHash", passwordHash, DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", userId, DbType.Int64, direction: ParameterDirection.Input);

                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_ChangeNewPassword", param, commandType: CommandType.StoredProcedure);
                var Pass = pass;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<UserSimpleModel> GetUserByEmail(string email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", email, DbType.String, direction: ParameterDirection.Input);
                var user = new UserSimpleModel();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetUserByEmail", param, commandType: CommandType.StoredProcedure))
                {
                    user = (await result.ReadAsync<UserSimpleModel>()).FirstOrDefault();
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }
        public async Task<UserSimpleModel> GetEmailByUserId(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId, DbType.Int64, direction: ParameterDirection.Input);
                var user = new UserSimpleModel();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetEmailByUserId", param, commandType: CommandType.StoredProcedure))
                {
                    user = (await result.ReadAsync<UserSimpleModel>()).FirstOrDefault();
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }

        public async Task<List<UserModel>> GetList(long userId)
        {
            try
            {
                var resultModel = new List<UserModel>();
                var param = new DynamicParameters();


                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    resultModel = (await result.ReadAsync<UserModel>()).ToList();

                }



                return resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public void Dispose()
        {
            // Nothing to dispose.
        }

        public async Task<int> ForgotPassword(long userId, string code)
        {
            try
            {

                string codeHash = EncryptHelper.Encrypt(code);
                var param = new DynamicParameters();
                param.Add("@CodeHash", codeHash, DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", userId, DbType.Int64, direction: ParameterDirection.Input);

                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_ForgotPassword", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<int> CheckCodeForgotPassword(long userId, string code)
        {
            try
            {
                int status = -1;
                var param = new DynamicParameters();
                string codeHash = EncryptHelper.Encrypt(code);
                param.Add("@CodeHash", codeHash, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@StatusId", status, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                param.Add("@UserId", userId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                var result = _wsUnitOfWork.Context.Connection.Execute("sec_User_CheckCodeResetPassword", param, commandType: CommandType.StoredProcedure);
                status = param.Get<int>("@StatusId");
                return status;
            }
            catch (Exception e)
            {
                _logger.LogError(Utils.GetExceptionLog(e));
                return -1;
            }
        }

        public async Task<UserSimpleModel> GetUserById(long userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", userId, DbType.String, direction: ParameterDirection.Input);
                var user = new UserSimpleModel();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetUserById", param, commandType: CommandType.StoredProcedure))
                {
                    user = (await result.ReadAsync<UserSimpleModel>()).FirstOrDefault();
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }

        public async Task<string> GetEmailByCode(string code)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Code", code, DbType.String, direction: ParameterDirection.Input);
                string user = "";
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_Get_Email_By_Code", param, commandType: CommandType.StoredProcedure))
                {
                    user = (await result.ReadAsync<string>()).FirstOrDefault();
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }

        public async Task<string> GetUserNameByEmail(string email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", email, DbType.String, direction: ParameterDirection.Input);
                string user = "";
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_Get_UserName_By_Email", param, commandType: CommandType.StoredProcedure))
                {
                    user = (await result.ReadAsync<string>()).FirstOrDefault();
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }
        public async Task<int> Register(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", Utils.SerializeXml<UserModel>(model), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", model.UserId, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);
                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Register", param, commandType: CommandType.StoredProcedure);

                var userid = param.Get<long>("@UserId");
                model.UserId = userid;
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }
        public async Task<int> CheckOTP(UserOTPModel model)
        {
            try
            {
                var status1 = 0;
                var status = 0;
                if (model.Time > 0)
                {
                    var param = new DynamicParameters();
                    param.Add("@OTP", model.OTP, dbType: DbType.String, direction: ParameterDirection.Input);
                    param.Add("@UserId", model.UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                    param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                    param.Add("@StatusId", model.UserId, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Register_OTP", param, commandType: CommandType.StoredProcedure);
                        status = param.Get<int>("@StatusId");
                    if (status == 1)
                    {
                        var param1 = new DynamicParameters();
                        param1.Add("@UserId", model.UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                        param1.Add("@StatusId", model.UserId, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        param1.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                        var resul1t = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Register_Activated", param1, commandType: CommandType.StoredProcedure);
                        status1 = param1.Get<int>("@StatusId");

                    }
                    else
                    {
                        return status;
                    }
                }
                else
                {
                    status = 2;
                }
                return status;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<int> ActiveOTP(UserOTPModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@StatusId", model.UserId, dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);


                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Register_Activated", param, commandType: CommandType.StoredProcedure);
                var status = param.Get<int>("@StatusId");
                return status;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }
        public async Task<int> CheckActivated(string UserName)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserName", UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@StatusId", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await _wsUnitOfWork.Context.Connection.ExecuteAsync("sec_User_Check_Activated", param, commandType: CommandType.StoredProcedure);
                var status = param.Get<int>("@StatusId");
                return status;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }


        public async Task<List<UserSSOModel>> FindByNameSSOAsync(UserSSOModel model)
        {
            try
            {
                List<UserSSOModel> lstitem = new List<UserSSOModel>();
                var param = new DynamicParameters();
                param.Add("@UserName", model.UserNameSSO, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@ClientId", model.ClientId, dbType: DbType.Int16, direction: ParameterDirection.Input);
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetByName_SSO_List", param, commandType: CommandType.StoredProcedure))
                {
                    lstitem = (await result.ReadAsync<UserSSOModel>()).ToList();

                }

                return lstitem;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task<UserModel> FindByNameSSOClientAsync(UserSSOModel model)
        {
            try
            {
                UserModel item = new UserModel();
                var param = new DynamicParameters();
                param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@ClientId", model.ClientId, dbType: DbType.Int16, direction: ParameterDirection.Input);
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetByName_SSO", param, commandType: CommandType.StoredProcedure))
                {
                    item = (await result.ReadAsync<UserModel>()).FirstOrDefault();

                }

                return item;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task<int> InsertSSO(UserSSOModel model, long userid)
        {
            try
            {
                UserSSOModel item = new UserSSOModel();
                var param = new DynamicParameters();
                param.Add("@UserId", userid, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@SsoIdentityId", model.ClientId, dbType: DbType.Int64, direction: ParameterDirection.Output);
                param.Add("@ClientId", model.ClientId, dbType: DbType.Int16, direction: ParameterDirection.Input);
                param.Add("@ClientUserId", model.ClientUserId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Email", model.Email, dbType: DbType.String, direction: ParameterDirection.Input);
                var result = _wsUnitOfWork.Context.Connection.Execute("SSO_Insert", param, commandType: CommandType.StoredProcedure);
                var SsoIdentityId = param.Get<long>("@SsoIdentityId");

                model.SsoIdentityId = SsoIdentityId;
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<int> UpdateOTP(UserModel model)
        {
            try
            {
                UserSSOModel item = new UserSSOModel();
                var param = new DynamicParameters();

                param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@OTP", model.OTP, dbType: DbType.String, direction: ParameterDirection.Input);


                var result = _wsUnitOfWork.Context.Connection.Execute("sec_UserUpdate_OTP", param, commandType: CommandType.StoredProcedure);
                string UserName = param.Get<string>("@UserName");

                model.UserName = UserName;
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<List<ClientModel>> GetListClient(long userId)
        {
            try
            {
                var resultModel = new List<ClientModel>();
                var param = new DynamicParameters();
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);


                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_SSO_Client_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    resultModel = (await result.ReadAsync<ClientModel>()).ToList();
                }

                return resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<int> UpdateSSO(UserSSOModel model)
        {
            try
            {
                UserSSOModel item = new UserSSOModel();
                var param = new DynamicParameters();
                param.Add("@UserId", 0, dbType: DbType.Int64, direction: ParameterDirection.Output);
                param.Add("@UserName", model.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserNameSSO", model.UserNameSSO, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@ClientId", model.ClientUpdate, dbType: DbType.Int64, direction: ParameterDirection.Input);

                var result = _wsUnitOfWork.Context.Connection.Execute("SSO_Update", param, commandType: CommandType.StoredProcedure);
                var SsoIdentityId = param.Get<long>("@UserId");

                model.SsoIdentityId = SsoIdentityId;
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<UserSSOModel> FindByNameUserXrefAsync(string normalizedUserName)
        {
            try
            {
                var model = new UserSSOModel();
                var param = new DynamicParameters();
                param.Add("@UserName", normalizedUserName, dbType: DbType.String, direction: ParameterDirection.Input);

                //var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_Xref_GetByName", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UserSSOModel>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }

                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<UserSSOModel> FindByNameUserXrefClientAsync(string normalizedUserName, int? clientid)
        {
            try
            {
                var model = new UserSSOModel();
                var param = new DynamicParameters();
                param.Add("@UserName", normalizedUserName, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@ClientId", clientid, dbType: DbType.Int32, direction: ParameterDirection.Input);
                //var result = _wsUnitOfWork.Context.Connection.Query<UserModel>("sec_User_GetByName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_Xref_Client_GetByName", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UserSSOModel>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }

                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<UserSSOModel> CheckNumberSms(string phone)
        {
            try
            {
                UserSSOModel item = new UserSSOModel();
                var param = new DynamicParameters();
                param.Add("@Phone", phone, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Count", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("sec_User_GetPhone", param, commandType: CommandType.StoredProcedure))
                {
                    item = (await result.ReadAsync<UserSSOModel>()).FirstOrDefault();
                    //model.OrganizationIds = (await result.ReadAsync<ListOrganizationId>()).ToList();
                }
                var totalRecord = param.Get<int>("@Count");
                item.Count = totalRecord;
                //var result = _wsUnitOfWork.Context.Connection.Query<CheckNumberSms>("sec_User_GetPhone", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return null;
            }
        }
    }
}

