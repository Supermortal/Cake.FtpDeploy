using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.FtpDeploy.Services;

namespace Cake.FtpDeploy
{
    public class FtpDeployRunner<TSettings> : Tool<TSettings> where TSettings : FtpDeploySettings, new()
    {

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly IToolLocator _tools;
        private readonly IFtpService _ftpService;

        public FtpDeployRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IFtpService ftpService) : base(fileSystem, environment, processRunner, tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _tools = tools;
            _ftpService = ftpService;
        }

        protected override string GetToolName()
        {
            return "Cake.FtpDeploy";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "ftpdeploy";
        }

        public virtual void Execute(TSettings settings)
        {
            ValidateSettings(settings);
            _ftpService.DeleteAll(settings);
            _ftpService.UploadAll(settings);
        }

        protected virtual void ValidateSettings(TSettings settings)
        {
            if (settings.FtpUserName == null)
            {
                throw new ArgumentNullException("settings.FtpUserName", "An ftp username must be specified.");
            }
            if (settings.FtpPassword == null)
            {
                throw new ArgumentNullException("settings.FtpPassword", "An ftp password must be specified.");
            }
            if (settings.ArtifactsPath == null || !Directory.Exists(settings.ArtifactsPath))
            {
                throw new ArgumentException("settings.ArtifactsPath", "A valid artifacts directory must be specified.");
            }
            if (settings.FtpUri == null)
            {
                throw new ArgumentNullException("settings.FtpUri", "A base ftp uri must be specified.");
            }
        }
    }
}
