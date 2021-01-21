using System.Web.Http.Controllers;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OnlineStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineStore.Core.Common.Contracts;
using OnlineStore.Data;
using System.Data.Entity;
using OnlineStore.Data.Contracts;

namespace OnlineStore.ServiceHost.API.Core
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            RegisterInterceptors(container);

            RegisterHttpControllers(container);

            RegisterConfigurationHelpers(container);

            RegisterEngines(container);

            RegisterEntityFrameworkContexts(container);

            RegisterDataRepositories(container);

            RegisterCacheProviders(container);

            RegisterLoggers(container);
        }

        private void RegisterLoggers(IWindsorContainer container)
        {
            //global scope LifestyleSingleton
            container.Register(
                DependencyContainer.Descriptor
                .BasedOn<ILogger>()
                .WithServiceBase()
                .LifestyleSingleton()
                );
        }

        private void RegisterCacheProviders(IWindsorContainer container)
        {
            container.Register(
                DependencyContainer.Descriptor
                .BasedOn<ICacheProvider>()
                .WithServiceBase()
                .LifestyleSingleton()
                );
        }

        private static void RegisterDataRepositories(IWindsorContainer container)
        {
            container.Register(
                DependencyContainer.Descriptor
                .BasedOn(typeof(IRepository<,>))
                .WithService
                .AllInterfaces()
                .LifestylePerWebRequest()
                );
        }

        private static void RegisterEntityFrameworkContexts(IWindsorContainer container)
        {
            //DbContext istendiginde OnlineStoreContexi git bana getir
            container.Register(Component.For<DbContext>()
                .ImplementedBy<OnlineStoreContext>()
                .LifestylePerWebRequest()
                );
        }

        private void RegisterEngines(IWindsorContainer container)
        {
            //category controller i kullanacagız bir business engine var ve aslında ICategoryEngine ı   implente eden  bir conctare implemantasyonuna ihtiyac duyacagiz
            // bu yuzden registrationi IBusinessEngine uzerinden yapacagız ama biz senden isterken herhangi bir interface uzerinden isteyebilecegim icin AllInterfaces kullandım
            //ICategoryEngine resolve ederken bu classı implemente eden kimse (CategoryEngine) git bana bunu getir ve bunu butun interfaceler uzeinden yap bana
            container.Register(
                DependencyContainer.Descriptor
                .BasedOn<IBusinessEngine>()
                .WithService
                .AllInterfaces()
                .LifestylePerWebRequest()
                );
        }

        private void RegisterConfigurationHelpers(IWindsorContainer container)
        {
            //sadece bir kez olustur bu instance ve her zman onu kullan
            container.Register(
                DependencyContainer.Descriptor
                .BasedOn<IConfigurationHelper>()
                .WithServiceBase()
                .LifestyleSingleton()
                );
        }


        private void RegisterHttpControllers(IWindsorContainer container)
        {

            //projede nekadar ihttpcontroller concrate implemantasyonu varsa hepsini register et
            //
            container.Register(
                DependencyContainer.Descriptor
                .BasedOn<IHttpController>()
                .WithService//conctare implemantasyonlarını al
                .Self()
                .LifestylePerWebRequest()
                );
        }

        private void RegisterInterceptors(IWindsorContainer container)
        {
            //reflect edilmis assembler lar uzerinden bir islem yapacagız

            //implemente edilmiş concrate implemantasyonu kullanacagım
            //birden çok obje nin container üzerinde register edilmesini saglayacagız ancak onemli olan bizim için olusan objenin lifeCycle i ne olcagi
            //herbir web request icin bir tane instance ver
            container.Register(
                            DependencyContainer.Descriptor
                                .BasedOn<IInterceptor>()
                                .WithService
                                .Self()
                                .LifestylePerWebRequest()
                            );
            //aynı web request içinde oldugumuzda daha once olusturdugu instance verecektir. LifestyleSingleton hernesne her istege ozel olusturulur ve o istek boyunce hep aynı nesne kullanılır.
            //LifestyleTransient her request te yeniden bir instace olusturur dispose etme yuku bize kalır işimiz bittiginde otomatik dispose etmez
            //LifestyleSingleton en basit kuruluma sahiptir.olusturan nesneden bir defa olusturur ve kim isterse ona hep aynı nesneyi verir 
            ////////////////bu da bazı nesnelerde istemedigim kazalara yol acar farklı treadların aynı  nesneye ualasmak istediklerinde sorun olusturur.
            ///
        }


    }
}