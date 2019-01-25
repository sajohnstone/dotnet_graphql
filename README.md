# GraphQL dotnet Example

This is an example of using graphQL with dot net.

## Tools
#### Coverlet [https://github.com/tonerdo/coverlet]
This provides code coverage reports for dotnet in OpenCover format (comptabile with SonarQube).  To install the tool run the following from the command line
```
dotnet tool install --global coverlet.console
```
To add it to the project you need to run the following
```
dotnet test Tests/tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```
