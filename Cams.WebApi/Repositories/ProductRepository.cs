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
using System.IO;
using System.ComponentModel;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;

namespace XichLip.WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private IWSUnitOfWork _wsUnitOfWork;
        private readonly ILogger<ProductRepository> _logger;
        private readonly IHostingEnvironment _environment;

        public ProductRepository(IWSUnitOfWork wsUnitOfWork, ILogger<ProductRepository> logger, IHostingEnvironment environment)
        {
            _wsUnitOfWork = wsUnitOfWork;
            _logger = logger;
            _environment = environment;
        }
        public async Task<BaseListModel<ProductModel>> GetList(ProductRequestModel model, long userId)
        {
            try
            {
                var resultModel = new BaseListModel<ProductModel>();
                var param = new DynamicParameters();
                //param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Keyword", model.Name, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Expired", model.Expired, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@PageIndex", model.PageIndex, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@PageSize", model.PageSize, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Sort", model.Sort, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Direction", model.Direction, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TotalRecord", model.PageSize, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_Product_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    resultModel.ListModels = (await result.ReadAsync<ProductModel>()).ToList();
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

        public async Task<int> Save(ProductModel model, long userId)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@XML", Utils.SerializeXml<ProductModel>(model), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@ProductId", model.ProductId, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                var result = _wsUnitOfWork.Context.Connection.Execute("cat_Product_Save", param, commandType: CommandType.StoredProcedure);

                var Id = param.Get<int>("@ProductId");
                model.ProductId = Id;

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
                param.Add("@ProductId", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
                //param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@StatusID", status, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                _wsUnitOfWork.Context.Connection.Execute("cat_Product_Delete", param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@StatusID");
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                return -1;
            }
        }

        public async Task<ProductModel> Get(long id, long userId)
        {
            try
            {
                var model = new ProductModel();

                var param = new DynamicParameters();
                param.Add("@ProductId", id, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_Product_Get", param, commandType: CommandType.StoredProcedure))
                {
                    model = (await result.ReadAsync<ProductModel>()).FirstOrDefault();

                }
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<string> RandomKey()
        {
            try
            {
                Random res = new Random();

                // String that contain both alphabets and numbers
                String str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int size = 12;

                // Initializing the empty string
                String randomstring = "";

                for (int i = 0; i < size; i++)
                {

                    // Selecting a index randomly
                    int x = res.Next(str.Length);

                    // Appending the character at the 
                    // index to the random alphanumeric string.
                    randomstring = randomstring + str[x];
                }
                randomstring = randomstring.Substring(0, 4) + "-" + randomstring.Substring(4, 4) + "-" + randomstring.Substring(8, 4);
                return randomstring;

            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<string> GenKey()
        {
            try
            {
                var randomKey = await RandomKey();
                var param = new DynamicParameters();
                var keys = new List<string>();
                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_Product_Get_List_Key", param, commandType: CommandType.StoredProcedure))
                {
                    keys = (await result.ReadAsync<string>()).ToList();

                }
                for (int i = 0; i < 100; i++)
                {
                    if (!CheckExists(randomKey, keys))
                    {
                        return randomKey;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public bool CheckExists(string randomKey, List<string> keys)
        {
            try
            {
                if (keys.Exists(p => p == randomKey))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }
        }
        public async Task<byte[]> DownLoad()
        {
            try
            {
                var resultModel = new BaseListModel<ProductModel>();
                var param = new DynamicParameters();
                param.Add("@Keyword", null, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Expired", null, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@PageIndex", null, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@PageSize", null, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Sort", null, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Direction", null, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TotalRecord", null, dbType: DbType.Int64, direction: ParameterDirection.InputOutput);

                using (var result = await _wsUnitOfWork.Context.Connection.QueryMultipleAsync("cat_Product_GetList", param, commandType: CommandType.StoredProcedure))
                {
                    resultModel.ListModels = (await result.ReadAsync<ProductModel>()).ToList();
                }
                byte[] byteArray;
                var fileInfo = new FileInfo(Path.Combine(_environment.ContentRootPath, "Templates/Product.xlsx"));
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                    workSheet.Cells["A9"].LoadFromCollection(resultModel.ListModels);
                    byteArray = package.GetAsByteArray();
                }
                return byteArray;
            }
            catch (Exception ex)
            {
                _logger.LogError(Utils.GetExceptionLog(ex));
                throw ex;
            }

        }
    }
}
