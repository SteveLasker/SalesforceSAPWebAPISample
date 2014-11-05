using Salesforce.Common;
using Salesforce.Force;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoRepairManager.Salesforce
{
    public class SalesforceService
    {
        public SalesforceService()
        {
            this.AuthenticationClient = new AuthenticationClient();
        }

        public AuthenticationClient AuthenticationClient { get; private set; }

        public async Task<ForceClient> GetUsernamePasswordForceClient()
        {
            await this.AuthenticationClient.UsernamePasswordAsync(
#error enter the values in web.config for your Salesforce Connected App
                SalesforceService.GetAppSetting("Salesforce:ConsumerKey"),
                SalesforceService.GetAppSetting("Salesforce:ConsumerSecret"),
                SalesforceService.GetAppSetting("Salesforce:Username"),
                SalesforceService.GetAppSetting("Salesforce:Password") + SalesforceService.GetAppSetting("Salesforce:SecurityToken"));
 
            return new ForceClient(
                this.AuthenticationClient.InstanceUrl,
                this.AuthenticationClient.AccessToken,
                this.AuthenticationClient.ApiVersion);
        }



        /// <summary>
        /// Checks if the user is currently authenticated with a Salesforce AccessToken.
        /// </summary>
        public static bool IsAuthenticated()
        {
            throw new InvalidOperationException("IsAuthenticated not implemented for Authentication flows beyond Web Server Flow at this time");
        }

        internal static string GetAppSetting(string name)
        {
            string setting = ConfigurationManager.AppSettings[name];
            if (String.IsNullOrWhiteSpace(setting) || setting == "RequiredValue")
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "The value for the '{0}' key is missing from the appSettings section of the config file.", name));
            }

            return setting;
        }
    }
}