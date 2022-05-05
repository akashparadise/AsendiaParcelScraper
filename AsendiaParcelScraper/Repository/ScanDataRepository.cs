using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AsendiaParcelScraper.Repository
{
    public class ScanDataRepository : IScanDataRepository
    {
        static JObject o1 = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\JSONs\Messages.json"));

        private readonly ILogger _logger;

        public ScanDataRepository(ILogger<ScanDataRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method will extract data from CSV files and load into a XML file
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns>true if no CSV files are available in the selected directory</returns>
        bool IScanDataRepository.LoadCsvData(string dirPath)
        {
            _logger.LogInformation((string)o1["OperationStarted"]);
            List<string> linesFromAllCSVFiles = new List<string>();

            string[] csvFiles = Directory.EnumerateFiles(dirPath, "*.csv").ToArray();

            if (csvFiles.Length < 1)
            {
                Console.WriteLine(o1["NoCSVFiles"]);
                return true;
            }

            linesFromAllCSVFiles = ReadDataFromCSVFiles(csvFiles, linesFromAllCSVFiles);

            if (linesFromAllCSVFiles.Count <= 1)
            {
                Console.WriteLine(o1["EmptyCSVFile"]);
                return true;
            }

            string[] headers = String.Concat(linesFromAllCSVFiles[0].Where(c => !Char.IsWhiteSpace(c))).ToString().Split(',').Select(x => x.Trim('\"')).ToArray();

            _logger.LogInformation((string)o1["DataConversionStarted"]);
            //converting data extracted from CSV files in XML format
            var xmlData = new XElement("Orders",
               linesFromAllCSVFiles.Where((line, index) => index > 0).Select(line => new XElement("Item",
                  line.Split(',').Select((column, index) => new XElement(headers[index], column)))));

            #region
            //converting data as per the test specifications in a required XML format
            var formattedXML =
                    new XElement("Orders",
                    from data in xmlData.Elements("Item")
                    group data by (string)data.Element(headers[0]) into groupedOrderData
                    select new XElement("Order",
                        new XAttribute("No", groupedOrderData.Key),
                        from g in groupedOrderData
                        group g by (string)g.Element(headers[1]) into groupedConsignmentData
                        select new XElement("Consignment",
                            new XAttribute("No", groupedConsignmentData.Key),
                            from h in groupedConsignmentData
                            group h by (string)h.Element(headers[2]) into groupedParcelData
                            select new XElement("Parcel",
                                new XAttribute("Code", groupedParcelData.Key),
                                from i in groupedParcelData
                                group i by (string)i.Element(headers[12]) into groupedItemData
                                select new XElement("Item",
                                    new XAttribute("Description", groupedItemData.Key),
                                    from j in groupedItemData
                                    select new XElement("Details",
                                        j.Element(headers[3]),
                                        j.Element(headers[4]),
                                        j.Element(headers[5]),
                                        j.Element(headers[6]),
                                        j.Element(headers[7]),
                                        j.Element(headers[8]),
                                        j.Element(headers[9]),
                                        j.Element(headers[10]),
                                        j.Element(headers[11]),
                                        j.Element(headers[13]).Value == "" ? new XElement(headers[13], "GBP") : j.Element(headers[13])
                                )
                            )
                        )
                    ),
                    new XElement("TotalValue", groupedOrderData.Descendants(headers[10]).Sum(x => double.Parse(x.Value))),
                    new XElement("TotalWeight", groupedOrderData.Descendants(headers[11]).Sum(x => double.Parse(x.Value)))
                )
            );
            _logger.LogInformation((string)o1["DataConversionFinished"]);
            _logger.LogInformation((string)o1["CreatingXMLStarted"]);

            formattedXML.Save(dirPath + "\\CSVDataInXMLFormat.xml");

            _logger.LogInformation((string)o1["CreatingXMLFinished"]);
            _logger.LogInformation((string)o1["OperationFinished"]);
            #endregion
            Console.WriteLine(o1["XMLFileSaved"]);
            return false;
        }

        /// <summary>
        /// Extract data from all CSV files available in the selected directory
        /// </summary>
        /// <param name="csvFiles"></param>
        /// <param name="linesFromAllCSVFiles"></param>
        /// <returns>Extracted data in form of list of string</returns>
        List<string> ReadDataFromCSVFiles(string[] csvFiles, List<string> linesFromAllCSVFiles)
        {
            _logger.LogInformation("Reading records from all the CSV files: Started");
            foreach (string path in csvFiles)
            {
                if (linesFromAllCSVFiles.Count == 0)
                {
                    linesFromAllCSVFiles.AddRange(File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1")));
                }
                else
                {
                    linesFromAllCSVFiles.AddRange(File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1")).Skip(1));
                }
            }
            _logger.LogInformation("Reading records from all the CSV files: Finished");
            return linesFromAllCSVFiles;
        }
    }
}
