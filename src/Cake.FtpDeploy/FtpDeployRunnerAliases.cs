using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.FtpDeploy.Services;

namespace Cake.FtpDeploy
{
    public static class FtpDeployRunnerAliases
    {
        [CakeMethodAlias]
        public static void FtpDeploy(this ICakeContext context, FtpDeploySettings settings)
        {
            var webService = new WebService();
            var ftpService = new FtpService(context.Log, webService);
            var runner = new FtpDeployRunner<FtpDeploySettings>(context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools, ftpService);
            runner.Execute(settings);
        }
    }
}
