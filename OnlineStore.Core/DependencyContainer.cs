using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core
{
    //DI Container
    public static class DependencyContainer
    {
        //registiration işlemleri için
        public static IWindsorContainer WindsorContainer { get; private set; }

        //castle windsor da registiration işlemi
        //bin klasorundeki dll lerin  ref edilmiş hallerinin tutulacagı bir description
        public static FromAssemblyDescriptor Descriptor { get; private set; }

   
        private static bool bootstrapped;


        //variable lari doldurmak için 
        public static void Bootstrap()
        {
            if (!bootstrapped)
            {
                WindsorContainer = new WindsorContainer();

                //bin içindeki dll leri alıyoruz.
                //ana dizindeki bin klasöründeki dll leri aldık
                var assemblyFilter = new AssemblyFilter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));

                //filter ile belirtilen dosyadaki bütün class ları  doldur
                Descriptor = Classes.FromAssemblyInDirectory(assemblyFilter);

                //instal et assembly filter icindeki dll leri
                WindsorContainer.Install(FromAssembly.InDirectory(assemblyFilter));

                bootstrapped = true;
            }
        }

        public static T Resolve<T>()
        {
            return WindsorContainer.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return WindsorContainer.Resolve(type);
        }
    }
}
