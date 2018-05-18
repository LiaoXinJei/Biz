using BizDataLibrary.Models;
using BizDataLibrary.Repositories;
using BuildSchoolBizApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildSchoolBizApp.Services
{
    public class ProductService
    {
        public object GetModel { get; internal set; }

        //假的行為，到目前為止還不知道資料庫的真實內容，測試用
        //public static List<ProductViewModel> FakeProducts = new List<ProductViewModel>();

        public OperationResult Create(ProductViewModel input)
        {
            var result = new OperationResult();
            try
            {
                //if (FakeProducts.Any((x) => x.ParNo == product.PartNo) != null)
                //{
                //    throw new ArgumentException($"PartNo {product.PartNo} is not exsist");
                //}
                //else
                //{
                //    FakeProduct.Add(product);
                //    result.IsSuccessful = true;
                //}

                BizModel context = new BizModel();
                BizRepository<Product> repository = new BizRepository<Product>(context);
                //context 橋梁、承上啟下，接通兩個不同的端點(EX:資料庫、程式)
                Product entity = new Product()
                {
                    PartNo = input.PartNo,
                    PartName = input.PartName
                };
                repository.Create(entity);
                context.SaveChanges();
                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }

            return result;
        }

        public ProductListViewModel GetProducts()
        {
            var result = new ProductListViewModel();
            result.Items = new List<ProductViewModel>();
            BizModel context = new BizModel();
            BizRepository<Product> repository = new BizRepository<Product>(context);
            foreach(var item in repository.GetAll().OrderBy((x) => x.PartNo))
            {
                var p = new ProductViewModel()
                {
                    PartNo = item.PartNo,
                    PartName = item.PartName
                };
                result.Items.Add(p);
            }
            return result;
        }
    }
}
