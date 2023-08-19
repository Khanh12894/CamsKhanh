using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WorkSimple.Infrastructure.Interfaces;
using XichLip.WebApi.Interface;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.Registration;
using WorkSimple.Infrastructure.Utils;
using XichLip.WebApi.Models.User;


namespace XichLip.WebApi.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private IWSUnitOfWork _wsUnitOfWork;
        private readonly ILogger<RegistrationRepository> _logger;

        public RegistrationRepository(IWSUnitOfWork wsUnitOfWork, ILogger<RegistrationRepository> logger)
        {
            _wsUnitOfWork = wsUnitOfWork;
            _logger = logger;
        }
        public async Task<BaseListModel<RegistrationModel>> GetList(RegistrationRequestModel model, long userId)
        {
            try
            {
                var resultModel = new BaseListModel<RegistrationModel>();
                var param = new DynamicParameters();
                //param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Keyword", model.Version, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@PageIndex", model.PageIndex, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@PageSize", model.PageSize, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Sort", model.Sort, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Direction", model.Direction, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TotalRecord", model.PageSize, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_Registration_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    resultModel.ListModels = (await result.ReadAsync<RegistrationModel>()).ToList();

                }
                var totalRecord = param.Get<long>("@TotalRecord");
                resultModel.Paging = new PagerModel(model.PageIndex, model.PageSize, totalRecord);
                return resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }

        public async Task<int> Save(RegistrationModel model, long userId)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@XML", Utils.SerializeXml<RegistrationModel>(model), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Id", model.No, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                var result = _wsUnitOfWork.Context.Connection.Execute("cat_Registration_Save", param, commandType: CommandType.StoredProcedure);

                var Id = param.Get<int>("@Id");
                model.No = Id;

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<int> Delete(int id, long userId)
        {
            try
            {
                int status = -1;
                var param = new DynamicParameters();
                param.Add("@RegistrationId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
                //param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@StatusID", status, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                _wsUnitOfWork.Context.Connection.Execute("cat_Registration_Delete", param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@StatusID");
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<RegistrationModel> Get(long id, long userId)
        {
            try
            {
                var model = new RegistrationModel();

                var param = new DynamicParameters();
                param.Add("@RegistrationId", id, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_Registration_Get", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<RegistrationModel>()).FirstOrDefault();

                }
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
    }
}
