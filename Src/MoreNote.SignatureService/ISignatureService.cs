namespace MoreNote.SignatureService
{
    public interface ISignatureService
    {

       
        public Task<String> rawSignature(String data);

      
        public Task<bool> rawVerify(String data, String sign, String cer, bool usbKey, String pubKeyModulusInHex, String pubKeyExpInHex);

    }
}