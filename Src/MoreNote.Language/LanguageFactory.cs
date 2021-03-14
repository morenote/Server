using MoreNote.Language.Model;
using MoreNote.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Language
{
    public class LanguageFactory
    {
        private static Dictionary<string, LanguageResource> LanguageResources = new Dictionary<string, LanguageResource>();

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        public static LanguageResource GetLanguageResource(string language)
        {
            switch (language)
            {
                case "de-de":
                    return GetLanguageResource(LanguageEnum.ZH_CN);

                case "en-us":
                    return GetLanguageResource(LanguageEnum.EN_US);

                case "es-co":
                    return GetLanguageResource(LanguageEnum.ES_CO);

                case "fr-fr":
                    return GetLanguageResource(LanguageEnum.FR_FR);

                case "pt-pt":
                    return GetLanguageResource(LanguageEnum.PT_PT);

                case "zh-hk":
                    return GetLanguageResource(LanguageEnum.ZH_HK);

                default:
                    return GetLanguageResource(LanguageEnum.ZH_CN);
            }
        }

        public static LanguageResource GetLanguageResource(LanguageEnum languageEnum)
        {
            string locale = Enum.GetName(typeof(LanguageEnum), languageEnum).ToLower();
            if (!LanguageResources.Keys.Contains(locale))
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (!LanguageResources.Keys.Contains(locale))
                    {
                        LanguageResources.Add(locale, new LanguageResource(locale));
                    }
                }
            }
            return LanguageResources[locale];
        }
    }
}