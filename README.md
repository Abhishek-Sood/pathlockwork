# pathlockwork

## How to run the files 

### Task 1 

Frontend <br>
npm install axios bootstrap <br>
npm run<br>

Backend <br>
dotnet run <br>



### Task 2 + Task 3 (Scheduler is implemented)

Frontend <br>
npm install axios bootstrap react-router-dom <br>
npm run

Backend<br>
dotnet add package Microsoft.EntityFrameworkCore<br>
dotnet add package Microsoft.EntityFrameworkCore.Sqlite<br>
dotnet add package Microsoft.EntityFrameworkCore.Tools<br>
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer<br>
dotnet ef migrations add InitialCreate<br>
dotnet ef database update<br>
dotnet run<br>

