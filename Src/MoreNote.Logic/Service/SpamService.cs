using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;

using SpamClassificationML.Model;

using System;

namespace MoreNote.Logic.Service
{
    public class SpamService
    {
        private DataContext dataContext;
        public ConfigFileService ConfigFileService { get; set; }

        public SpamService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        private static ConsumeModel consumeModel = null;

        private static Object lockObj = new object();

        private ConsumeModel GetConsumeModel()
        {
            WebSiteConfig webSiteConfig = ConfigFileService.WebConfig;
            if (consumeModel == null)
            {
                lock (lockObj)
                {
                    if (consumeModel == null)
                    {
                        consumeModel = new ConsumeModel();
                        if (consumeModel.predEngine == null)
                        {
                            consumeModel.loadModel(webSiteConfig.MachineLearning.SpamModelPath);
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

        public ModelOutput Predict(string input)
        {
            lock (lockObj)
            {
                var consumeModel = GetConsumeModel();
                ModelOutput result = consumeModel.Predict(input);
                return result;
            }
        }

        public void AddSpamInfo(SpamInfo spamInfo)
        {
            dataContext.SpamInfo.Add(spamInfo);
            dataContext.SaveChanges();
        }
    }
}