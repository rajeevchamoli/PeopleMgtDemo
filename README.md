# People Management Web Application
Demo application for basic user data management.

-***Technologies and tools used***:
- Angular  (v7.1.x)
- [.Net Core](https://dotnet.microsoft.com/download/dotnet-core/2.2) (v2.2.3)
- Entity framework core 
- Sqlite DB (https://www.sqlite.org/2019/sqlite-dll-win64-x64-3270200.zip)
- [Node.js](https://nodejs.org/en/download/) (v10.x.x) 
- [VS 2017 community](https://visualstudio.microsoft.com/downloads/) (v15.9)

# Pre-reqs
Following products needs to be installed for development setup.
1. Node.js
2. Angular CLI (7.1.4 or greater) (`npm install -g '@angular\cli'`)
3. .Net Core 2.2.3

## Get started

### Clone the repo

```
launch shell/cmd
git clone https://github.com/rajeevchamoli/PeopleMgtDemo.git
cd App
```

#### Two ways to build and launch the application

### Build using [VS 2017 community] ( It is simpler Approach )

* Go to folder 'App'
* Launch PeopleMgt.sln in VS 2017
* Press F5 or run it  ( this might take some time to download the packages if not alredy done)
* Application should launch in browser. ( It might timeout in first attempt, so need to just refresh the page.) 

### Manual Build ( using Angular cli )

## Install npm packages Manually

```
Install the npm packages described in the `package.json` and verify that it works:

launch shell/cmd
cd App\PeopleMgt\ClientApp
npm install

This cmd may take some time to install required packages from **package.json**

```


## Build Angular WebApp Manually

```
launch shell/cmd
cd App\PeopleMgt\ClientApp
Ng build

```
**`Ng build` - runs the TypeScript compiler, asset copier.**

## Launch the Angular Web App

```shell
cd App\PeopleMgt\ClientApp
Ng serve
```
*`open your browser to launch the app on http://localhost:4200/`*
  
#### Run xUnit test cases

Test explorer in Visual Studio 2017 can help execute the testcases.
