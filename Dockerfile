# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

# Install dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef

# Add the dotnet tools directory to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the project files
COPY *.csproj ./
RUN dotnet restore

COPY . ./

# Expose port (optional, adjust to match your app's port)
EXPOSE 5288

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "run"]
