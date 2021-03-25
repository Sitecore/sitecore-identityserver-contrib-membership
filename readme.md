# Sitecore.IdentityServer4.Contrib.Membership

[![Build status](https://ci.appveyor.com/api/projects/status/4ifi93bfr7rl9p3l/branch/develop?svg=true)](https://ci.appveyor.com/project/sc-alexandernaumchenkov/sitecore-identityserver-contrib-membership/branch/develop)

## ASP.NET 2.0 Membership Database as Identity Server User Store
Identity Server is a framework and doesn't provide implementations of user data sources out of the box.
If you have an existing ASP.NET 2.0 Membership Database containing user data for existing systems then you can install the following package:

```powershell
PM> Install-Package Sitecore.IdentityServer4.Contrib.Membership
```

To add the plugin, add the following to the OWIN startup class of your IdentityServer instance:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    var builder = services.AddIdentityServer(options => { })
        ...
        .AddMembershipService(new MembershipOptions
        {
            ConnectionString = "...",   // Membership database connection string
            ApplicationName = "..."     // Membership Application Name
        })
        ...
}
```

This will validate user logins and passwords against an existing database.  No support is provided for maintaining users and it is not recommended that you use this for a new implementation. 
IdentityServer provides a [plugin](https://github.com/IdentityServer/IdentityServer4.AspNetIdentity) that supports [ASP.NET Identity](http://www.asp.net/identity).
