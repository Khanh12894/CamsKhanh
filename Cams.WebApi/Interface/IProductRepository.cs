using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.Product;

namespace XichLip.WebApi.Interface
{
    public interface IProductRepository
    {
        Task<BaseListModel<ProductModel>> GetList(ProductRequestModel model, long userId);
        Task<int> Save(ProductModel model, long userId);
        Task<int> Delete(int id, long userId);
        Task<ProductModel> Get(long id, long userId);
        Task<string> GenKey();
        Task<byte[]> DownLoad();
    }
}
