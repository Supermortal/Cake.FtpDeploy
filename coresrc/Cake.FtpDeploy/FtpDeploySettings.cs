using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.Tooling;

namespace Cake.FtpDeploy
{
    public class FtpDeploySettings : ToolSettings
    {
        public string FtpUri { get; set; }
        public string ArtifactsPath { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
    }
}
