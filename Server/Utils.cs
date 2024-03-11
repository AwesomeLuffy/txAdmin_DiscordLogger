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
        
        /// <summary>
        /// Function to search for a specific identifier for a player
        /// </summary>
        /// <param name="player">The player we want to search </param>
        /// <param name="identifierType">The type of identifier, use the predefined inside the Utils class</param>
        /// <returns>The identifier or None if not found</returns>
        public static string SearchPlayerIdentifiers(Player player, string identifierType){
            if (player == null || player.Identifiers == null){
                return "None";
            }
            foreach (var identifier in player.Identifiers){
                if (identifier.StartsWith(identifierType)){
                    return identifier.Split(':')[1];
                }
            }

            return "None";
        }
        
        /// <summary>
        /// Function to search for a specific identifier for a player but we use the source id instead of the player object
        /// </summary>
        /// <param name="source">Id of the player</param>
        /// <param name="identifierType">The type of identifier, use the predefined inside the Utils class</param>
        /// <returns>The identifier or None if not found</returns>
        public static string SearchPlayerIdentifiersFromSource(string source, string identifierType){
            string identifier = API.GetPlayerIdentifierByType(source, identifierType);
            if (identifier != null){
                return identifier.Split(':')[1];
            }
            return "None";
        }
    }
}