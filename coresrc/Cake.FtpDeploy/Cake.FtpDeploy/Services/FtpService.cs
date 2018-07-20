using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;

namespace Cake.FtpDeploy.Services
{
    public class FtpService : IFtpService
    {

        private volatile int _requestNumber = 0;

        private readonly ICakeLog _log;
        private readonly IWebService _webService;

        public FtpService(ICakeLog log, IWebService webService)
        {
            _log = log;
            _webService = webService;
        }

        public void DeleteAll(FtpDeploySettings settings)
        {
            var baseFtpUri = settings.FtpUri;
            var directoriesToRemove = new List<string>();
            DeleteAllRecursive(baseFtpUri, settings, directoriesToRemove);

            foreach (var directory in directoriesToRemove)
            {
                RemoveDirectory(directory, settings);
            }

            while (_requestNumber != 0)
            {
                Thread.SpinWait(1);
            }
        }

        public void DeleteAllRecursive(string ftpUri, FtpDeploySettings settings, List<string> directoriesToRemove)
        {
            var directoryDetailsList = ListDirectoryDetails(ftpUri, settings);

            foreach (var directoryDetails in directoryDetailsList)
            {
                if (directoryDetails.Type == DirectoryDetailsType.File)
                {
                    var filePath = ftpUri + "/" + directoryDetails.Name;
                    Interlocked.Increment(ref _requestNumber);
                    _log.Information($"Deleting file: {filePath}\n");
                    Task.Factory.StartNew(() => DeleteFile(filePath, settings));
                }
                else if (directoryDetails.Type == DirectoryDetailsType.Directory)
                {
                    var directoryPath = ftpUri + "/" + directoryDetails.Name;
                    DeleteAllRecursive(directoryPath, settings, directoriesToRemove);
                    directoriesToRemove.Add(directoryPath);
                }
            }
        }

        public void UploadAll(FtpDeploySettings settings)
        {
            var baseArtifactsPath = settings.ArtifactsPath;
            var baseArtifactsPathLength = baseArtifactsPath.Length;
            UploadAllRecursive(baseArtifactsPathLength, baseArtifactsPath, settings);

            while (_requestNumber != 0)
            {
                Thread.SpinWait(1);
            }
        }

        public void UploadAllRecursive(int baseArtifactsPathLength, string directoryPath, FtpDeploySettings settings)
        {
            var baseDirectory = new DirectoryInfo(directoryPath);
            var adjustedDirectoryPath = directoryPath.Substring(baseArtifactsPathLength);

            foreach (var file in baseDirectory.EnumerateFiles())
            {
                var ftpFilePath = settings.FtpUri + adjustedDirectoryPath + "/" + file.Name;
                Interlocked.Increment(ref _requestNumber);
                _log.Information($"Uploading file: {ftpFilePath}\n");
                Task.Factory.StartNew(() => UploadFile(ftpFilePath, file.FullName, settings));
            }

            foreach (var directory in baseDirectory.EnumerateDirectories())
            {
                var newDirectoryPath = directoryPath + "/" + directory.Name;
                var ftpDirectoryPath = settings.FtpUri + newDirectoryPath.Substring(baseArtifactsPathLength);

                MakeDirectory(ftpDirectoryPath, settings);
                UploadAllRecursive(baseArtifactsPathLength, newDirectoryPath, settings);
            }
        }

        public List<DirectoryDetails> ListDirectoryDetails(string ftpUri, FtpDeploySettings settings)
        {
            var directoryDetailsList = _webService.ListDirectoryDetails(ftpUri, settings.FtpUserName, settings.FtpPassword);
            return directoryDetailsList;
        }

        public void DeleteFile(string ftpUri, FtpDeploySettings settings)
        {
            _webService.DeleteFile(ftpUri, settings.FtpUserName, settings.FtpPassword);
            Interlocked.Decrement(ref _requestNumber);
        }

        public void RemoveDirectory(string ftpUri, FtpDeploySettings settings)
        {
            _log.Information($"Removing directory: {ftpUri}\n");
            _webService.RemoveDirectory(ftpUri, settings.FtpUserName, settings.FtpPassword);
        }

        public void UploadFile(string ftpUri, string filePath, FtpDeploySettings settings)
        {
            _webService.UploadFile(ftpUri, filePath, settings.FtpUserName, settings.FtpPassword);
            Interlocked.Decrement(ref _requestNumber);
        }

        public void MakeDirectory(string ftpUri, FtpDeploySettings settings)
        {
            _log.Information($"Making directory: {ftpUri}\n");
            _webService.MakeDirectory(ftpUri, settings.FtpUserName, settings.FtpPassword);
        }
    }

    public class DirectoryDetails
    {
        public DirectoryDetailsType Type { get; set; }
        public string Name { get; set; }
    }

    public enum DirectoryDetailsType
    {
        Directory,
        File
    }
}