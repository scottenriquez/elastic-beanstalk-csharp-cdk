using Amazon.CDK;

namespace ElasticBeanstalkBlueGreenDeployment
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new ElasticBeanstalkBlueGreenDeploymentStack(app, "ElasticBeanstalkBlueGreenDeploymentStack");
            app.Synth();
        }
    }
}
