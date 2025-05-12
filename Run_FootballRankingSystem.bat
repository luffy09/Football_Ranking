@echo off
echo Restoring NuGet packages...
dotnet restore

echo Building the project...
dotnet build --no-restore

echo Applying EF Core migrations...
dotnet ef database update

echo Running the application with dotnet...

start dotnet run

:: Wait for a few seconds to ensure the app starts up
timeout /t 5 /nobreak

echo Open Swagger UI in the default web browser
start http://localhost:5000/swagger

pause
