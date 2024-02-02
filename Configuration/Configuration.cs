﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SmartLogger
{
    [XmlRoot("Configuration")]
    public class Configuration
    {
        private bool enabled = true;
        private string appName = PlainAppFilename();
        private string resolution = "Standard";
        private Level sessionDefaults = Level.Debug;
        private Level defaultLevel = Level.Warning;
        private int majorVersion = 1;
        private int minorVersion = 6;
        private int maintVersion = 0;

        [XmlElement("MajorVersion")]
        public int MajorVersion
        {
            get => majorVersion;
            set => majorVersion = value;
        }
        [XmlElement("MinorVersion")]
        public int MinorVersion
        {
            get => minorVersion;
            set => minorVersion = value;
        }
        [XmlElement("MaintVersion")]
        public int MaintVersion
        {
            get => maintVersion;
            set => maintVersion = value;
        }
        [XmlElement("Enabled")]
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }
        [XmlElement("AppName")]
        public string AppName
        {
            get => appName;
            set => appName = value;
        }
        [XmlElement("Resolution")]
        public string Resolution
        {
            get => resolution;
            set => resolution = value;
        }
        [XmlElement("SessionDefaults")]
        public Level SessionDefaults
        {
            get => sessionDefaults;
            set => sessionDefaults = value;
        }
        [XmlElement("DefaultLevel")]
        public Level DefaultLevel
        {
            get => defaultLevel;
            set => defaultLevel = value;
        }

        [XmlArray("Areas")]
        [XmlArrayItem("Area")]
        public List<Area> Areas;

        [XmlArray("Connections")]
        [XmlArrayItem("Connection")]
        public List<Connection> Connections;

        [XmlArray("Rules")]
        [XmlArrayItem("Rule")]
        public List<Rule> Rules;

        public Configuration()
        {
            Bootstrap.Log.LogMessage("Configuration created");
            Connections = new List<Connection>();
            Areas = new List<Area>();
            Rules = new List<Rule>();
        }
        public Configuration(bool production)
        {
            Bootstrap.Log.LogMessage("Default({production}) Configuration created");
            Connections = new List<Connection>();
            Areas = new List<Area>();
            Rules = new List<Rule>();
            DefaultConfiguration(production);
        }

        public static string PlainAppFilename()
        {
            string runningApplication = Process.GetCurrentProcess().MainModule.FileName;
            return System.IO.Path.GetFileNameWithoutExtension(runningApplication);

        }


        public Rule FindRuleByName(string name)
        {
            foreach (Rule rule in Rules)
            {
                if (name.ToLower() == rule.Name.ToLower())
                {
                    return rule;
                }
            }
            return null;
        }
        public static Configuration LoadConfiguration(string configurationFilename)
        {
            Bootstrap.Log.EnterMethod($"LoadConfiguration: '{configurationFilename}'");
            Configuration result = null;
            bool production = !configurationFilename.ToLower().Contains("polyworks");
            bool exists = !string.IsNullOrEmpty(configurationFilename) || File.Exists(configurationFilename);
            if (exists)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    using (FileStream fileStream = new FileStream(configurationFilename, FileMode.Open))
                    {
                        result = (Configuration)serializer.Deserialize(fileStream);
                        Bootstrap.Log.LogColored(Gurock.SmartInspect.Level.Debug, Color.BurlyWood, result.ConfigurationString());
                    }
                }
                catch (Exception e)
                {
                    Bootstrap.Log.LogException(e);
                    Bootstrap.Log.LogWarning($"The configuration file {configurationFilename} could not be parsed. The default configuration is being used.");
                    result = new Configuration(production); 
                }
            }
            else
            {
                Bootstrap.Log.LogWarning($"The configuration file {configurationFilename} was not found. The default configuration is being used.");
                result = new Configuration(production); 
            }
            Bootstrap.Log.LeaveMethod("LoadConfiguration");
            return result;
        }
        public void SaveToFile(string filename)
        {
            try
            {
                XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
                StringWriter xml = new StringWriter();
                var utf8NoBom = new UTF8Encoding(false);
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.Encoding = utf8NoBom;
                var xmlWriter = XmlWriter.Create(xml, settings);
                xmlSerializer.Serialize(xmlWriter, this);
                File.WriteAllText(filename, xml.ToString());

            }
            catch (Exception ex)
            {
                Bootstrap.Log.LogException($"Error trying to save configuration file {filename}.", ex);
            }

        }

        private static string DefaultLogFilePath()
        {
            string companyName = "3D Infotech";
            string runningApplication = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string filename = System.IO.Path.GetFileNameWithoutExtension(runningApplication);
            string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return $"{dataPath}\\{companyName}\\{filename}\\logs";
        }

        public void AddDefaultRules(bool production)
        {
            Rule rule;
            RuleAreaItem item;

            rule = new Rule();
            rule.Name = "Production";
            rule.Enabled = true;
            rule.Priority = 10;
            item = new RuleAreaItem();
            item.Mask = "*";
            item.Enabled = production;
            item.LoggingLevel = Level.Warning;
            item.DetailLevel = Level.Off;
            rule.RuleAreaItems.Add(item);
            Rules.Add(rule);

            rule = new Rule();
            rule.Name = "Diagnose";
            rule.Enabled = false;
            rule.Priority = 15;
            item = new RuleAreaItem();
            item.Mask = "*";
            item.Enabled = true;
            item.LoggingLevel = Level.Warning;
            item.DetailLevel = Level.Standby;
            rule.RuleAreaItems.Add(item);
            Rules.Add(rule);

            rule = new Rule();
            rule.Name = "Developer";
            rule.Enabled = false;
            rule.Priority = 15;
            item = new RuleAreaItem();
            item.Mask = "*";
            item.Enabled = !production;
            item.LoggingLevel = Level.Warning;
            item.DetailLevel = Level.Debug;
            rule.RuleAreaItems.Add(item);
            Rules.Add(rule);



        }

        public void DefaultConfiguration(bool production = true)
        {

            Enabled = true;
            DefaultLevel = Level.Debug;
            SessionDefaults = Level.Debug;
            Resolution = "Standard";
            AppName = PlainAppFilename();
            Connections.Clear();
            Areas.Clear();
            Rules.Clear();
            AddDefaultConnectons();
            AddDefaultRules(production);
        }
        public void AddDefaultConnectons()
        {

            string path = DefaultLogFilePath();
            Bootstrap.Log.LogString("DefaultLogFilePath", path);
            Connection connection;
            connection = new Connection();
            connection.Enabled = false;
            connection.ConnectionType = ConnectionTypes.File;
            connection.FilePath = path;
            connection.FileName = "DetailedLog.sil";
            connection.AppendInsteadOfOverride = true;
            connection.MaximumLogCount = 35;
            connection.MaximumLogSize = 500000;
            connection.Rotate = LogRotate.Hourly;
            connection.LogCaption = "Details";
            connection.AutomaticallyReconnect = true;
            connection.LogLevel = Level.Debug;
            Connections.Add(connection);

            connection = new Connection();
            connection.ConnectionType = ConnectionTypes.File;
            connection.Enabled = true;
            connection.FilePath = path;
            connection.FileName = "IssuesLog.sil";
            connection.AppendInsteadOfOverride = true;
            connection.MaximumLogCount = 6;
            connection.MaximumLogSize = 500000;
            connection.Rotate = LogRotate.Weekly;
            connection.LogCaption = "Issues";
            connection.AutomaticallyReconnect = true;
            connection.LogLevel = Level.Warning;
            Connections.Add(connection);

            connection = new Connection();
            connection.ConnectionType = ConnectionTypes.Text;
            connection.Enabled = false;
            connection.FilePath = path;
            connection.FileName = "IssuesLog.txt";
            connection.AppendInsteadOfOverride = true;
            connection.MaximumLogCount = 15;
            connection.MaximumLogSize = 500000;
            connection.Rotate = LogRotate.Monthly;
            connection.LogCaption = "Errors";
            connection.AutomaticallyReconnect = true;
            connection.LogLevel = Level.Error;
            Connections.Add(connection);

            connection = new Connection();
            connection.ConnectionType = ConnectionTypes.Pipe;
            connection.Enabled = true;
            connection.LogCaption = "smartinspect";
            connection.AutomaticallyReconnect = true;
            connection.LogLevel = Level.Debug;
            Connections.Add(connection);


        }
        public string ConfigurationString(bool forcePaths = true)
        {
            //DefaultConfiguration();
            if (Connections.Count == 0)
            {
                return "pipe(caption=smartinspect, reconnect=true, level=debug)";

            }
            string fragment = "";
            foreach (Connection connection in Connections)
            {
                if (!connection.Enabled)
                {
                    continue;
                }
                fragment += connection.ConnectionString() + ", ";
                if (forcePaths && ((connection.ConnectionType == ConnectionTypes.File) || (connection.ConnectionType == ConnectionTypes.Text)))
                {
                    Directory.CreateDirectory(connection.FilePath);
                }
            }
            return fragment.Substring(0, fragment.Length - 2); // remove last comma

        }

    }

}
