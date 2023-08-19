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
using XichLip.WebApi.Models.Product;
using WorkSimple.Infrastructure.Utils;
using XichLip.WebApi.Models.ProductType;

namespace XichLip.WebApi.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private IWSUnitOfWork _wsUnitOfWork;
        private readonly ILogger<ProductTypeRepository> _logger;

        public ProductTypeRepository(IWSUnitOfWork wsUnitOfWork, ILogger<ProductTypeRepository> logger)
        {
            _wsUnitOfWork = wsUnitOfWork;
            _logger = logger;
        }
        public async Task<BaseListModel<ProductTypeModel>> GetProductTypeSelectList()
        {
            try
            {
                var resultModel = new BaseListModel<ProductTypeModel>();
                

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_ProductType_SelectList", commandType: CommandType.StoredProcedure))
                {
                    resultModel.ListModels = (await result.ReadAsync<ProductTypeModel>()).ToList();

                }
                return resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
    }
}
