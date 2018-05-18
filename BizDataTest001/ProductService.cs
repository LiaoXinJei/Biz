using BizDataLibrary.Models;
using BizDataLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizDataTest001
{
    public class ProductService
    {
        public void Create(Product entity)
        {
            BizModel context = new BizModel();
            BizRepository<Product> repository = new BizRepository<Product>(context);
            repository.Create(entity);
            context.SaveChanges();
        }

        public List<Product> GetAll()
        {
            BizModel context = new BizModel();
            BizRepository<Product> repository = new BizRepository<Product>(context);
            //var result = repository.GetAll<string>((x) => x.PartNo).ToList();
            var result = repository.GetAll().OrderBy((x) => x.PartNo).ToList();
            return result;
        }

        public Product GetByPartNo(string partno)
        {
            BizModel context = new BizModel();
            BizRepository<Product> repository = new BizRepository<Product>(context);
            //var result = repository.GetAll<string>((x) => x.PartNo)
            //    .FirstOrDefault((x) => x.PartNo == partno);
            var result = repository.GetAll().FirstOrDefault((x) => x.PartNo == partno);
            return result;
        }
    }
}
