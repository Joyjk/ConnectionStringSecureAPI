# ConnectionStringSecureAPI

ConnectionStringSecureAPI is an ASP.NET Core Web API project designed to enhance the security of connection strings by providing encryption and decryption functionalities. It simplifies the process of securely storing and managing connection strings in the `appsettings.json` file.

## Features
- **Encrypt Connection Strings:** Securely encrypt raw connection strings and save them to `appsettings.json`.
- **Encrypt Existing Connection Strings:** Encrypt an already existing connection string in `appsettings.json`.
- **Decrypt Connection Strings:** Decrypt encrypted connection strings stored in `appsettings.json`.
- **Check Encryption Status:** Verify whether the connection string is currently encrypted or decrypted.

## Endpoints

### 1. Encrypt and Save Connection String
**URL:** `/api/ConnectionString/encrypt`  
**Method:** `POST`  
**Description:** Encrypts a raw connection string and saves it to the `appsettings.json` file.  

**Request Body:**
```json
"YourConnectionStringHere"
