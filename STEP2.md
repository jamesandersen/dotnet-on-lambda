## Prepare our API to be deployed to AWS Lambda

1. **Add [`Amazon.Lambda.AspNetCoreServer`](http://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer/0.8.2-preview1) to `StarWarsMicroservice\project.json`** - This package (still in preview as of this writing) will handle some tricky plumbing to
    * Translate an AWS API Gateway event into an `HttpRequestMessage` on the way into our API
    * Translate the outgoing `HttpResponseMessage` back into the expected AWS API Gateway response on the way out.
    * Provide a [handler entry point](http://docs.aws.amazon.com/lambda/latest/dg/dotnet-programming-model-handler-types.html) that will route the incoming message using the controllers in our API. 
    
    Checkout [the Readme](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.AspNetCoreServer) for (a few) more details.

    ```
        "dependencies": {
            "Microsoft.NETCore.App": {
                "version": "1.0.1",
                "type": "platform"
                },
            "Microsoft.AspNetCore.Mvc": "1.0.2",
            // ...
            // other packages...
            // ...
            // ADD THIS BELOW
            "Amazon.Lambda.AspNetCoreServer": "0.8.4-preview1"
        }
    ```
2. **Run `dotnet restore` to pull down the package from NuGet**
3. **Add a [`LambdaGateway`](StarWarsMicroservice/LambdaGateway.cs) class to the project** - As noted in the README.md for the project, this class will need to extend from `[APIGatewayProxyFunction](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.AspNetCoreServer/APIGatewayProxyFunction.cs)`.  It can go anywhere in the project but take note of the exact namespace as we'll need it in step 3.
4. **Run `dotnet build **/project.json` and ensure the project still builds successfully**
    