.NET Web App W/ Following Functionalities:
- Displays all products (products table in Database) w/ info (price, image, etc.)
- Only Products in Stock (Stock table in Database) are displayed 
- A User can register or login
    - password is verified (at least 8 chars, 1 special char, 1 digit)
    - Admin Users (admin login role) can Create new products, edit products (database updated accordingly)
- A User can add products to their cart (quantities are validated against stock)
    - after a successful checkout, stock table database is updated

1. Install Latest .NET Core SDK  
.NET SDK Version:           8.0.401  

2. Create a DB to hold products, stock, and logins data  
My DB:  
name = NetProductApp  
tables:  
logins(id, username, password, role)  
products(id, name, price, image_path)
stock(id, num_instock)  

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
