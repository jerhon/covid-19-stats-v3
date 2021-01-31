# README

This is a .NET / React application that's used to display some interesting statistics around COVID-19 trends.
The application has a US map in which states can be clicked, and statistics regarding COVID-19 will display.

# Running the Application

This application can be run locally, by navigating to the src/Honlsoft.CovidApp directory and running the command `dotnet watch run`

or you can pull down the latest version of the docker container to run locally as well.

```docker run --rm -it -p 8080:80 jerhon/hs-covid-app-v3```

Then navigate to http://localhost:8080 to see the application.

# Running the Tests

At the head of the application run ```dotnet test```

# How it Works

The application takes data from The Covid Tracking Project, and imports it into an in memory SQL Lite database accessed via EF Core.
It will also query the project hourly to check for data updates, and merge it in to the SQL Lite database.
There are a few APIs which take an transform the data and return in back in a fashion the front end React application can utilize.
It's not the most efficient for querying, and CPU usage, but it gets the job done.
Work could be done to precompute or cache results to costly queries.

The APIs have an associated Swagger file that React consumes.
The React application uses Redux as a state store, and simply calls the APIs.
There isn't much in the way of error checking at this point if an API fails, but it could easily be added with the 

For the UI the application is utilizing [Material-UI](https://material-ui.com/).
The Map of the US is an SVG that I wrapped in a simple react component to make it interactive.
