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
        public static string GetFormattedDiscordMention(string id)
        {
            return "<@" + id + ">";
        }
        public static void PerformWebhookRequest(Embed embed, string webhookUrl){
            try{
                DiscordWebhookClient dwc = new DiscordWebhookClient(webhookUrl);
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