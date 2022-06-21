using Microsoft.Extensions.Caching.Distributed;

using MoreNote.Common.Utils;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.SignatureService;
using MoreNote.Common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Security.USBKey.CSP
{

    public class EPassService
    {
        IDistributedIdGenerator idGenerator;

        ISignatureService signatureService;
        private IDistributedCache distributedCache;//内存缓存
        public EPassService(IDistributedIdGenerator idGenerator, 
                            ISignatureService signatureService,
                            IDistributedCache distributedCache)
        {
            this.idGenerator = idGenerator;
            this.distributedCache = distributedCache;
            this.signatureService = signatureService;


        }
        public ServerChallenge GenServerChallenge(string tag,string requestNumber, long? userId)
        {
            //随机数
            var random= RandomTool.CreatSafeRandomBase64(32);

            var challenge = new ServerChallenge()
            {
                Id = this.idGenerator.NextId(),
                UserId = userId,
                Tag = tag,
                RequestNumber = requestNumber,
                Random = random,
                UinxTime = UnixTimeUtil.GetTimeStampInInt32()
            };
            SaveServerChallenge(challenge);
            return challenge;
        }
        /// <summary>
        /// 存储挑战
        /// </summary>
        /// <param name="challenge"></param>
        private  void SaveServerChallenge(ServerChallenge challenge)
        {
            var json= JsonSerializer.Serialize(challenge);
            distributedCache.SetString("challenge" + challenge.Id.ToString(), json,200);

        }
        /// <summary>
        /// 从缓存中获得服务器挑战
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public ServerChallenge GetServerChallenge(long? challengeId)
        {
            var json=   distributedCache.GetString("challenge" + challengeId.ToString());
            if (json==null)
            {
                return null;
            }
            var challenge= JsonSerializer.Deserialize<ServerChallenge>(json);
            return challenge;
        }
        /// <summary>
        /// 删除服务器挑战
        /// </summary>
        /// <param name="challengeId"></param>
        public void DeleteChallenge(long challengeId)
        {
            distributedCache.Remove("challenge" + challengeId.ToString());
        }

        //验证客户端的响应
        public async Task<bool> VerifyClientResponse(ClientResponse clientResponse,bool clearChallenge=false)
        {
            var challenge = GetServerChallenge(clientResponse.Id);
            if (challenge==null)
            {
                return false;
            }
            var verfiy = challenge.VerifyTime(200);
            if (verfiy==false)
            {
                return false;
            }
            var data = challenge.GetBytes().ByteArrayToBase64();
            var cer = clientResponse.Certificate;
            var sign = clientResponse.Sign;
            //验证签名
            var result= await this.signatureService.rawVerify(data,sign,"",true,cer, "010001");
            return result;
        }

        public async Task<bool> VerifyDataSign(DataSignDTO dataSignDTO)
        {
            var data = dataSignDTO.SignData.GetBytes().ByteArrayToBase64();
            var cer = dataSignDTO.Certificate;
            var sign = dataSignDTO.Sign;
            var result = await this.signatureService.rawVerify(data, sign, "", true, cer, "010001");
            return result;
        }
    }
}
