using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Discord;
using Discord.Webhook;

namespace DiscordLoggerv2.Server{
    /// <summary>
    /// EventListener class, all the events that we want to listen to are here
    /// All will be started in a Thread to not add latency for the main Thread in FiveM
    /// </summary>
    public class EventListener{
        
        /// <summary>
        /// Function that Handle the OnPlayerConnecting Event
        /// </summary>
        /// <param name="player">Player object (form the Source object that fivem updated</param>
        /// <param name="playerName">String name</param>
        /// <param name="setKickReason">Function to call if we want to cancel the event</param>
        /// <param name="defferals">Represent the actions we can do with the player</param>
        public static void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason,
            object defferals){
            new Thread(() => {
                string steamIdentifier = Utils.SearchPlayerIdentifiers(player, Utils.SteamIdentifier);
                string discordId = Utils.SearchPlayerIdentifiers(player, Utils.DiscordIdentifier);

                // EmbedBuilder that return an Embed object to send in the request
                var embed = new EmbedBuilder()
                    .WithTitle("Connexion")
                    .WithDescription(Discord.GetFormattedDiscordMention(discordId) + " tente de se connecter !")
                    .WithColor(Color.Green)
                    .AddField("Nom Steam", player.Name, true)
                    .AddField("Steam ID", steamIdentifier, true)
                    .Build();

                Discord.PerformWebhookRequest(embed, ServerMain.ConfigWebhook["Connect"]);
            }).Start();
        }

        /// <summary>
        /// Function that handle when a player disconnect
        /// </summary>
        /// <param name="player">Player object (form the Source object that fivem updated</param>
        /// <param name="reason">Reason why the player left</param>
        public static void OnPlayerDropped([FromSource] Player player, string reason){
            new Thread(() => {
                var embed = new EmbedBuilder()
                    .WithTitle("Déconnexion")
                    .WithDescription(
                        Discord.GetFormattedDiscordMention(
                            Utils.SearchPlayerIdentifiers(player, Utils.DiscordIdentifier)) +
                        " s'est déconnecté !")
                    .WithColor(Color.Red)
                    .AddField("Raison", reason, true)
                    .Build();

                Discord.PerformWebhookRequest(embed, ServerMain.ConfigWebhook["Disconnect"]);
            }).Start();
        }

        /// <summary>
        /// Function that handle when a player die
        /// This event is called from tx first in monitor/sv_logger.lua
        /// I added another call to my function to handle the death
        /// </summary>
        /// <param name="player">Player object (form the Source object that fivem updated</param>
        /// <param name="killerSrc">Killer id as a string (if there was) (can't have it from Source)</param>
        /// <param name="cause">The cause of the death</param>
        public static void OnPlayerDeath([FromSource] Player player, string killerSrc, string cause){
            new Thread(() => {
                var embed = new EmbedBuilder()
                    .WithTitle("Mort")
                    .WithDescription(
                        Discord.GetFormattedDiscordMention(
                            Utils.SearchPlayerIdentifiers(player, Utils.DiscordIdentifier)) +
                        " est mort !")
                    .WithColor(Color.Red)
                    .AddField("Tueur",
                        (killerSrc == "False"
                            ? "None"
                            : Discord.GetFormattedDiscordMention(
                                Utils.SearchPlayerIdentifiersFromSource(killerSrc, Utils.DiscordIdentifier))), true)
                    .AddField("Cause", cause, true)
                    .Build();
                Discord.PerformWebhookRequest(embed, ServerMain.ConfigWebhook["Death"]);
            }).Start();
        }

        /// <summary>
        /// Function that handle when an explosion occure in the server
        /// </summary>
        /// <param name="playerSrc">Id of the player as a string (can't have it from source)</param>
        /// <param name="explosionType">Type of the explosion as a string (Like PETROL_PUMP for a Gas Station)</param>
        public static void OnExplosion(string playerSrc, string explosionType){
            new Thread(() => {
                var embed = new EmbedBuilder()
                    .WithTitle("Explosion")
                    .AddField("Joueur", Discord.GetFormattedDiscordMention(
                        Utils.SearchPlayerIdentifiersFromSource(playerSrc, Utils.DiscordIdentifier)), true)
                    .AddField("Type d'explosion", explosionType, true)
                    .WithColor(Color.Red)
                    .Build();

                Discord.PerformWebhookRequest(embed, ServerMain.ConfigWebhook["Explosion"]);
            }).Start();
        }


        /// <summary>
        /// Function that handle the event when a staff use the menu
        /// </summary>
        /// <param name="source">Id of the player that perform the action (can't have it from Source)</param>
        /// <param name="action">The type of action the player performed</param>
        /// <param name="data">The details of the action</param>
        public static void OnMenuAction(string source, string action, string data){
            new Thread(() => {
                var embed = new EmbedBuilder()
                    .WithTitle("Menu")
                    .WithDescription(
                        Discord.GetFormattedDiscordMention(
                            Utils.SearchPlayerIdentifiersFromSource(source, Utils.DiscordIdentifier)))
                    .WithColor(Color.Blue)
                    .AddField("Action", action, true)
                    .AddField("Data", data, true)
                    .Build();

                Discord.PerformWebhookRequest(embed, ServerMain.ConfigWebhook["Menu"]);
            }).Start();
        }
    }
}