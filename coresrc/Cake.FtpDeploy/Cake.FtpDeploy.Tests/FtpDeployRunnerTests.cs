using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cake.FtpDeploy.Tests
{
    public class FtpDeployRunnerTests
    {

        private readonly FtpDeployFixture<FtpDeploySettings> _fixture;

        public FtpDeployRunnerTests()
        {
            _fixture = new FtpDeployFixture<FtpDeploySettings>();
        }

        [Fact]
        public void RunsWithCorrectVariables()
        {
            _fixture.Settings.FtpUri = "ftp://www.test.com";
            _fixture.Settings.FtpUserName = "testFtpUsername";
            _fixture.Settings.FtpPassword = "testFtpPassword";
            _fixture.Settings.ArtifactsPath = "./TestArtifacts";
            var result = _fixture.Run();
        }

        [Fact]
        public void ThrowsArgumentNullExceptionsOnAllEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Run());
        }

        [Fact]
        public void ThrowsArgumentNullExceptionsOnUsernameEmpty()
        {
            _fixture.Settings.FtpUri = "ftp://www.test.com";
            _fixture.Settings.FtpUserName = null;
            _fixture.Settings.FtpPassword = "testFtpPassword";
            _fixture.Settings.ArtifactsPath = "./TestArtifacts";
            Assert.Throws<ArgumentNullException>(() => _fixture.Run());
        }

        [Fact]
        public void ThrowsArgumentNullExceptionsOnPasswordEmpty()
        {
            _fixture.Settings.FtpUri = "ftp://www.test.com";
            _fixture.Settings.FtpUserName = "testFtpUsername";
            _fixture.Settings.FtpPassword = null;
            _fixture.Settings.ArtifactsPath = "./TestArtifacts";
            Assert.Throws<ArgumentNullException>(() => _fixture.Run());
        }

        [Fact]
        public void ThrowsArgumentNullExceptionsOnArtifactsEmpty()
        {
            _fixture.Settings.FtpUri = "ftp://www.test.com";
            _fixture.Settings.FtpUserName = "testFtpUsername";
            _fixture.Settings.FtpPassword = "testFtpPassword";
            _fixture.Settings.ArtifactsPath = null;
            Assert.Throws<ArgumentException>(() => _fixture.Run());
        }

        [Fact]
        public void ThrowsArgumentNullExceptionsOnArtifactsBadDirectory()
        {
            _fixture.Settings.FtpUri = "ftp://www.test.com";
            _fixture.Settings.FtpUserName = "testFtpUsername";
            _fixture.Settings.FtpPassword = "testFtpPassword";
            _fixture.Settings.ArtifactsPath = "some/non/existent/directory";
            Assert.Throws<ArgumentException>(() => _fixture.Run());
        }
    }
}
