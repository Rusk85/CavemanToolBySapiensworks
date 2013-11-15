//uncomment to include reference to other assembly you might need (one directive per each)
//#r "mycustom.dll"
using System;
using System.IO;
using CSake;
using NuGet;   //comment it out if you don't create nuget packs or manipulate nuspec files

//don't use namespaces
//any class you create will be considered an inner class of the CSakeWrapper class

const string SlnFile=@"../src/Caveman.sln";

const string SlnDir=@"../src";

const string TempDir=@"temp";

//for nuget
const string PackageDir = @"temp/package";

static string CurrentDir=Path.GetFullPath("./");

static string[] Projects=new[]{"CavemanTools","CavemanTools.Web","CavemanTools.MVC"};

public static void CleanUp()
{
    TempDir.CleanupDir();
    SlnFile.MsBuildClean();         
}


[Depends("CleanUp")] 
public static void Build()
{
    SlnFile.MsBuildRelease();
}

[Default]
[Depends("Build")] 
public static void Local()
{
    foreach(var project in Projects)
    {
        var releaseDir=Path.GetFullPath(Path.Combine(SlnDir,project,"bin/Release"));
        "robocopy".Exec(releaseDir,TempDir,"/E","/XN","/NS","/NC","/NJH","/NJS");
}

//------------------------------ Utils ----------------

//updates version in nuspec file
static void UpdateVersion(string nuspecFile,string assemblyName)
{
    var nuspec=nuspecFile.AsNuspec();   
    nuspec.Metadata.Version=GetVersion(assemblyName);
    nuspec.Save(TempDir);    
}

//basePath= relative path for package files source. Usually is the project dir.
static void BuildNuget(string nuspecFile,string basePath)
{
    if (!nuspecFile.EndsWith(".nuspec"))
    {
        nuspecFile+=".nuspec";
    }
    Path.Combine(TempDir,nuspecFile).CreateNuget(basePath,PackageDir);    
}

static string GetVersion(string asmName)
{
   throw new NotImplementedException();
   //return Path.Combine(ReleaseDir,asmName).GetAssemblyVersion().ToSemanticVersion().ToString();
}

