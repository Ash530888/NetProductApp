1. Install Latest .NET Core SDK  
.NET SDK Version:           8.0.401  

2. Create a DB to hold products and logins data  
My DB:  
name = NetProductApp  
tables:  
logins(username, password, role)  
products(name, price, image_path)  

3. (note these packages are postgres specific - need to install diff for diff DB) Install the packages (terminal commands) needed for a dotnet app to communicate with your DB  
dotnet add package Microsoft.EntityFrameworkCore  
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL  

4. set up the ‘DbContext’  
edit ‘ApplicationDbContext’ in the ‘Models’ folder as required with table names  

5. Configure your app to connect w/ DB  
‘appsettings.json’ add the connection string for the db (host, db, username, password)  
may need to edit 'Program.cs'  

6. create and apply a migration
dotnet add package Microsoft.EntityFrameworkCore.Design  
dotnet ef migrations add InitialCreate  
dotnet ef database update  

8. Run the app: dotnet run
