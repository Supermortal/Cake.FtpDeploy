using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.FtpDeploy.Services
{
    public class TestWebService : IWebService
    {
        public void DeleteFile(string ftpUri, string username, string password)
        {

        }

        public void RemoveDirectory(string ftpUri, string username, string password)
        {

        }

        public void UploadFile(string ftpUri, string filePath, string username, string password)
        {

        }

        public void MakeDirectory(string ftpUri, string username, string password)
        {

        }

        public List<DirectoryDetails> ListDirectoryDetails(string ftpUri, string username, string password)
        {
            if (ftpUri == "ftp://www.test.com")
            {
                return new List<DirectoryDetails>()
                {
                    new DirectoryDetails()
                    {
                        Name = "test.txt",
                        Type = DirectoryDetailsType.File
                    },
                    new DirectoryDetails()
                    {
                        Name = "test2.txt",
                        Type = DirectoryDetailsType.File
                    },
                    new DirectoryDetails()
                    {
                        Name = "test3.txt",
                        Type = DirectoryDetailsType.File
                    },
                    new DirectoryDetails()
                    {
                        Name = "testDirectory",
                        Type = DirectoryDetailsType.Directory
                    }
                };
            }
            else if (ftpUri == "ftp://www.test.com/testDirectory")
            {
                return new List<DirectoryDetails>()
                {
                    new DirectoryDetails()
                    {
                        Name = "test4.txt",
                        Type = DirectoryDetailsType.File
                    },
                    new DirectoryDetails()
                    {
                        Name = "test5.txt",
                        Type = DirectoryDetailsType.File
                    },
                    new DirectoryDetails()
                    {
                        Name = "test6.txt",
                        Type = DirectoryDetailsType.File
                    }
                };
            }

            return new List<DirectoryDetails>();
        }

    }
}
