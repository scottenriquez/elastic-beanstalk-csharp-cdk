using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CodeCommit;
using Amazon.CDK.AWS.CodePipeline;
using Amazon.CDK.AWS.CodePipeline.Actions;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;

namespace ElasticBeanstalkBlueGreenDeployment
{
    public class ElasticBeanstalkBlueGreenDeploymentStack : Stack
    {
        internal ElasticBeanstalkBlueGreenDeploymentStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var blueEnv = Node.TryGetContext("blue_env");
            var greenEnv = Node.TryGetContext("green_env");
            var appName = Node.TryGetContext("app_name");

            var bucket = new Bucket(this, "BlueGreenBucket", new BucketProps
            {
                RemovalPolicy = RemovalPolicy.DESTROY
            });

            var handler = new Function(this, "BlueGreenLambda", new FunctionProps
            {
                Runtime = Runtime.PYTHON_3_7,
                Code = Code.FromAsset("resources"),
                Handler = "blue_green.lambda_handler",
                Environment = new Dictionary<string, string>
                {
                    ["BUCKET"] = bucket.BucketName
                }
            });

            bucket.GrantReadWrite(handler);

            var repo = new Repository(this, "Repository", new RepositoryProps
            {
                RepositoryName = "MyRepositoryName"
            });

            var pipeline = new Pipeline(this, "MyFirstPipeline");

            var sourceStage = pipeline.AddStage(new StageOptions
            {
                StageName = "Source"
            });

            var sourceArtifact = new Artifact_("Source");

            var sourceAction = new CodeCommitSourceAction(new CodeCommitSourceActionProps
            {
                ActionName = "CodeCommit",
                Repository = repo,
                Output = sourceArtifact
            });

            sourceStage.AddAction(sourceAction);

            var deployStage = pipeline.AddStage(new StageOptions
            {
                StageName = "Deploy"
            });

            var lambdaAction = new LambdaInvokeAction(new LambdaInvokeActionProps
            {
                ActionName = "InvokeAction",
                Lambda = handler,
                UserParameters = new Dictionary<string, object> 
                {
                    ["blueEnvironment"] = blueEnv,
                    ["greenEnvironment"] = greenEnv,
                    ["application"] = appName
                },
                Inputs = new[] {sourceArtifact}
            });

            deployStage.AddAction(lambdaAction);
        }
    }
}
