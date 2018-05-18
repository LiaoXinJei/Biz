using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildSchoolBizApp.ViewModels
{
    class ProcurementViewModel
    {
        public int ProcurementID { get; set; }
        public string PartNo { get; set; }
        public DateTime PurchasingDay { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int InventoryUnitPrice { get; set; }
    }
}
