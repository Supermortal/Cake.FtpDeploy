using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.FtpDeploy.Services
{
    public interface IWebService
    {
        void DeleteFile(string ftpUri, string username, string password);
        void RemoveDirectory(string ftpUri, string username, string password);
        void UploadFile(string ftpUri, string filePath, string username, string password);
        void MakeDirectory(string ftpUri, string username, string password);
        List<DirectoryDetails> ListDirectoryDetails(string ftpUri, string username, string password);
    }
}
