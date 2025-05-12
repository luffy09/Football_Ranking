First method to run:

-First you need to set up the database dotnet ef migrations add SetupDataBase

-Run the Run_FootBallRankingSystem.bat


Second method to run:

-First you need to set up the database ---------- dotnet ef migrations add SetupDataBase

-Get nuPacs ---------- dotnet restore

-Build ---------- dotnet build --no-restore

-Apply Migration ---------- dotnet ef database update

-Then just run with the visual studio run button

(if you use dotnet run swagger may not open automatically. 
To access it open https://localhost:7230/swagger/index.html
OR https://localhost:5000/swagger/index.html)