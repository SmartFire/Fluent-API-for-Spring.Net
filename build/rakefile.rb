require 'albacore'
require 'rake'
require 'fileutils'

task :default => [:clean, :build, :coverage]
task :nuget_all, :fmk, :fmkdir, :needs => [:build, :test, :package, :nupack]

task :nuget do
    Rake::Task['clean'].invoke
    Rake::Task['build'].invoke("v3.5")
    Rake::Task['test'].invoke
    Rake::Task['package'].invoke("net35")
    
    Rake::Task['build'].reenable
    Rake::Task['test'].reenable
    Rake::Task['package'].reenable
    
    Rake::Task['build'].invoke("v4.0")
    Rake::Task['test'].invoke
    Rake::Task['package'].invoke("net40")
    
    Rake::Task['nupack'].invoke
end

task :clean do
    if File.directory? Dir.pwd + "/package"
        FileUtils.rm_r Dir.pwd + "/package"
    end
    if File.directory? Dir.pwd + "/release"
        FileUtils.rm_r Dir.pwd + "/release"
    end
end

msbuild :build, [:fmk] do |msb, args|
    args.with_defaults(:fmk=>"v3.5")
    
    msb.properties :configuration => :Release, :OutputPath => Dir.pwd + "/release", :TargetFrameworkVersion => args[:fmk]
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

task :package, [:fmkdir] do |nuget, args|
    args.with_defaults(:fmkdir=>"net35")

    if !File.directory? Dir.pwd + "/package"
        Dir.mkdir Dir.pwd+"/package" 
    end
    
	Dir.mkdir Dir.pwd+"/package/" + args[:fmkdir]
    
    FileUtils.cp Dir.pwd+"/release/FluentSpring.dll", Dir.pwd+"/package/" + args[:fmkdir] + "/FluentSpring.dll"
    FileUtils.cp Dir.pwd+"/release/Spring.Data.dll", Dir.pwd+"/package/" + args[:fmkdir] + "/Spring.Data.dll"
    FileUtils.cp Dir.pwd+"/release/Spring.Web.dll", Dir.pwd+"/package/" + args[:fmkdir] + "/Spring.Web.dll"

end

nugetpack :nupack, [:fmkdir] do |nuget, args|
	FileUtils.cp Dir.pwd+"/nuget/web.config.transform", Dir.pwd+"/package/web.config.transform"
	FileUtils.cp Dir.pwd+"/nuget/app.config.transform", Dir.pwd+"/package/app.config.transform"

    nuget.nuspec = "fluentspring.nuspec"
end