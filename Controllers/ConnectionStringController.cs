using ConnectionStringSecureAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectionStringSecureAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ConnectionStringController : ControllerBase
    {
        [HttpPost("encrypt")]
        public IActionResult EncryptAndSaveConnectionString([FromBody] string rawConnectionString)
        {
            try
            {
                // Encrypt the connection string
                string encryptedConnectionString = EncryptDecryptService.Encrypt(rawConnectionString);

                // Get the appsettings.json path
                string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

                // Load and parse the existing appsettings.json
                string jsonContent = System.IO.File.ReadAllText(appSettingsPath);
                var jsonDocument = System.Text.Json.JsonDocument.Parse(jsonContent);

                // Deserialize JSON into a modifiable dictionary
                var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                var jsonObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonDocument.RootElement.ToString());

                // Ensure the "ConnectionStrings" section exists and update it
                if (!jsonObject.ContainsKey("ConnectionStrings"))
                {
                    jsonObject["ConnectionStrings"] = new Dictionary<string, string>();
                }

                var connectionStrings = jsonObject["ConnectionStrings"] as Dictionary<string, object>;
                if (connectionStrings == null)
                {
                    connectionStrings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonObject["ConnectionStrings"].ToString());
                    jsonObject["ConnectionStrings"] = connectionStrings;
                }

                connectionStrings["DefaultConnection"] = encryptedConnectionString;

                // Serialize the updated JSON and write back to file
                string updatedJson = System.Text.Json.JsonSerializer.Serialize(jsonObject, options);
                System.IO.File.WriteAllText(appSettingsPath, updatedJson);

                return Ok(new { message = "Connection string encrypted and saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("encrypt-existing")]
        public IActionResult EncryptExistingConnectionString()
        {
            try
            {
                // Get the appsettings.json path
                string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

                // Load and parse the existing appsettings.json
                string jsonContent = System.IO.File.ReadAllText(appSettingsPath);
                var jsonDocument = System.Text.Json.JsonDocument.Parse(jsonContent);

                // Deserialize JSON into a modifiable dictionary
                var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                var jsonObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonDocument.RootElement.ToString());

                // Ensure the "ConnectionStrings" section exists
                if (!jsonObject.ContainsKey("ConnectionStrings"))
                {
                    return NotFound(new { message = "ConnectionStrings section not found in appsettings.json." });
                }

                var connectionStrings = jsonObject["ConnectionStrings"] as Dictionary<string, object>;
                if (connectionStrings == null)
                {
                    connectionStrings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonObject["ConnectionStrings"].ToString());
                    jsonObject["ConnectionStrings"] = connectionStrings;
                }

                // Ensure the "DefaultConnection" exists
                if (!connectionStrings.ContainsKey("DefaultConnection"))
                {
                    return NotFound(new { message = "DefaultConnection not found in ConnectionStrings section." });
                }

                // Get the existing connection string
                string rawConnectionString = connectionStrings["DefaultConnection"].ToString();

                // Check if the connection string is already encrypted
                if (EncryptDecryptService.IsEncrypted(rawConnectionString))
                {
                    return Ok(new { message = "Connection string is already encrypted." });
                }

                // Encrypt the connection string
                string encryptedConnectionString = EncryptDecryptService.Encrypt(rawConnectionString);

                // Update the connection string with the encrypted value
                connectionStrings["DefaultConnection"] = encryptedConnectionString;

                // Serialize the updated JSON and write back to file
                string updatedJson = System.Text.Json.JsonSerializer.Serialize(jsonObject, options);
                System.IO.File.WriteAllText(appSettingsPath, updatedJson);

                return Ok(new { message = "Connection string encrypted and saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("decrypt")]
        public IActionResult DecryptAndSaveConnectionString()
        {
            try
            {
                // Get the appsettings.json path
                string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

                // Load the appsettings.json content
                if (!System.IO.File.Exists(appSettingsPath))
                {
                    return NotFound(new { message = "appsettings.json file not found." });
                }

                string jsonContent = System.IO.File.ReadAllText(appSettingsPath);

                // Parse and deserialize JSON
                var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                var jsonObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent);

                if (jsonObject == null || !jsonObject.ContainsKey("ConnectionStrings"))
                {
                    return NotFound(new { message = "ConnectionStrings section not found in appsettings.json." });
                }

                var connectionStrings = jsonObject["ConnectionStrings"] as Dictionary<string, object>;
                if (connectionStrings == null)
                {
                    connectionStrings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonObject["ConnectionStrings"].ToString());
                    jsonObject["ConnectionStrings"] = connectionStrings;
                }

                if (!connectionStrings.ContainsKey("DefaultConnection"))
                {
                    return NotFound(new { message = "DefaultConnection not found in ConnectionStrings section." });
                }

                // Get the existing encrypted connection string
                string encryptedConnectionString = connectionStrings["DefaultConnection"].ToString();

                // Check if the connection string is already decrypted
                if (!EncryptDecryptService.IsEncrypted(encryptedConnectionString))
                {
                    return Ok(new { message = "Connection string is already decrypted." });
                }

                // Decrypt the connection string
                string decryptedConnectionString = EncryptDecryptService.Decrypt(encryptedConnectionString);

                // Update the connection string with the decrypted value
                connectionStrings["DefaultConnection"] = decryptedConnectionString;

                // Serialize and write the updated JSON back to appsettings.json
                string updatedJson = System.Text.Json.JsonSerializer.Serialize(jsonObject, options);
                System.IO.File.WriteAllText(appSettingsPath, updatedJson);

                return Ok(new { message = "Connection string decrypted and saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("check-connection-status")]
        public IActionResult CheckConnectionStringStatus()
        {
            try
            {
                // Get the appsettings.json path
                string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

                // Load and parse the appsettings.json file
                string jsonContent = System.IO.File.ReadAllText(appSettingsPath);
                var jsonDocument = System.Text.Json.JsonDocument.Parse(jsonContent);

                // Check if ConnectionStrings:DefaultConnection exists
                if (jsonDocument.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings) &&
                    connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection))
                {
                    string connectionString = defaultConnection.GetString();

                    // Check if the connection string is encrypted
                    bool isEncrypted = EncryptDecryptService.IsEncrypted(connectionString); // Custom logic to determine encryption status

                    if (isEncrypted)
                    {
                        return Ok(new { message = "Connection Encrypted" });
                    }
                    else
                    {
                        return Ok(new { message = "Connection Decrypted" });
                    }
                }

                return NotFound(new { message = "DefaultConnection not found in appsettings.json" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
