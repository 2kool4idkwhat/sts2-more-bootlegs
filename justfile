build:
    DOTNET_ROLL_FORWARD=LatestMajor dotnet build

fmt:
    csharpier format .
