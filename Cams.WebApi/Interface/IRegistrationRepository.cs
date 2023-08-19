using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.Registration;
using XichLip.WebApi.Models.User;

namespace XichLip.WebApi.Interface
{
    public interface IRegistrationRepository
    {
        Task<BaseListModel<RegistrationModel>> GetList(RegistrationRequestModel model, long userId);
        Task<int> Save(RegistrationModel model, long userId);
        Task<int> Delete(int id, long userId);
        Task<RegistrationModel> Get(long id, long userId);
    }
}
