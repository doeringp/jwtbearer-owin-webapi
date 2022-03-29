# Sample: JWT Bearer Authentication for an ASP.NET OWIN/Katana Web API

In the `web.config` replace in the `<appSettings>` the `authority` with your own OpenID Connect provider.

Run the web api with IIS Express.

Then you can test getting an access token and calling the api using:

```powershell
$jwt = curl -X POST "https://<authority>/connect/token" `
     -H "Content-Type: application/x-www-form-urlencoded" `
     -d ("grant_type=client_credentials&" +
         "client_id=<your-client-id>&" +
         "client_secret=<your-client-secret>&" +
         "scope=api") `
    | ConvertFrom-Json | Select -ExpandProperty access_token

curl -i -H "Authorization: Bearer $jwt" http://localhost:51862/identity
```