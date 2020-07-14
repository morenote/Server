using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;

using SpamClassificationML.Model;

using System;

namespace MoreNote.Logic.Service
{
    public class SpamService
    {
        private static ConsumeModel consumeModel = null;

        private static Object lockObj = new object();

        private static ConsumeModel GetConsumeModel()
        {
            WebSiteConfig webSiteConfig = ConfigFileService.GetWebConfig();
            if (consumeModel == null)
            {
                lock (lockObj)
                {
                    if (consumeModel == null)
                    {
                        consumeModel = new ConsumeModel();
                        if (consumeModel.predEngine == null)
                        {
                            consumeModel.loadModel(webSiteConfig.ModelPath);
                        }
                    }
                    return consumeModel;
                }
            }
            else
            {
                return consumeModel;
            }
        }

        public static ModelOutput Predict(string input)
        {
            lock (lockObj)
            {
                var consumeModel = GetConsumeModel();
                ModelOutput result = consumeModel.Predict(input);
                return result;
            }
        }
        public static void AddSpamInfo(SpamInfo spamInfo)
        {
            using (var db=DataContext.getDataContext())
            {
                db.SpamDB.Add(spamInfo);
                db.SaveChanges();
            }

        }
    }
}