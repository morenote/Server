using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;

namespace MoreNote.SignatureService.NetSign
{
    public class NetSignService : ISignatureService
    {

        INetSignApi _netSignApi;
        public NetSignService(INetSignApi netSignApi)
        {
            _netSignApi = netSignApi;
        }

        public Task<string> rawSignature(string data)
        {
            return _netSignApi.rawSignature(data);
        }

        public Task<bool> rawVerify(string data, string sign, string cer)
        {
            return this._netSignApi.rawVerify(data, sign, cer);
        }
    }
}
