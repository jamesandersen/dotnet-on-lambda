## Publish Scripts

1. **Automate AWS Deployment** - With a minor change to `project.json` we can automate the deployment of further changes to our API whenever the `dotnet publish` command is used.  The [`project.json` spec](https://docs.microsoft.com/en-us/dotnet/articles/core/tools/project-json#scripts) supports the execution of script commands after publishing.  We can add our two `aws` commands to `project.json` as follows and then a simple `dotnet publish` of our project will also deploy a new version out to AWS.

    ```
        ,
        "scripts": {
            "postpublish": [
            "aws cloudformation package --template-file starwars-api-template.yml --output-template-file serverless-output.yaml --s3-bucket starwars-api-lambda",
            "aws cloudformation deploy --template-file serverless-output.yaml --stack-name starwars-api --capabilities CAPABILITY_IAM"
            ]
        }
    ```

## Further Reading
* [Announcing C# Support for AWS Lambda](https://aws.amazon.com/blogs/compute/announcing-c-sharp-support-for-aws-lambda/)
* [Programming Model for Authoring Lambda Functions in C#](http://docs.aws.amazon.com/lambda/latest/dg/dotnet-programming-model.html)
* [Creating Deployment Package with .NET Core CLI](http://docs.aws.amazon.com/lambda/latest/dg/lambda-dotnet-coreclr-deployment-package.html)
* [Source Code for `Amazon.Lambda.AspNetCoreServer`](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.AspNetCoreServer)