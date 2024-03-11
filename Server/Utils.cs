using CitizenFX.Core;
using CitizenFX.Core.Native;
using Debug = System.Diagnostics.Debug;

namespace DiscordLoggerv2.Server{

    public class Utils{
        public static readonly string DiscordIdentifier = "discord";
        public static readonly string SteamIdentifier = "steam";
        public static readonly string LicenseIdentifier = "license";
        public static readonly string XblIdentifier = "xbl";
        public static readonly string LiveIdentifier = "live";
        public static readonly string IpIdentifier = "ip";
        
        
        public static string SearchPlayerIdentifiers(Player player, string identifierType){
            if (player == null){
                return "None";
            }
            foreach (var identifier in player.Identifiers){
                if (identifier.StartsWith(identifierType)){
                    return identifier.Split(':')[1];
                }
            }

            return "None";
        }
        
        public static string SearchPlayerIdentifiersFromSource(string source, string identifierType){
            string identifier = API.GetPlayerIdentifierByType(source, identifierType);
            if (identifier != null){
                return identifier.Split(':')[1];
            }
            return "None";
        }
    }
}