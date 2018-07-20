using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.FtpDeploy.Services;
using NSubstitute;
using Xunit;

namespace Cake.FtpDeploy.Tests
{
    public class FtpServiceTests
    {

        private readonly ICakeLog _mockLog;
        private readonly IWebService _testWebService;
        private readonly FtpDeploySettings _testSettings;

        public FtpServiceTests()
        {
            _mockLog = Substitute.For<ICakeLog>();           
            _testSettings = new FtpDeploySettings()
            {
                FtpUri = "ftp://www.test.com",
                FtpUserName = "testFtpUsername",
                FtpPassword = "testFtpPassword",
                ArtifactsPath = "./TestArtifacts"
            };

            _testWebService = Substitute.For<IWebService>();
            var testWebService = new TestWebService();
            _testWebService.ListDirectoryDetails("ftp://www.test.com", Arg.Any<string>(), Arg.Any<string>())
                .Returns(testWebService.ListDirectoryDetails("ftp://www.test.com", null, null));
            _testWebService.ListDirectoryDetails("ftp://www.test.com/testDirectory", Arg.Any<string>(), Arg.Any<string>())
                .Returns(testWebService.ListDirectoryDetails("ftp://www.test.com/testDirectory", null, null));
        }

        [Fact]
        public void UploadFileIsCalledSixTimes()
        {
            var serviceUnderTest = new FtpService(_mockLog, _testWebService);
            serviceUnderTest.UploadAll(_testSettings);
            _testWebService.Received(6).UploadFile(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void MakeDirectoryIsCalled()
        {
            var serviceUnderTest = new FtpService(_mockLog, _testWebService);
            serviceUnderTest.UploadAll(_testSettings);
            _testWebService.Received().MakeDirectory(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void RemoveDirectoryIsCalled()
        {
            var serviceUnderTest = new FtpService(_mockLog, _testWebService);
            serviceUnderTest.DeleteAll(_testSettings);
            _testWebService.Received().RemoveDirectory(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteFileIsCalledSixTimes()
        {
            var serviceUnderTest = new FtpService(_mockLog, _testWebService);
            serviceUnderTest.DeleteAll(_testSettings);
            _testWebService.Received(6).DeleteFile(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
