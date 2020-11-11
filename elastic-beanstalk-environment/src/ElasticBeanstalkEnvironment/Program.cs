using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticBeanstalkEnvironment
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new ElasticBeanstalkEnvironmentStack(app, "ElasticBeanstalkEnvironmentStack");
            app.Synth();
        }
    }
}
