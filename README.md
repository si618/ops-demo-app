# atops-demo-app

[![build](https://github.com/si618/atops-demo-app/actions/workflows/build.yml/badge.svg)](https://github.com/si618/atops-demo-app/actions/workflows/build.yml)
[![push](https://github.com/si618/atops-demo-app/actions/workflows/push.yml/badge.svg)](https://github.com/si618/atops-demo-app/actions/workflows/push.yml)

Demo application for experimenting with DevOps, GitOps ... All The Ops!

- [Pushes](https://github.com/si618/atops-demo-app/actions/workflows/push.yml) to [docker hub](https://hub.docker.com/repository/docker/si618/atops-demo-app/general)
- Image can be automatically deployed to a Kubernetes cluster using an [ArgoCD configuration](https://github.com/si618/atops-demo-config)

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

### Install docker

```bash
sudo snap install docker
```
```bash
# Optionally run docker as current user
# Warning: https://snapcraft.io/docker
sudo addgroup --system docker
sudo adduser $USER docker
newgrp docker
sudo snap disable docker
sudo snap enable docke
```

### Install demo app

```bash
git clone https://github.com/si618/atops-demo-app.git
cd atops-demo-app
dotnet build ./Demo.sln
dotnet test ./Demo.sln

# Verify http demo is working locally
dotnet run --project ./DemoHttp

# Verify http demo is working in docker
docker compose up -d
```