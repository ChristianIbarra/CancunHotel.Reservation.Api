To Run the API.

- Pull the repository.
- If using Visual Studio: Open package manager console and run Update-Database.
- If using VS Code: Open a command line in the project folder and run dotnet ef database update.

API is using swagger to expose the endpoint schemas and also allows you to try out endpoints.
To open swagger: run the application and navigate to the localhost address (plus selected port) /swagger

Reservation Controller is the only one protected with authorize attribute.
To use them:
- First user the corresponding login/register endpoint.
- You will get an authorization token.
- In the swagger UI you will find an Authorize Button at the top right corner, click it and add the token you got plus the bearer prefix. I.E. bearer 8sgdkasjdgisd9sgdausb9.
- Click authorize.
- You should be able to use the protected endpoints now.
