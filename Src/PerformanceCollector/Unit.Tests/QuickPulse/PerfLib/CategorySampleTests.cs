﻿namespace Unit.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CategorySampleTests
    {
        [TestMethod]
        public void CategorySampleReadsDataCorrectly()
        {
            // ARRANGE
            var dataList = new List<byte>();
            string resourceName = "Unit.Tests.QuickPulse.PerfLib.PerfData.data";

            Stream stream = null;
            try
            {
                stream = typeof(CategorySampleTests).Assembly.GetManifestResourceStream(resourceName);

                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    stream = null;

                    dataList.AddRange(reader.ReadToEnd().Split(',').Select(byte.Parse));
                }
            }
            finally
            {
                stream?.Dispose();
            }

            byte[] data = dataList.ToArray();

            var perfLib = PerfLib.GetPerfLib();

            // ACT
            var categorySample = new CategorySample(data, 230, 6, perfLib);

            // ASSERT
            Assert.AreEqual(28, categorySample.CounterTable.Count);
            Assert.AreEqual(165, categorySample.InstanceNameTable.Count);
            Assert.AreEqual(6, categorySample.CounterTable.First().Key);
            Assert.AreEqual("Idle", categorySample.InstanceNameTable.First().Key);
        }
    }
}