using AutoMapper;
using OnlineStore.Core;
using OnlineStore.Core.Common.Contracts;
using OnlineStore.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Business
{
    public class BusinessEngineBase
    {
        //farklı farklı engine lerin farklı configurasyonları ola bilecegi için public yaptık
        protected IConfigurationHelper ConfigurationHelper;
        private readonly ILogger _logger;


        protected IMapper Mapper;
        public BusinessEngineBase()
        {
            //TODO: yuardaki field 'lar initialize edilecek

            //Auto Mapper configurastonlarını yükledik 
            var mapperConfiguraiton = new MapperConfiguration(cfg =>
            {
                RequestMessagesToEntities.Map(cfg);
                EntitiesToResponseMessages.Map(cfg);
            });
            Mapper = mapperConfiguraiton.CreateMapper();


            ConfigurationHelper = DependencyContainer.Resolve<IConfigurationHelper>();
            _logger = (ILogger)DependencyContainer.Resolve(typeof(ILogger));
        }

        /// <summary>
        /// wrapper method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        protected T ExecuteWithExceptionHandledOperation<T>(Func<T> fun)
        {
            try
            {
                var result = fun.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString(), ex);
                throw;
            }
        }

        protected void ExecuteWithExceptionHandledOperation(Action action)
        {
            try
            {

                action.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString(), ex);
                throw;
            }
        }
    }
}
