# Cake.FtpDeploy

![](https://ci.appveyor.com/api/projects/status/ky32ma3hw9ujidwl/branch/master?svg=true)

## Example Usage In Build.cake

```
FtpDeploy(new FtpDeploySettings() {
        FtpUri = "ftp://somebaseftpurl",
        FtpUserName = "some-ftp-username",
        FtpPassword = "some-ftp-password",
        ArtifactsPath = "relative-or-absolute-path-to-published-artifacts"
    });
```

This addin will delete all files at the ftp URI, and then upload every file in the ArtifactsPath directory

## To publish artifacts locally from a solution, create a Cake task similar to this:
```
Task("Publish Artifacts Locally")
    .Does(() =>
{
    MSBuild(
        "./GAS.sln", x => {

            x.ArgumentCustomization = args => args.Append("/p:DeployOnBuild=true").Append("/p:PublishProfile=FolderProfile");

            x
            .SetConfiguration("Release")
            .WithTarget("Build");
        });
});
```

The PublishProfile can be created in Visual Studio (it needs to be a Folder publish)
See more here: [https://docs.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio](https://docs.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio)
