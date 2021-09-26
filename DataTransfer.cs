using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

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
    }
}
