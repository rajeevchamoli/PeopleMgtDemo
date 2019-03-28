# People Management Web Application
Demo application for basic user profile management.

-***Technologies and tools used***:
- Angular  (v7.1.x)
- [.Net Core](https://dotnet.microsoft.com/download/dotnet-core/2.2) (v2.2.x)
- Entity framework core 
- Sqlite DB (https://www.sqlite.org/2019/sqlite-dll-win64-x64-3270200.zip)
- [Node.js](https://nodejs.org/en/download/) (v10.x.x) 
- [VS 2017 community](https://visualstudio.microsoft.com/downloads/) (v15.9)

# Pre-reqs
Following products needs to be installed for development setup.
1. Node.js
2. Angular CLI (`npm install -g '@angular\cli'`)
3. .Net Core 2.2

## Get started

### Clone the repo

```shell
git clone https://github.com/rajeevchamoli/PeopleMgtDemo.git
cd App
```

#### Two ways to build and launch the application

### Build using [VS 2017 community]

* Go to folder 'App'
* Launch PeopleMgt.sln in VS 2017
* Press F5 or run it  ( this might take some time to download the packages if not alredy done)
* Application should launch in browser.

### Manual build

## Install npm packages manually

```shell
Install the `npm` packages described in the `package.json` and verify that it works:

```shell
cd App\PeopleMgt\ClientApp
npm install
```

## Build Angular WebApp manually

```shell
cd App\PeopleMgt\ClientApp
Ng build
```
* `Ng build` - runs the TypeScript compiler, asset copier.

## Launch the Angular Web App

```shell
cd App\PeopleMgt\ClientApp
Ng serve
```

