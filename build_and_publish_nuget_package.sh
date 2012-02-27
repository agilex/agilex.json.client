#!/bin/bash

# project vars
nuget='./tools/nuget.exe'
msbuild='/c/Windows/Microsoft.NET/Framework/v4.0.30319/msbuild.exe'
project_src_dir='./src/agilex.json.client'
build_mode='Release'
project_output_dir="$project_src_dir/bin/$build_mode"
build_dir="./build"
package_output_dir="./deploy"
package_spec_file="$build_dir/Package.nuspec"
build_log="$build_dir/build_log.txt"

# build project
$msbuild $project_src_dir/agilex.persistence.nhibernate.csproj /property:Configuration=$build_mode > $build_log

errors=`cat $build_log | grep -i "Build FAILED"`
if [ "" != "${errors}" ]
then
	echo "Build failed"
	exit -1
fi

# increment build version number
currentVersion=`grep version $package_spec_file | cut -d '>' -f2 | cut -d '<' -f1`
echo "Current assembly version $currentVersion"
let patch=`echo $currentVersion | cut -d '.' -f4`+1
suggestedVersion="`echo $currentVersion | cut -d '.' -f1,2,3`.${patch}"
echo "enter new version number eg. $suggestedVersion" [Enter to accept]
read -e newversion
if [ "" == "${newversion}" ]
then
 newversion=$suggestedVersion
fi
echo "Using $newversion"
sed -i -e s/\<version\>[0-9]*.[0-9]*.[0-9]*.[0-9]*\</\<version\>$newversion\</ $package_spec_file

# copy dlls to lib dir
mkdir -p ./build/lib/net40
cp $project_output_dir/agilex.json.client.dll $build_dir/lib/net40

# package
$nuget pack $package_spec_file -OutputDirectory $package_output_dir

# push to gallery (requires api key is set on command line)
$nuget push $package_output_dir/agilex.json.client.$newversion.nupkg