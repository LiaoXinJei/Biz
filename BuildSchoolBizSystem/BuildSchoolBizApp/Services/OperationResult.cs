using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildSchoolBizApp.Services
{
    public class OperationResult//目的是為了回傳操作是否成功，以及發生問題時的例外內容
    {
        public bool IsSuccessful { get; set; }
        public Exception exception { get; set; }
    }

    public static class OperationResultHelper//用來寫入
    {
        public static string WriteLog(this OperationResult value)//擴充方法
        {
            if (value.exception != null)
            {
                string path = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
                path = path + ".txt";
                File.WriteAllText(path, value.exception.ToString());
                return path;
            }
            else
            {
                return "沒有存檔";
            }
        }
    }
}
