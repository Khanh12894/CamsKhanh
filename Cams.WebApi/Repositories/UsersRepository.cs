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
using XichLip.WebApi.Models.User;
using WorkSimple.Infrastructure.Utils;

namespace XichLip.WebApi.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private IWSUnitOfWork _wsUnitOfWork;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(IWSUnitOfWork wsUnitOfWork, ILogger<UsersRepository> logger)
        {
            _wsUnitOfWork = wsUnitOfWork;
            _logger = logger;
        }
        public async Task<BaseListModel<UsersModel>> GetList(UsersRequestModel model, long userId)
        {
            try
            {
                var resultModel = new BaseListModel<UsersModel>();
                var param = new DynamicParameters();
                //param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Keyword", model.ID, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@PageIndex", model.PageIndex, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@PageSize", model.PageSize, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Sort", model.Sort, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Direction", model.Direction, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TotalRecord", model.PageSize, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_User_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    resultModel.ListModels = (await result.ReadAsync<UsersModel>()).ToList();

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

        public async Task<int> Save(UsersModel model, long userId)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@XML", Utils.SerializeXml<UsersModel>(model), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@UserId", model.ID, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                var result = _wsUnitOfWork.Context.Connection.Execute("cat_User_Save", param, commandType: CommandType.StoredProcedure);

                var Id = param.Get<int>("@UserId");
                model.Seq = Id;

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
                param.Add("@UserId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
                //param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@StatusID", status, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                _wsUnitOfWork.Context.Connection.Execute("cat_User_Delete", param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@StatusID");
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<UsersModel> Get(long id, long userId)
        {
            try
            {
                var model = new UsersModel();

                var param = new DynamicParameters();
                param.Add("@Id", id, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_User_Getlist", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<UsersModel>()).FirstOrDefault();

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
