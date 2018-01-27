using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cake.FtpDeploy.Services
{
    public class WebService : IWebService
    {

        private readonly Regex _splitRegex = new Regex("\\s+", RegexOptions.Compiled);

        public void DeleteFile(string ftpUri, string username, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpUri);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(username, password);

            var response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        public void RemoveDirectory(string ftpUri, string username, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpUri);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            request.Credentials = new NetworkCredential(username, password);

            var response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        public void UploadFile(string ftpUri, string filePath, string username, string password)
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(username, password);
                client.UploadFile(ftpUri, "STOR", filePath);
            }
        }

        public void MakeDirectory(string ftpUri, string username, string password)
        {
            var webRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
            webRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            webRequest.Credentials = new NetworkCredential(username, password);

            var response = (FtpWebResponse)webRequest.GetResponse();
            response.Close();
        }

        public List<DirectoryDetails> ListDirectoryDetails(string ftpUri, string username, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpUri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(username, password);

            var directoryDetailsList = new List<DirectoryDetails>();
            var response = (FtpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                while (!streamReader.EndOfStream)
                {
                    var responseLine = streamReader.ReadLine();
                    var split = _splitRegex.Split(responseLine);

                    var directoryDetails = new DirectoryDetails()
                    {
                        Type = (split[0].StartsWith("d")) ? DirectoryDetailsType.Directory : DirectoryDetailsType.File,
                        Name = split.Last()
                    };
                    directoryDetailsList.Add(directoryDetails);
                }
            }
            response.Close();

            return directoryDetailsList;
        }

    }
}
