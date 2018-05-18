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
    public class SellingService
    {
        public OperationResult Create(SellingViewModel input)
        {
            var result = new OperationResult();
            try
            {
                BizModel context = new BizModel();
                using (var transaction = context.Database.BeginTransaction()) //以下三張表都有send動作，每次都rollback，錯誤就死
                {
                    var sellCount = input.Quantity;
                    var procurementRepo = new BizRepository<Procurement>(context); //庫存
                    var sellingSourceRepo = new BizRepository<SellingSource>(context);//
                    var sellingRepo = new BizRepository<Selling>(context);//總需求
                    Selling entity = new Selling()//產生銷售實體
                    {
                        PartNo = input.PartNo,
                        Quantity = input.Quantity,
                        SalesJobNumber = input.SalesJobNumber,
                        SellingDay = input.SellingDay,
                        UnitPrice = input.UnitPrice
                    };
                    sellingRepo.Create(entity);//新建selling repository
                    context.SaveChanges();
                    var products = procurementRepo.GetAll()
                        .Where((x) => x.PartNo == input.PartNo).OrderBy((x) => x.PurchasingDay);
                    foreach (var p in products)
                    {
                        if (sellCount <= 0) //庫存
                        {
                            break;
                        }
                        else
                        {
                            //throw new Exception("test");
                            
                            if (p.InvetoryQuantity >= sellCount) // 庫存大於
                            {
                                p.InvetoryQuantity = p.InvetoryQuantity - sellCount;
                                CreateSellingSource(sellingSourceRepo, entity.SellingId, p.ProcurementId, sellCount);
                                sellCount = 0;
                            }
                            else//大於庫存
                            {
                                sellCount = sellCount - p.InvetoryQuantity;
                                CreateSellingSource(sellingSourceRepo, entity.SellingId, p.ProcurementId, p.InvetoryQuantity);
                                p.InvetoryQuantity = 0;
                            }
                        }
                    }

                    context.SaveChanges();
                    result.IsSuccessful = true;
                    transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;
        }

        private void CreateSellingSource(BizRepository<SellingSource> sellingSourceRepo, int sellingId, int procurementId, int sellCount)
        {

            SellingSource sellingSource = new SellingSource()
            {
                ProcurementId = procurementId,
                SellingId = sellingId,
                Quantity = sellCount
            };
            sellingSourceRepo.Create(sellingSource);

        }

        public SellingListQueryViewModel GetSellingBySalesAndDay(int jobNumber, DateTime begin, DateTime end)
        {
            var result = new SellingListQueryViewModel();
            BizModel context = new BizModel();
            var sellingRepo = new BizRepository<Selling>(context);
            var salesManRepo = new BizRepository<SalesMan>(context);
            var temp =
            from selling in sellingRepo.GetAll()
            join sales in salesManRepo.GetAll()
            on selling.SalesJobNumber equals sales.JobNumber
            where selling.SalesJobNumber == jobNumber
            && selling.SellingDay >= begin
            && selling.SellingDay < end
            select new SellingQueryViewModel
            {
                PartNo = selling.PartNo,
                Quantity = selling.Quantity,
                SalesJobNumber = selling.SalesJobNumber,
                SalesName = sales.Name,
                SellingDay = selling.SellingDay,
                SellingId = selling.SellingId,
                UnitPrice = selling.UnitPrice,
                TotalPrice = selling.UnitPrice * selling.Quantity
            };

            result.Items = temp.ToList();

            return result;
        }
    }
}
