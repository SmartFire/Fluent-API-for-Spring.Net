require 'albacore'
require 'rake'
require 'fileutils'

task :default => [:clean, :build, :coverage, :package]
task :nuget => [:clean, :build, :test, :package]

task :clean do
    if File.directory? Dir.pwd + "/package"
        FileUtils.rm_r Dir.pwd+"/package"
    end
end

msbuild :build do |msb|
    msb.properties :configuration => :Debug, :OutputPath => Dir.pwd + "/release"
    msb.verbosity = "quiet"
    msb.targets :Clean, :Build
    msb.solution = "../SpringContrib.sln"
end

nunit :test do |nunit|
    nunit.command  = "nunit-console-x86"
    nunit.assemblies Dir.pwd + "/release/FluentSpring.Tests.dll"
end

ncoverconsole :coverage do |ncc|
  ncc.command = "NCover.Console.exe"
  ncc.output :xml => "test-coverage.xml"
  ncc.working_directory = Dir.pwd + "/release"
  ncc.cover_assemblies("FluentSpring.dll")
  
  nunit = NUnitTestRunner.new("nunit-console-x86.exe")
  nunit.assemblies Dir.pwd + "/release/FluentSpring.Tests.dll"
  nunit.options '/noshadow'
		
  ncc.testrunner = nunit
end

exec :package do |cmd|
    
	Dir.mkdir Dir.pwd+"/package"
    FileUtils.cp Dir.pwd+"/release/FluentSpring.dll", Dir.pwd+"/package/FluentSpring.dll"
    FileUtils.cp Dir.pwd+"/release/Spring.Core.dll", Dir.pwd+"/package/Spring.Core.dll"
    FileUtils.cp Dir.pwd+"/release/Spring.Data.dll", Dir.pwd+"/package/Spring.Data.dll"
    FileUtils.cp Dir.pwd+"/release/Spring.Aop.dll", Dir.pwd+"/package/Spring.Aop.dll"
    FileUtils.cp Dir.pwd+"/release/Spring.Web.dll", Dir.pwd+"/package/Spring.Web.dll"
    FileUtils.cp Dir.pwd+"/release/Common.Logging.dll", Dir.pwd+"/package/Common.Logging.dll"
    FileUtils.cp Dir.pwd+"/release/Common.Logging.Log4Net.dll", Dir.pwd+"/package/Common.Logging.Log4Net.dll"
    FileUtils.cp Dir.pwd+"/release/log4net.dll", Dir.pwd+"/package/log4net.dll"
	FileUtils.cp Dir.pwd+"/nuget/web.config.transform", Dir.pwd+"/package/web.config.transform"
	FileUtils.cp Dir.pwd+"/nuget/app.config.transform", Dir.pwd+"/package/app.config.transform"

    cmd.command = "nuget"
    cmd.parameters = "pack fluentspring.nuspec"
end