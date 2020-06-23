namespace UpYunLibrary.OSS
{
    public class UpYunPolicy
    {
    }

    public class OSSOptions
    {
        public string bucket { get; set; }
        public string save_key{ get; set; }
        public int expiration { get; set; }

    }
}