using OnlineStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace OnlineStore.ServiceHost.API.Core
{

    //web api hangi controller in kullanacagının bulundugu yer

    //hangi dependency lerin resolve edilme işlmelerini gerçekleştirelim
    public class WindsorHttpControllerActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {

            ///reflecte edimiş bir tipin Container içinde bir instance i varsa onu bana ver

            var instance = DependencyContainer.Resolve(controllerType);

            if (instance == null)
            {
                //contoler bulunamadı
                throw new HttpException((int)HttpStatusCode.NotFound, string.Format("{0} cannot be resolved.", controllerType.Name));
            }
            //cast etme işlemi
            //istenilen controller resolve edildi
            return (IHttpController)instance;
        }
    }
}