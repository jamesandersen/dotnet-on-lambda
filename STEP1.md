## Prequisites
This guide will assume you are already somewhat comfortable with [Git](https://git-scm.com/) and have the command line tools for dotnet core and AWS installed. Here's what needs to be in place before you proceed:

1. **Install [.NET Core](https://www.microsoft.com/net/core)** - Double check that `dotnet --info` runs successfully.  You should see some output similar to this:

    ```
    jandersen-mbp15r:dotnet-on-lambda jandersen$ dotnet --info
    .NET Command Line Tools (1.0.0-preview2-1-003177)

    Product Information:
    Version:            1.0.0-preview2-1-003177
    Commit SHA-1 hash:  a2df9c2576

    Runtime Environment:
    OS Name:     Mac OS X
    OS Version:  10.12
    OS Platform: Darwin
    RID:         osx.10.12-x64
    ```
2. **[Install](http://docs.aws.amazon.com/cli/latest/userguide/installing.html) and [Configure](http://docs.aws.amazon.com/cli/latest/userguide/cli-chap-getting-started.html) the [AWS Command Line Interface](https://aws.amazon.com/cli/)** -  
    Double check that `aws iam get-user` runs successfully.  You should see some output similar to this (obviously this is sanitized a bit):
    
    ```
    USER	arn:aws:iam::999999999999:user/jandersen	2017-01-03T05:17:17Z	2017-01-25T03:11:30Z	/	AAAAAAAAAAAAAAAA	jandersen
    ```
3. **Clone [this repo](https://github.com/jamesandersen/dotnet-on-lambda)** - The rest of the guide will assume you're working in the root of your local working copy of the repository.

## Restore & Run

```
# Restore NuGet packages
dotnet restore

# Run (-p passes the project directory to run)
dotnet run -p StarWarsMicroservice
```

You should see output similar to the following:
```
jandersen-mbp15r:dotnet-on-lambda jandersen$ dotnet run -p StarWarsMicroservice
Project StarWarsMicroservice (.NETCoreApp,Version=v1.0) will be compiled because expected outputs are missing
Compiling StarWarsMicroservice for .NETCoreApp,Version=v1.0

Compilation succeeded.
    0 Warning(s)
    0 Error(s)

Time elapsed 00:00:01.2441395
 

Hosting environment: Production
Content root path: /Users/jandersen/Projects/dotnet-on-lambda
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Test it out
Browse to http://localhost:5000/api/starwars/characters/search/skywalker and verify that you get some sample JSON
```
[
    {"name":"Luke Skywalker","url":"http://swapi.co/api/people/1/","eye_color":"blue","birth_year":"19BBY"},
    {"name":"Anakin Skywalker","url":"http://swapi.co/api/people/11/","eye_color":"blue","birth_year":"41.9BBY"},
    {"name":"Shmi Skywalker","url":"http://swapi.co/api/people/43/","eye_color":"brown","birth_year":"72BBY"}
]
```

## A Few Notes
* **Why Not .NET Core 1.1.x?** - AWS Lambda supports the latest *LTS* version which is 1.0.3 (at present); see [this discussion](https://github.com/aws/aws-lambda-dotnet/issues/36) for more information.
* **Why Not Start With `dotnet new -t web`?** - The starter project created by `dotnet new -t web` includes a lot of front end code that is unrelated to a microservice API.  This repo strips down to the most relevant code for the purpose of the guide.
