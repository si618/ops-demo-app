# argocd-demo-app

[![build](https://github.com/si618/argocd-demo-app/actions/workflows/build.yml/badge.svg)](https://github.com/si618/argocd-demoapp/actions/workflows/build.yml)
[![push](https://github.com/si618/argocd-demo-app/actions/workflows/push.yml/badge.svg)](https://github.com/si618/argocd-demoapp/actions/workflows/push.yml)

Demo application pushed to [docker hub](https://hub.docker.com/repository/docker/si618/argocd-demo-app/general) for
automated deployment using an [ArgoCD configuration](https://github.com/si618/argocd-demo-config)

## Setup

Instructions to build on Ubuntu

### Install dotnet

```bash
# Install Microsoft package repository (optional when Ubuntu supports dotnet 7)
wget https://packages.microsoft.com/config/ubuntu/22.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install SDK and ASP.NET Core Runtime
sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
sudo apt-get update && sudo apt-get install -y aspnetcore-runtime-7.0
```

### Install demo app

```bash
# Clone and build demo app
git clone https://github.com/si618/argocd-demo-app.git
cd argocd-demo-app
dotnet restore
dotnet build --no-restore
dotnet test --no-restore

# Verify demo app is working locally
dotnet run --project ./DemoApi
```