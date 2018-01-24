using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.FtpDeploy.Services
{
    public interface IFtpService
    {
        void DeleteAll(FtpDeploySettings settings);
        void DeleteAllRecursive(string ftpUri, FtpDeploySettings settings, List<string> directoriesToRemove);
        void UploadAll(FtpDeploySettings settings);
        void UploadAllRecursive(int baseArtifactsPathLength, string directoryPath, FtpDeploySettings settings);
        List<DirectoryDetails> ListDirectoryDetails(string ftpUri, FtpDeploySettings settings);
        void DeleteFile(string ftpUri, FtpDeploySettings settings);
        void RemoveDirectory(string ftpUri, FtpDeploySettings settings);
        void UploadFile(string ftpUri, string filePath, FtpDeploySettings settings);
        void MakeDirectory(string ftpUri, FtpDeploySettings settings);
    }
}
