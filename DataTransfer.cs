using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Linq;

namespace DuckWalk
{
    public class DataTransfer
    {
        string settingsFilePath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\settings.xml";

        /// <summary>
        /// Create the settings file.
        /// </summary>
        public void CreateXmlFile()
        {

            if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory))
            { 

                if (!File.Exists(settingsFilePath))
                {
                    File.Create(settingsFilePath).Close();

                    XmlWriterSettings wSet = new XmlWriterSettings();
                    wSet.Indent = true;

                    XmlWriter xmlWriter = XmlWriter.Create(settingsFilePath, wSet);
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("settings");
                    xmlWriter.WriteStartElement("searchengines");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush();
                    xmlWriter.Close();

                    Process.Start(settingsFilePath);
                }
            }
        }

        public bool AddSearchEngine(string name, string prefix)
        {
            var doc = Doc();

            var searchEngines = doc.GetElementsByTagName("searchengines");
            var el = searchEngines[0].AppendChild(doc.CreateElement("engine"));
            var at1 = el.Attributes.Append(doc.CreateAttribute("name"));
            var at2 = el.Attributes.Append(doc.CreateAttribute("prefix"));
            at1.InnerText = name; 
            at2.InnerText = prefix;

            var children = searchEngines[0].ChildNodes;
            var exists = children.Cast<XmlNode>().Any(x => x.Name == at1.InnerText && x.Value == at2.InnerText);

            if (!exists)
            {
                SaveXmlDocument(doc);
                return true;
            }

            return false;
        }

        public List<string> GetSearchEngines()
        {
            List<string> list = new List<string>();
            var doc = Doc();

            var searchEngines = doc.GetElementsByTagName("engine");
            searchEngines.Cast<XmlNode>().ToList().ForEach(x => list.Add(x.Attributes.GetNamedItem("name").InnerText));

            return list;

        }


        private XmlDocument Doc()
        {
            XmlDocument document = new XmlDocument();
            document.Load(settingsFilePath);
            return document;
        }

        private void SaveXmlDocument(XmlDocument doc)
        {
            doc.Save(settingsFilePath);
        }
    }
}
