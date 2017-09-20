# ASP.NET Core 2.0 and Ubisecure SSO integration with OpenID Connect

## Discovery and client registration

### openid-configuration.json

The OpenID Connect Provider metadata of your Ubisecure SSO Server. Provider metadata is published at a well known URI derived from the name of the Provider by concatenating **/.well-known/openid-configuration**. For example, if the name of your Ubisecure SSO Server is **https://sso.example.com/uas** then the URI for Provider metadata is

https://sso.example.com/uas/.well-known/openid-configuration

### client-config.json

The OpenID Connect Client metadata. This file is generated by Ubisecure SSO Server at client registration time. You may use Ubisecure SSO Management Console or Management API for client registration.

### Managemet API and PowerShell automation

Run `sso-register-client.ps1` to create **openid-configuration.json** and **client-config.json**

## Code review

Most code files are as generated by the Visual Studio 2017 ASP.NET Core wizard. Only two files were modified for this integration

* Startup.cs
* Views/Home/Index.cshtml

### Startup.cs

TODO

### Index.cshtml

TODO

## Launching

Use Visual Studio 2017 to launch AspNetCoreSample application on http://localhost:19282

## References

* http://openid.net/specs/openid-connect-discovery-1_0.html
* http://openid.net/specs/openid-connect-registration-1_0.html
* http://openid.net/specs/openid-connect-core-1_0.html
