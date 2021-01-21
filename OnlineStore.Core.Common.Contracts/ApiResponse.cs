using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Common.Contracts
{

    //client lar bu structer a gore api consume edecekler


   //her zaman serialize olabilir diye bir durum olmadıgı için
   //her zaman error message olcak diye bişey yok hata olmadıgında dönmeye gerek yok
    [DataContract]
    public class ApiResponse<T>
    {
        public ApiResponse(HttpStatusCode statusCode, T result, string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessage = errorMessage;
        }

        public ApiResponse()
        {

        }

        [DataMember]
        public string Version { get { return "1.0"; } }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public T Result { get; set; }
    }
}
