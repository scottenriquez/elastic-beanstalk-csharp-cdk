using Amazon.CDK;
using Amazon.CDK.AWS.ElasticBeanstalk;

namespace ElasticBeanstalkEnvironment
{
  public class ElasticBeanstalkEnvironmentStack : Stack
  {
    internal ElasticBeanstalkEnvironmentStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
      const string APPLICATION_NAME = "cdk-application";
      const string ENVIRONMENT_NAME = "cdk-dotnet-environment";

      var solutionStackName = Node.TryGetContext("solutionStackName").ToString();

      var cfnApplication = new CfnApplication(this, "Application", new CfnApplicationProps
      {
        ApplicationName = APPLICATION_NAME
      });
      
      var cfnEnvironment = new CfnEnvironment(this, "Environment", new CfnEnvironmentProps
      {
        EnvironmentName = ENVIRONMENT_NAME,
        ApplicationName = cfnApplication.ApplicationName,
        SolutionStackName = solutionStackName,
        OptionSettings = new []
        {
          new CfnEnvironment.OptionSettingProperty()
          {
            Namespace = "aws:elasticbeanstalk:environment",
            OptionName = "ServiceRole",
            Value = "aws-elasticbeanstalk-service-role"
          },
          new CfnEnvironment.OptionSettingProperty()
          {
            Namespace = "aws:autoscaling:launchconfiguration",
            OptionName = "IamInstanceProfile",
            Value = "aws-elasticbeanstalk-ec2-role"
          },
          new CfnEnvironment.OptionSettingProperty()
          {
            Namespace = "aws:elasticbeanstalk:environment",
            OptionName = "EnvironmentType",
            Value = "SingleInstance"
          }
        }
      });
      
      cfnEnvironment.AddDependsOn(cfnApplication);
    }
  }
}
