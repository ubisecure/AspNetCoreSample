Import-Module "oauth2"
Import-Module "sso-api-v2"

$sso = "https://sso.example.com:8443"

New-OAuthClientConfig -Name "bootstrap.json" | New-SSOLogon -Uri $sso -Code:$false -Username "system"

Get-OAuthMetadata -Authority "$sso/uas" | ConvertTo-Json | Set-Content -Path "openid-configuration.json" -Force

$password1 = Get-SSOObject -Type "method" "password.1" -ErrorAction Stop
$allusers = Get-SSOObject -Type "group" "System","Authenticated Users" -ErrorAction Stop

$site = Set-SSOObject -Type "site" "AspNetCoreSample"
$site | Set-SSOLink -Link $password1 | Out-Null

$users = $site | Set-SSOChild -ChildType "group" "users"
$users | Set-SSOLink -LinkName "member" -Link $allusers | Out-Null

$policy = $site | Set-SSOChild -ChildType "policy" "policy"

$app = $site | Set-SSOChild -ChildType "application" "application" -Enabled
$app | Set-SSOLink -LinkName "allowedTo" -Link $users | Out-Null
$app | Set-SSOLink -Link $policy | Out-Null
$app | Set-SSOLink -Link $password1 -Enabled | Out-Null

$request = @{"redirect_uris"=@("http://localhost:19282/signin-oidc")} | ConvertTo-Json
$response = $app | Set-SSOAttribute -Name "metadata" -ContentType "application/json" -Body $request

$response | ConvertTo-Json | Set-Content -Path "client-config.json" -Force
