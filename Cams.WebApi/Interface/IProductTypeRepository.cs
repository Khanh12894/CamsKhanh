using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XichLip.WebApi.Models.Base;
using XichLip.WebApi.Models.Product;
using XichLip.WebApi.Models.ProductType;

namespace XichLip.WebApi.Interface
{
    
    public interface IProductTypeRepository
    {
        Task<BaseListModel<ProductTypeModel>> GetProductTypeSelectList();
        //Task<int> Save(ProductModel model, long userId);
        //Task<int> Delete(int id, long userId);
        //Task<ProductModel> Get(long id, long userId);
    }
}
