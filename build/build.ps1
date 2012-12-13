Framework "4.0"
$buildDir=$psake.build_script_dir
$rootDir=Split-Path $buildDir
$tempDir=join-path $rootDir "temp"
$nugetOut=join-path $rootDir "nugets"
$nuget=join-path $buildDir "libs\nuget.exe"
$frameworkVersion="Net40"

$slnDir=$rootDir+"\src"
$slnFile=$slnDir+"\Caveman.sln"

task default -depends clean, compile-all, pack-caveman, pack-caveman-mvc

task clean{
   Write-Host "Cleaning..."
   exec { msbuild $slnFile /t:Clean /v:quiet }
   rd $tempDir -recurse
   mkdir $tempDir
}

task compile-all {
    exec { msbuild $slnFile /t:Build /p:Configuration=Release /v:quiet }
}

task pack-caveman {
    $current="CavemanTools"
	PreNuget $current
	"Updating version..."
	$script:cavemanVersion=[string](UpdateVersion $current)
	 "Building package..."
	 $nuspec=[string](GetNuspec $current)
	 & $nuget pack $nuspec -OutputDirectory $nugetOut
}

task pack-caveman-mvc {
    $current="CavemanTools.Mvc"
	PreNuget $current
	"Updating version..."
	UpdateVersion $current $script:cavemanVersion
	 "Building package..."
	 $nuspec=[string](GetNuspec $current)
	 & $nuget pack $nuspec -OutputDirectory $nugetOut
}

#-------------------------------------------- Functions ----------------------

function PreNuget ($current)
{
	$packDir=PackDir($current)
	mkdir $packdir
	xcopy  "$slnDir\$current\bin\Release\$current.*"  $packdir
	xcopy "$buildDir\$current.nuspec" "$tempDir\$current"
}

function UpdateVersion($current,$dep="")
{
   $nuspec= GetNuspec $current
   $specFile=[xml](Get-Content $nuspec)
   $asm= [string](PackDir $current) +"\$current.dll"
   $version=[string](GetVersion $asm)
   $specFile.package.metadata.version=$version
   if ($dep -ne "") {
        $specFile.package.metadata.dependencies.dependency.version=$dep
   }
   $specFile.Save($nuspec)
   return $version
}

function GetNuspec($current)
{
return "$tempDir\$current\$current.nuspec"
}

function PackDir($current)
{
return "$tempDir\$current\lib\$frameworkVersion"
}

function GetVersion($asm)
{
 [string] $version= [System.Diagnostics.FileVersionInfo]::GetVersionInfo($asm).ProductVersion.ToString()
 #Write-Host "$asm version is " $version
 return $version
}