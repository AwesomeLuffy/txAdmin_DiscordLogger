using System;
using System.Collections.Generic;
using System.Net;
using CitizenFX.Core;
using Discord;
using Discord.Webhook;

namespace DiscordLoggerv2.Server
{
    public class Discord
    {
        /// <summary>
        /// Function to get a formatted Discord mention like <@ID>
        /// </summary>
        /// <param name="id">The Discord id to format</param>
        /// <returns>The fromatted String</returns>
        public static string GetFormattedDiscordMention(string id)
        {
            return "<@" + id + ">";
        }
        
        /// <summary>
        /// Function to Perform a Webhook request to a Discord channel with Discord.Net.Webhook
        /// </summary>
        /// <param name="embed">The embed objet to send</param>
        /// <param name="webhookUrl">The url of the Webhook</param>
        public static void PerformWebhookRequest(Embed embed, string webhookUrl){
            try{
                // Create the client to the Webhook
                DiscordWebhookClient dwc = new DiscordWebhookClient(webhookUrl);
                // Send the message
                dwc.SendMessageAsync(embeds: new Embed[]{ embed });
            }
            catch (Exception e){
                if (ServerMain.DebugMode){
                    Debug.WriteLine(e.ToString());
                }

                Debug.WriteLine("Impossible d'envoyer le message sur le webhook Discord.");
            }
        }
    }
}