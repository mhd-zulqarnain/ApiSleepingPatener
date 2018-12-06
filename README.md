# Apisleepingpatener

Follow the link
http://www.advancesharp.com/blog/1216/oauth-web-api-token-based-authentication-with-custom-database

Install packages
Install-Package Microsoft.AspNet.Mvc -Version 4.0.20710.0 -ProjectName XXXXX

Install-Package Microsoft.AspNet.Web.Optimization

Install-Package Microsoft.AspNet.Identity.EntityFramework -Version 2.2.2

Update-Package -Reinstall


show error

<system.web>
    <customErrors mode="Off"/>
</system.web>
<system.webServer>
    <httpErrors errorMode="Detailed" />
</system.webServer>

After adding new entity
Need to create project from scratch
-add libraries
-copy the controller and model 
-add the entitiees
-add mising frameworks

# If the error cause "SleepingPartnermanagementTestingEntities" already been added publishing .net
After the <connectionString> and before the first <add....> node, add <clear/>
