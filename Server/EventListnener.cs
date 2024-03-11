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
    public class EventListener{
        public static void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason,
            object defferals){
            new Thread(() => {
                string steamIdentifier = Utils.SearchPlayerIdentifiers(player, Utils.SteamIdentifier);
                string discordId = Utils.SearchPlayerIdentifiers(player, Utils.DiscordIdentifier);

                var embed = new EmbedBuilder()
                    .WithTitle("Connexion")
                    .WithDescription(Discord.GetFormattedDiscordMention(discordId) + " tente de se connecter !")
                    .WithColor(Color.Green)
                    .AddField("Nom Steam", player.Name, true)
                    .AddField("Steam ID", steamIdentifier, true)
                    .Build();

                Discord.PerformWebhookRequest(embed, ServerMain.Config["Connect"]);
            }).Start();
        }

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

                Discord.PerformWebhookRequest(embed, ServerMain.Config["Disconnect"]);
            }).Start();
        }

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
                Discord.PerformWebhookRequest(embed, ServerMain.Config["Death"]);
            }).Start();
        }

        public static void OnExplosion(string playerSrc, string explosionType){
            new Thread(() => {
                var embed = new EmbedBuilder()
                    .WithTitle("Explosion")
                    .AddField("Joueur", Discord.GetFormattedDiscordMention(
                        Utils.SearchPlayerIdentifiersFromSource(playerSrc, Utils.DiscordIdentifier)), true)
                    .AddField("Type d'explosion", explosionType, true)
                    .WithColor(Color.Red)
                    .Build();

                Discord.PerformWebhookRequest(embed, ServerMain.Config["Explosion"]);
            }).Start();
        }


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

                Discord.PerformWebhookRequest(embed, ServerMain.Config["Menu"]);
            }).Start();
        }
    }
}