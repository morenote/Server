﻿using Microsoft.Extensions.Caching.Distributed;

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
        public ServerChallenge GenServerChallenge(string tag,long? userId)
        {
            //随机数
            var random= RandomTool.CreatSafeRandomBase64(32);

            var challenge = new ServerChallenge()
            {
                Id = this.idGenerator.NextId(),
                UserId = userId,
                Tag = tag,
                Random = random,
                UinxTime = UnixTimeUtil.GetTimeStampInInt32()
            };
            SaveServerChallenge(challenge);
            return challenge;
        }
        private  void SaveServerChallenge(ServerChallenge challenge)
        {
            var json= JsonSerializer.Serialize(challenge);
            distributedCache.SetString("challenge" + challenge.Id.ToString(), json,200);

        }
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
            var result= await this.signatureService.rawVerify(data,sign,"",true,cer, "010001");
            return result;

        }
    }
}