using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace DiscordLoggerv2.Server
{
    public class ServerMain : BaseScript
    {
        
        public static bool DebugMode = true;
        
        public static Dictionary<string, string> Config;

        public ServerMain()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate{ return true; };

            if (!LoadConfigFile())
            {
                Debug.WriteLine("Can't load config file, plugin not load.");
                return;
            }

            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(EventListener.OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Player, string>(EventListener.OnPlayerDropped);
            EventHandlers["txsv:logger:deathEvent"] += new Action<Player, string, string>(EventListener.OnPlayerDeath);
            EventHandlers["DiscordLogger:OnTxMenuAction"] += new Action<string, string, string>(EventListener.OnMenuAction);
            EventHandlers["DiscordLogger:OnExplosionEvent"] += new Action<string, string>(EventListener.OnExplosion);
            
            Debug.WriteLine("Hi from DiscordLoggerv2.Server!");
        }

        private static bool LoadConfigFile()
        {
            try
            {
                var configXmlString = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, "DiscordLoggerv2", "config.xml");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(configXmlString);
                XmlNodeList resources = doc.SelectNodes("DiscordLoggerConfig/webhook");

                if (resources == null)
                {
                    return false;
                }

                Config = new Dictionary<string, string>();

                foreach (XmlNode node in resources)
                {
                    if (node.Attributes == null || node.Attributes["name"] == null)
                    {
                        return false;
                    }
                    Config.Add(node.Attributes["name"].Value, node.InnerText);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                Debug.WriteLine("Probably incorrect config file !");
                return false;
            } 
            return true;
        }
    }
}