using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.User;

namespace XichLip.WebApi.Interface
{
    public interface IUsersRepository
    {
        Task<BaseListModel<UsersModel>> GetList(UsersRequestModel model, long userId);
        Task<int> Save(UsersModel model, long userId);
        Task<int> Delete(int id, long userId);
        Task<UsersModel> Get(long id, long userId);
    }
}
