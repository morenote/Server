namespace UpYunLibrary.ContentRecognition
{
    /// <summary>
    /// 内容识别操作动作钩子消息
    /// </summary>
    public class ContentIdentifiesHookMessages
    {
        public string operation { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public string task_id { get; set; }
    }
}