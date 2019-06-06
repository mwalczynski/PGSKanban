# PgsKanban

PGS Kanban is a kanban implementation. Is supports a owner-member system for kanban boards and a drag&drop system for kanban lists and cards.

# Getting Started
## Prerequisites
### Front-end:
* Node.js 6.11.0+
* NPM 3.10.10+
* Angular CLI

### Back-end:
* .NET Core SDK 2.0

## Installing

### Front-end:
Install the dependencies and start the server.
```s
cd pgs-kanban/PgsKanban_Frontend
npm install
npm start
```
The application is at http://localhost:4200
### Back-end:
The project can be run with Visual Studio or through the console.

The project's entry point is located at the pgs-kanban/PgsKanban_Backend/PgsKanban.Api/Program.cs path.

#### Running through the console:
```s
cd pgs-kanban/PgsKanban_Backend/PgsKanban.Api
dotnet restore
dotnet run
```

If you run the back-end through Visual Studio the packages are automatically restored.

#### You can change the used database by following these steps:

```s
Navigate the code to Startup.cs
Find the method ConfigureServices and the following two lines:
    services.AddDbContext<PgsKanbanContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
Change them f.e. to:
    services.AddDbContext<PgsKanbanContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("LocalDb")));

Then call update-database in the Package Manager Console.
```
The served website is available at http://localhost:8888

# Deployment
The project is deployed through TeamCity on virtual machine which is running on a local PGS server.

The virtual machine's name is pgsdev72

There are 3 environments set up for the project - PROD, DEV and TEST.
PROD gets it's source from the master branch on the project's repository,
DEV gets the source from the dev branch and TEST gets it from the test branch.

You can review each environment's build steps in the environment's settings.

#### The app can be deployed to production by following these steps:

```s
Connect to the virtual machine pgsdev72 through remote access

Open up a browser and navigate to http://localhost:8000

Log in to teamcity

Click the Run button next to the desired environment you wish to deploy.
```

##### If you choose the production build:
* the front-end is also available at kanban.pgs-soft.com
* the back-end is available at kanban-api.pgs-soft.com


# Built with
### Front-end
* [TypeScript](https://www.typescriptlang.org/) - TypeScript - superset of JavaScript
* [Angular 4](https://github.com/angular/angular) - web application framework
* [RxJs](https://github.com/Reactive-Extensions/RxJS) - reactive extensions for JavaScript
* [ngrx/store](https://github.com/ngrx/store)  - state management for Angular applications, inspired by Redux
* [Webpack](https://webpack.js.org/) - bundler/task runner 
* [Scss](http://sass-lang.com/) - CSS preprocessor
### Back-end
* [.Net Core 2.0 + C# 7.0](https://github.com/dotnet/core) - platform
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/#pivot=efcore) - ORM framework
* [SQLServer](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads) - database solution
* [ASP.Net Core Web Api](https://docs.microsoft.com/en-us/aspnet/core/) - web application framework
* [Swagger](https://swagger.io/) - Api documentation platform
* [AutoFac](https://autofac.org/) - Dependency Injection container
# Authors
### Mentors
* Łukasz Kurzyniec
* Katarzyna Michalak
### Developers
* Patryk Bielecki
* Michał Kurpiński
* Przemysław Morski
* Michał Walczyński
* Mateusz Wygoda