# Connecterra - Cow and Sensor Tracking system.

- The application is created using .net core and EntityFramework core and database in Microsoft SQL.
- The application has a standard SQL database with 4 tables: DBname as farmDB.
    - [dbo].[Audits]
    - [dbo].[Cows]
    - [dbo].[Farms]
    - [dbo].[Sensors]

- DbContext in Start up is using SQL Server and the connection string is present in DefaultConnection (appsettings.json). 

- There was an initial migration to the table using Microsoft.EntityFrameworkCore.Design. This was added to the API project for migration.

- Commands used in visual studio developer command prompt to create the database and the tables in SQL are as follows. 

    - dotnet ef migrations add InitialCreate -p Infrastructure -s API -c FarmContext
    - dotnet ef database update -p Infrastructure -s API -c FarmContext

- I have created a FarmContextSeed class, which will feed some mocked data to the database after running the solution. This will check if the tables are empty only then the data will be fed in.  This is reading the data from json files present in Infrastructure/SeedData.

- There are a total of 4 APIs created with multiple operations.

    Audit

    - GET  ​/api​/Audit - Get Audit List

    Cows

    - GET  ​/api​/Cows - Get list of cows
    - GET  ​/api​/Cows​/{id} - Get Cow based on cow ID
    - PUT ​/api​/Cows​/{cowId} - Update the cow's status based on CowId
    - GET  ​/api​/Cows​/count - Get Cow count base on farm , state and Date

    Farm

    - GET  ​/api​/Farm - Get list of Farms

    Sensors

    - GET  ​/api​/Sensors - Get list of Sensors
    - POST ​/api​/Sensors - Add a Sensor
    - GET  ​/api​/Sensors​/{id} - Get Sensor based on Sensor ID
    - PUT ​/api​/Sensors​/{sensorId} - Update a Sensor
    - GET  ​/api​/Sensors​/average - Get Sensor Average per month
    - GET  ​/api​/Sensors​/count - Get Sensor Count per month

- There is swagger implementation for the API to know the Request and Response with status code in details. Available in Json and can also access after running the application.
    - The local url for swagger - https://localhost:5001/swagger/index.html
    - The json is copied and available in (Documents/swagger.json) path. Copy the json to https://editor.swagger.io/ to view the APIs and its operations.

- I have created a Postman collection for the APIs available at (Documents/Connecterra(Cow and Sensor tracking).postman_collection).

1 - The state changes are reflected in the database by external services and APIs, i.e. someone from the app can change the sensor state to "Deployed" or some automation on the farm can change the cow state to "Pregnant".
   The API which can be used to achieve this is implemented under 
   - PUT ​/api​/Cows​/{cowId} - Update the cow's status based on CowId
   - PUT ​/api​/Sensors​/{sensorId} - Update a Sensor status

These changes are happening on a regular basis, i.e. multiple times a day from various sources. Sometimes errors may be committed as well, e.g., the cow's state can change from Pregnant to Dry and back to Pregnant within the same day. Typically cow state changes and sensor state changes occur after multiple days, so any quick changes happening in succession are generally erroneous and need to be flagged.


Part 1. You need to design an architecture for tracking this historical state, then implement it. Use your choice of database, microservice, language, platform, etc. technology. Would you update the existing database with any additional fields? If so, how would you index them? Additionally you may choose not to use the existing database to track these changes. If so, what kind of a mechanism would you use? How would you listen for these changes?

- I have used EntityFrameworkCore ChangeTracker to achieve the Audit trail of the data update and create. As part of this implementation I have created an Audit table and stored the audit details in that table whenever there is a change in state of Cow and Sensor.


Part 2. You also need to write a simple API that can answer the questions listed above.

1. How many cows are pregnant on farm "A" on a specific date? 
    - The API which can be used to achieve this is implemented under  - GET  ​/api​/Cows​/count - Get Cow count base on farm , state and Date
2. How many sensors died in June across the platform?
    - The API which can be used to achieve this is implemented under  - GET  ​/api​/Sensors​/count - Get Sensor Count per month based on status
3. On average how many new sensors are deployed every month in 2020?
    - The API which can be used to achieve this is implemented under - GET  ​/api​/Sensors​/average - Get Sensor Average per month
