## Deployment to AWS

1. **Prepare an S3 bucket** - The code for our Lambda Function will live in an S3 bucket.  Run the following command (make sure your AWS account has permissions to create a new S3 bucket) and look for output similar to the following:
    
    ```
    bash-3.2$ aws s3 mb s3://starwars-api-lambda
    make_bucket: starwars-api-lambda
    ```
2. **Publish the Microservice** - This is where the code for our Lambda Function will live.  Run the following command (make sure your AWS account has permissions to create a new S3 bucket) and look for output similar to the following:
    <pre><code>
    bash-3.2$ dotnet publish StarWarsMicroservice/
    <span style="color:green">publish: Published to /Users/jandersen/Projects/dotnet-on-lambda/StarWarsMicroservice/bin/Debug/netcoreapp1.0/publish</span>
    Published 1/1 projects successfully
    </code></pre>
3. **Add a Serverless Application Model Template** - What's a Serverless Application Model (SAM) template?  Good question.  Those familiar with AWS will probably have run across or used [Cloud Formation templates](https://aws.amazon.com/cloudformation/aws-cloudformation-templates/) which define service or application architectures to be deployed on AWS.   A SAM template is the same idea but Amazon has created a slightly more simple syntax focused on Lambda APIs which can be transformed (via the `aws` command line tool) to the standard cloud formation template syntax. The SAM syntax is described [here](https://github.com/awslabs/serverless-application-model/blob/master/versions/2016-10-31.md).  We'll use it to define the Lambda function and API gateway endpoint ("Resource" in AWS-speak) where our microservice will be deployed.
    * Create a [starwars-api-template.yml](StarWarsMicroservice/starwars-api-template.yml) inside the `StarWarsMicroservice/` directory
    * Follow [the link](StarWarsMicroservice/starwars-api-template.yml) to get the contents of the file.  Here are a few things to point out:
        * **The SAM Transformation** - The following property triggers the transformation form SAM syntax to standard cloud formation syntax:

            ```
            Transform: AWS::Serverless-2016-10-31
            ```
        * **Handler Property Must Match Your App** - If you're following along with this guide precisely you shouldn't need to adjust this line.  However, if you're adapting an existing project or otherwise tweaking, you may need to adjust this to properly reference the Lambda entrypoint to your application:
            ```
            # The syntax is <AssemblyName>::<Your.Namespace.ClassName>::<MethodName>
            
            Handler: StarWarsMicroservice::StarWarsMicroservice.LambdaGateway::FunctionHandlerAsync
            ```
        * **Notice the Code URI** - When we use the `aws` CLI to package up our app this specifies where it'll pull the compiled assemblies from.  Notice that this matches the directory, relative to the template file, to which our API was published previously.  If you specified a different publish directory than the default, update this line of the template accordingly.

            ```
            CodeUri: ./bin/Debug/netcoreapp1.0/publish
            ```
        * **Events Become API Gateway Resources** - The objects defined under "Events" are what will trigger your lambda function.  In our case we are defining API endpoints, hence `Type: Api`.   The `Path: /api/starwars/{proxy+}` means that any endpoint hitting the AWS API gateway matching `/api/starwars/*` will be forwarded on to our Lambda Function.   We want both `GET` and `POST` to reach our API.  

            ```
            Events:
                StarWarsGetResource:
                    Type: Api
                    Properties:
                        Path: /api/starwars/{proxy+}
                        Method: ANY
            ```

4. **Create the Cloud Formation Package** - Now we'll use [`aws cloudformation package`](http://docs.aws.amazon.com/cli/latest/reference/cloudformation/package.html) to transform our SAM template **AND** upload a deployment package for our Lambda function to the S3 bucket created previously.  From the root of the repository:
    ```
    bash-3.2$ bash-3.2$ aws cloudformation package --template-file StarWarsMicroservice/starwars-api-template.yml \
                                                    --output-template-file StarWarsMicroservice/serverless-output.yaml \
                                                    --s3-bucket starwars-api-lambda
    ```

5. **Use our Generated Cloud Formation template to Deploy to AWS** - Now we'll use [`aws cloudformation deploy`](http://docs.aws.amazon.com/cli/latest/reference/cloudformation/deploy/index.html) to actually **create** the AWS API Gateway and Lambda function using the generated template at the Lambda package with our microservice that was uploaded to S3 in the previous step.  Again from the root of our repository:
    ```
    bash-3.2$ aws cloudformation deploy --template-file StarWarsMicroservice/serverless-output.yaml \
                                        --stack-name starwars-api 
                                        --capabilities CAPABILITY_IAM
    ```

6. **Check the Deployment** - Let's now make sure our API is up.  The **first** request to the endpoint will incur the cost of "warming up"/initializing the lambda function; <span style="color: red">**it will be slow!**</span>. Subsequent requests should be significantly faster<sup>1</sup>. 
    * Find the **ID** of the API that has been created as part of our stack using the command line: `aws apigateway get-rest-apis`
    * The API endpoint should be in the format: `https://<YOUR_API_ID>.execute-api.us-west-2.amazonaws.com/Prod/`.  You should also be able to see it in the [AWS API Gateway Console](https://console.aws.amazon.com/apigateway/home) (make sure you're signed in).
    * Try hitting your endpoint in a browser.   For example <pre>https://<span style="color:red">&lt;YOUR_API_ID&gt;</span>.execute-api.us-west-2.amazonaws.com/Prod/api/starwars/characters/search/Vader</pre> should get us to Darth Vader...
        
        ```
        [{"name":"Darth Vader","url":"http://swapi.co/api/people/4/","eye_color":"yellow","birth_year":"41.9BBY"}]
        ```

### Footnotes
1. Refer to the [AWS forums](https://forums.aws.amazon.com/thread.jspa?threadID=181348) for further discussion of dealing with cold start time.
