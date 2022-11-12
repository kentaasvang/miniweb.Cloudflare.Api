# 🆕 miniweb.Cloudflare.Api

## ❓ Simple REST-api for updating records on Cloudflare
It's not a good idea to expose these endpoints to the internet. It's made to be used internally on a private server and thus have no authentication.

## ⚡ Getting Started
Configure user-secrets with e-mail and global api-key from cloudflare, then you should be good to go.
```bash
dotnet user-secrets init
dotnet user-secrets set "Cloudflare:Email" "your@email.com"
dotnet user-secrets set "Cloudflare:ApiKey" "your-api-key"
```

## 🔧 Building and Running on Ubuntu
You need an Ubuntuserver with .NET 6 SDK installed
Publish you application as self-contained and move release files to suitable location on you ubuntu-server. Generally `/opt` is a good choice.

**Create Unit-file in `/etc/systemd/system/cloudflareapi.service`**
```
[Unit]
Description=Autostart Cloudflare REST Api
After=network.target

[Service]
Type=simple
WorkingDirectory=<path-to-project-root>
ExecStart=<path-to-project-root>/<your-entry-DLL-file>
User=root # generally should not be run by root, but do what you want.
Restart=always
RestartSec=5

[Install]
WantedBy=multi-user.target
```
**Start service**
```
sudo systemctl start cloudflareapi.service
```

**Check status**
```
sudo systemctl status cloudflareapi.service
```