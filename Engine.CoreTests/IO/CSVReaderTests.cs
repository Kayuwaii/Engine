using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core.Utils;

namespace Engine.Core.IO.Tests
{
	[TestClass()]
	public class CSVReaderTests
	{

		[TestMethod()]
		public void ReadCsvTestLines()
		{
			string filePath = ".\\IO\\TESTCSV.csv";

			byte[] bytes = File.ReadAllBytes(filePath);

			string csvString = Encoding.UTF8.GetString(bytes);
			string[] lines = csvString.Split(csvString.GetNewlineSeparator()).Where(x => x != "").ToArray();

			Assert.IsInstanceOfType(CSVReader.ReadCsv<TestCSVItem>(lines), typeof(ICollection<TestCSVItem>));
		}

		[TestMethod()]
		public void ReadCsvTestFile()
		{
			string filePath = ".\\IO\\TESTCSV.csv";

			byte[] bytes = File.ReadAllBytes(filePath);

			Assert.IsInstanceOfType(CSVReader.ReadCsv<TestCSVItem>(bytes), typeof(ICollection<TestCSVItem>));
		}
	}

	internal class TestCSVItem
	{
		public string Col1 { get; set; }
		public double Col2 { get; set; }
		public DateTime Col3 { get; set; }
		public TimeSpan Col4 { get; set; }
		public bool Col5 { get; set; }
	}
}