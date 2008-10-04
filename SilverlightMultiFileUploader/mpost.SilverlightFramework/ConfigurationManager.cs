using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Collections.Generic;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightFramework
{
    public static class ConfigurationManager
    {
        public static AppSettings AppSettings
        {
            get;
            private set;
        }

        static ConfigurationManager()
        {
            var doc = XDocument.Load("ServiceReferences.ClientConfig");

            var dict = (from settingNode in
                            doc.Descendants("appSettings").Descendants("add")
                        select new
                        {
                            Key = settingNode.Attribute("key").Value,
                            Value = settingNode.Attribute("value").Value
                        }).ToDictionary(s => s.Key, s => s.Value);

            AppSettings = new AppSettings(dict);
        }
    }

    public class AppSettings
    {
        public string this[string key]
        {
            get
            {
                if (settings.ContainsKey(key))
                    return settings[key];
                else
                    return null;
            }
        }

        private Dictionary<string, string> settings;

        public AppSettings(Dictionary<string, string> settings)
        {
            this.settings = settings;
        }
    }
}
