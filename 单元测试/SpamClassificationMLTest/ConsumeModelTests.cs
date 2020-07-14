using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpamClassificationML.Model;

using System;
using System.Collections.Generic;
using System.Text;
using SpamClassificationML.Model;

namespace SpamClassificationML.Model.Tests
{
    [TestClass()]
    public class ConsumeModelTests
    {
        [TestMethod()]
        public void PredictTest()
        {
            // Add input data
            var input = new ModelInput();
            input.Value = "商业秘密的秘密性那是维系其商业价值和垄断地位的前提条件之一";
            // Load model and predict output of sample data
            ModelOutput result = ConsumeModel.Predict(@"E:\服务器\MLModel.zip", input);
            Console.WriteLine(result.Prediction);
            Console.WriteLine(result.Score);

        }
    }
}