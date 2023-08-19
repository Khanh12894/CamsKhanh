// Generated date: 2022/11/21
#region Using
using System.Collections.Generic;
using System.Threading.Tasks;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.SmsOTP;
using XichLip.WebApi.Models.User;
using XichLip.WebApi.Models.UserTokens;
#endregion Using

namespace XichLip.WebApi.Interfaces
{
    /// <summary>
    /// Interface IUserRepositories
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2022/11/21
    /// Updated By:
    /// Updated Time:
    public interface IUserRepositories {
        Task<BaseListModel<UserModel>> GetList(UserRequestModel req, long curentUserId);
        Task<UserModel> Get(int id, long curentUserId);
        Task<UserModel> FindByNameAsync2(string normalizedUserName);
        Task<int> Save(UserModel model);
        Task<int> SaveRefreshToken(UserTokensModel model);
        Task<UserTokensModel> GetRefreshToken(UserTokensModel model);
        Task<UserSimpleModel> GetUserByEmail(string email);
        Task<UserSimpleModel> GetEmailByUserId(UserModel UserId);
        Task<int> ChangeNewPassword(long userId, string pass);

        Task<int> ForgotPassword(long userId, string code);
        Task<int> CheckCodeForgotPassword(long userId, string code);

        Task<UserSimpleModel> GetUserById(long userId);

        Task<string> GetEmailByCode(string code);
        Task<string> GetUserNameByEmail(string email);
        Task<UserModel> FindByNameAsync(string normalizedUserName);
        Task<UserModel> FindByNameCommunicationAsync(UserModel model);
        Task<int> FindByNameSSOAsync(string normalizedUserName);
        Task<UserSSOModel> FindByNameUserXrefAsync(string normalizedUserName);
        
        Task<int> Register(UserModel model);
        Task<int> CheckOTP(UserOTPModel model);
    
        Task<int> CheckActivated(string UserName);
        Task<List<UserSSOModel>> FindByNameSSOAsync(UserSSOModel model);
        Task<int> InsertSSO(UserSSOModel model, long userid);
        Task<List<ClientModel>> GetListClient(long userId);
        Task<UserModel> FindByNameSSOClientAsync(UserSSOModel model);
        Task<int> UpdateSSO(UserSSOModel model);
        Task<UserSSOModel> FindByNameUserXrefClientAsync(string normalizedUserName, int? clientid);
        Task<int> UpdateOTP(UserModel model);
        Task<List<UserModel>> GetList(long userId);
        Task<UserSSOModel> CheckNumberSms(string phone);

    }     
}

