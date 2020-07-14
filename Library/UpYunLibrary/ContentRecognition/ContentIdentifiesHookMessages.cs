using System.Text.Json.Serialization;

namespace UpYunLibrary.ContentRecognition
{
    /// <summary>
    /// 内容识别操作动作钩子消息
    /// </summary>
    public class ContentIdentifiesHookMessages
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UpyunOperation operation { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UpyunContent content { get; set; }
        public string service { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UpyunType type { get; set; }
        public string uri { get; set; }
        public string task_id { get; set; }
    }
    public enum UpyunOperation { auto, person }
    public enum UpyunContent { image, live , text }
    public enum UpyunType { delete, shield, cancel_shield , forbidden, cancel_forbidden, test }

}