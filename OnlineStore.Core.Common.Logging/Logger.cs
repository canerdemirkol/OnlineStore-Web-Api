using OnlineStore.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly log4net.ILog _log;
        public Logger()
        {
            //log4net e ait configurasyon dosyalarının nerden yüklenecegini söyledik
            log4net.Config.XmlConfigurator.Configure(
                new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            //log un nerden geldigini gormek icin
            //hangi provider in insert ettiginin bilgisini gormek icin
          
            _log = log4net.LogManager.GetLogger(GetType().FullName);
            //log atılan modlun ismini yazıyoruz ki bu log nerden gelmiş onu görelim (log insert ederken)
            //hangi provider dan log geldigini görebiliriz.


        }
        public void Info(string message, Dictionary<string, string> additionalColumns = null)
        {
            //Assertion ları gerceklestirdik.
            System.Diagnostics.Contracts.Contract.Assert(!string.IsNullOrEmpty(message));

            BindAdditionalColumnsIfNotEmpty(additionalColumns);

            _log.Info(message);
        }

        public void Error(string message, Exception exception, Dictionary<string, string> additionalColumns = null)
        {
            System.Diagnostics.Contracts.Contract.Assert(!string.IsNullOrEmpty(message));
            System.Diagnostics.Contracts.Contract.Assert(exception != null);

            BindAdditionalColumnsIfNotEmpty(additionalColumns);

            _log.Error(message, exception);
        }

        //logın idsini degerini bildirimek için
        //ThreadContext ile istedigimiz key e value atabiliriz.
        //bazı colonlar ile ilgli bilgi vermek istedigimizde kullanırız
        private void BindAdditionalColumnsIfNotEmpty(Dictionary<string, string> additionalColumns)
        {
            if (additionalColumns != null && additionalColumns.Any())
            {
                foreach (var column in additionalColumns)
                {
                    log4net.ThreadContext.Properties[column.Key] = column.Value;
                }
            }
        }
    }
}
