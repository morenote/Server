using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace MoreNote.SignatureService.NetSign
{
  
    public interface INetSignApi: IHttpApi
    {


        [HttpPost("/rawSignature")]
        public Task<String> rawSignature(String data);

        [HttpPost("/rawVerify")]
        public Task<bool> rawVerify(String data, String sign, String cer);


    }
}
