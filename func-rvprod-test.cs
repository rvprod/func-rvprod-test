using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace rvprod
{
    public class func_rvprod_test
    {
        [FunctionName("func_rvprod_test")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            string secretName = "MySecret";
            string secretValue = "";
            var keyVaultUri = System.Environment.GetEnvironmentVariable("KEY_VAULT_URI");
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUri), credential);
            try
            {
                KeyVaultSecret secret = await client.GetSecretAsync(secretName);
                secretValue = secret.Value;
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to retrieve the secret: {ex.Message}");
            }

            log.LogInformation($"Retrieve secret: {secretValue}");
        }
    }
}
