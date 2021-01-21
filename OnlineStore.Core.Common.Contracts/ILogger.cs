using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Common.Contracts
{
    public interface ILogger
    {
        //bilgi mesajı için
        //log işlemlerinde
        //spesific olarak log blolarinda meydana gelebilecek
        //generik olmayan kolanlar olabildigi için
        //hatanın neyle ilgili oldugunu bildirmek icin
        //user ile ilgili bir şey logglarken key=userId value=userId olarak logglaya biliriz
        void Info(string message,Dictionary<string,string> additionalColumns=null);

        //hata mesajlari icin
        void Error(string message, Exception exeption, Dictionary<string, string> additionalColumns = null);
    }
}
