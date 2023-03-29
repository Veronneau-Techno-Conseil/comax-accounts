# Commun Axiom <img src="CommunAxiom.png" style="height: 1em" /> - Accounts application <img src="Profiles.png" style="height: 1em" /> 

### The data collaboration software ecosystem at the center of the Commun Axiom software suite.
Hosted at https://accounts.communaxiom.org

This projects hold most of the logic backing the authentication and authorization infrastructure for Commun Axiom. You can find the documentation at [https://communaxiom.org/fr/profils](https://communaxiom.org/fr/profils). The project is subdivided into two different applications. Accounts is the UI interface and also exposes the handlers associated to OIDC. It provides claims for any authenticated account on the application as well as registered applications passing ClientId / Secret.

## How to run

Both will require the following:
- Configure the connection string to point to a live postgres instance and database or flip to memorydb for an quick test. Keep in mind that memorydb will be erased when the application is shut down.
- Ensure that, aside from the connection string, the other configs are as follows:
   ```
   "DbConfig": {
    "MemoryDb": false,
    "ConnectionString": "Host=dbsrv.cluster.local;Port=5432;Database=accounts;Username=accountsdbsa;Password=rhrththg4re;",
    "ShouldDrop": false,
    "ShouldMigrate": true
  },
  ```
 - For central, ensure you configure your oidc settings to connect to a live instance of Accounts  
 - Run using wither your preferred debugger or using `dotnet run` command on the folder of the application to run.
