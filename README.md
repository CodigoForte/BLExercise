# Summary

This repository includes a .NET 6 solution, implementing the specs detailed in the following User Story.

## User Story

### Product Management in the System
### Description:
As an administrator,
I want to have the capability to manage the products on our platform (add, edit, view, and delete),
So I can keep the product information updated and provide our customers with an accurate and up-to-date catalog.

**Acceptance Criteria:**

- **Add Products:** There should be an interface or form where I can input the product information (name, description, price) and save it to the database.
  
  The following validations must be met when adding a product:
  - Prices of products must be greater than zero and cannot exceed 10,000.
  - Product names cannot be empty.
  - Product names must contain at least 2 characters and a maximum of 50 characters.
  - Product descriptions cannot be empty and must have at least 20 characters.

- **Edit Products:** From a list of existing products, I want to select one and be able to modify any of its information and save those changes.
The same validations listed above for adding a product apply when editing.

- **View Products:** I should be able to see a list of all existing products.

- **Delete Products:** From the product list, I want to select a product and have an option to delete it from the system. I should receive a confirmation prompt before performing the deletion to prevent accidental deletions.

- **User Creation:** I want to be able to register a new user by providing details such as name, email, and password.

- **Secure Access:** Once the user is created, they should be able to log in with their credentials. The system should validate the credentials and provide appropriate error messages in case of incorrect information.


## Pre-requisites
This solution runs on .NET 6 and SQL Server Express
- Visual Studio 2022
- SQL Server Express

## Architecture

| Project       | Type                 | Description                                                           |
| ------------- | -------------------- | --------------------------------------------------------------------- |
| BL.Core       | Class Library        | Common project with Business Entities.                                |
| BL.Data       | Class Library        | Data Access Layer, contains the repositories. Uses Dapper.            |
| BL.Services   | Class Library        | Business Logic Layer, contains business rules and validations.        |
| BL.Setup      | ASP.NET Core Web App | Setup of the Database, contains scripts for the db schema.            |
| BL.Tests      | xUnit Test           | Contains the Unit Tests for the Business Logic.                       |
| BL.WebAPI     | ASP.NET Core Web API | WebAPI Project (MVC)                                                  |
| BL.WebSite    | ASP.NET Core Web App | Front-End, contains Razor pages with jQuery for the calls to the API. |


## Setup the Database

First, build the complete solution. 
Then, set **BL.Setup** as Startup project, and Start (F5) the project. 
This will open a website with a single button for the **One Click Setup**, this will create the Database in SQL Server Express.

### Connection String
The **BL.Setup** and **the BL.WebAPI** projects have configured the Connection String in the `appsettings.json` file as:
```
"Server=.\\SQLEXPRESS;Database=BLProducts;Trusted_Connection=True;Encrypt=False;"
```

## Running the WebAPI and WebSite
Right click on the solution, and select 'Configure Startup projects...', select the option **Multiple startup projects:** and set the action **Start** for the projects **BL.WebAPI** and **BL.WebSite** and click on **OK**. 

Finally, Start (F5) to run the projects. 

This will open two browser windows, one with the WebAPI running Swagger, and the other with the WebSite.

Then, you can create a new user, log in, and manage the products.

## Thought process

### Database
Because the short deadline and the nature of the prototype, first I tried to use a _SQL Server Database Project_ and publishing the Database as _LocalDB_, but I had multiple issues, eg: the .mdf should be accesible from the WebAPI project, so I tried to publish to the folder _app_data_ in the WebAPI project, but I had permissions and routing issues.

Then, I tried to create directly the _LocalDB_ file in the WebAPI project, but the template for the item Service-based Database is not available for this type of projects.

Finally I decided to use a ASP.NET Core Web App project with logic that I already use for my different projects. I created the schema of the database in SQL Server Management Studio, generated the sql scripts and use these scripts in the Setup Project that uses _SQL Server Express_.

### TDD
I started with TDD for the business logic validations, without any issues. 

But for testing the Repository I needed to configure Dapper and because I'm more used to Entity Framework and ADO.NET, and it's the first time I use Dapper for Data Access, the time was not going to be enough for all the required configurations. So, I'm not including TDD unit tests for the Repository.

Same thing with WebAPI tests, because requires more time for configuration and mocking of the Authorization.

In the end it was a very interesting challenge and helped me to work for the first time with Dapper, which I had wanted to do for a long time.

> Last update: 2023 09 22
