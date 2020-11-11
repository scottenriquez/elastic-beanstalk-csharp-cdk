using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

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
