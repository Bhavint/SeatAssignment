# Theater Reservation

The application takes in reservation requests and assigns seats in a theater with predefines number of rows and a fixed number of seats in each row. The assignment is done offline. Several types of assignment algorithms and input output methods can be pre-configured to achieve different ditribution of assignments. Presently, it supports reading from a text file and writing to a text file. More on this in the configuration section.

# Assumptions

* The theater capacity is variable.
* The format for reservation request and seat assignment is clearly defined and the system does not attempt to curate input.
* Maximizing theater utilization implies that as many seats as possible should be assigned.
* Maximizing customer satisfaction implies:
  * All seat assignments to a single request must be contiguous i.e in the same row.
  * Requests should be processed in their order of arrival. (However this is a business decision. Accordingly the system provides for limited high level flexibility.)
* Theater utilization takes higher priority than customer satisfaction. The system will ensure contiguous assignment as far as possible but does not guarantee it.
* This is not supposed to be a distributed/multithreaded application. However, it can be extended with modifications.
* This is primarily ment for windows based systems. However, it can be modified to make it platform agnostic.

## Getting Started

You will need a variant of Microsoft Visual Studio 2013 or higher to develop, debug and write tests. 

### Prerequisites

* The project uses NuGet Package manager to manage external dependencies. 
Download nuget(v3.4.4) from https://dist.nuget.org/index.html.

* Download and Install .NET 4.5.2 from https://www.microsoft.com/en-us/download/details.aspx?id=42642

### Installing Development Environment

To setup a development environment, download Microsoft Visual Studio Community edition along with .NET 4.5.2 from https://www.visualstudio.com/free-developer-offers/ and follow the steps

## Configuration

### App Settings
App settings define certain parameters necessary to constraint the size of the input.
* NumberOfRows : Defines the number of rows in the theater
* SeatsInEachRow : Defines the number of seats in each row
* DefaultInputFilePath : Filepath to read input from in case no file path is given as a parameter
* DefaultOutputFilePath : Filepath to write output to case no file path is given as a parameter

### Ioc Container Configuration
This lets you pick between various implementations with different rules of assigning seats. For detailed explanation about the configuration schema, visit https://msdn.microsoft.com/en-us/library/dn507421(v=pandp.30).aspx 
The interesting bits lie in configuring various implementations of ITheaterManager. Replace the "mapTo" value in the       
<register type="ITheaterManager" mapTo="FairTheaterManager" name="mainTheaterManager">

* Simple Theater Manager: Assigns seats to reservation requests row wise in the sequence of their arrival starting from top left corner.
* Fair Theater Manager: Assigns all seats to reservation requests in the sequence of their arrival in the same row as far as possible while minimizing vacancies in each row.
* Other implementations decorate Simple/Fair Managers by prioritizing larger/smaller reservation requests.

## Building From Command Line
Step 1: Download Dependencies
You will need to download all external dependencies before starting the build. 
```
<drive letter>:\Path\To\nuget.exe restore Path\To\Solution\MySolution.sln
```
e.g
```
C:\Program Files\Nuget\nuget.exe restore G:\Projects\SeatAssignment\SeatAssignment.sln
```
Step 2: Build
.NET projects are built with MSBuild. Find the location of MSBuild.exe in your .NET installation directory or run the command line from Visual Studio Developer Command Prompt where MSBuild is preloaded command. 
```
<drive letter>:\Path\To\MSBuild.exe Path\To\Solution\MySolution.sln /t:Build /p:Configuration=Debug /p:TargetFramework=v4.0
```
e.g.
```
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe G:\Projects\SeatAssignment\SeatAssignment.sln /t:Build /p:Configuration=Debug /p:TargetFramework=v4.0
```
## Running the tests
Use MSTest.exe to run tests. Find the MSTest.exe file from your Visual Studio Installation or run through Visual Studio Developer Command Prompt where MSTest is a preloaded command.
```
<drive letter>:\Path\To\MSTest.exe /testcontainer:Path\To\TestContainer\MyTestContainer.dll /testsettings:Path\To\TestSettings\MyTestSettings.testsettings
```
```
G:\Projects\MSTest.exe /testcontainer:SeatAssignment.Tests/bin/debug/SeatAssignment.Tests.dll /testsettings:local.testsettings
```

## Running from Command Line
```
<drive letter>:\Path\To\Executable\MyExecutable.exe [Path\To\Input\File\MyInputFile.txt] [Path\To\OutputFile\MyOutputFile.txt]
```

e.g.

```
C:\> G:\Projects\SeatAssignment\SeatAssignment\bin\Debug\SeatAssignment.exe G:\Projects\sample-input G:\Projects\sample-output.txt
```
or simply 
```
C:\> G:\Projects\SeatAssignment\SeatAssignment\bin\Debug\SeatAssignment.exe
```

if you want to assign default file paths through the App Settings.

## Built With

* [Unity](https://github.com/unitycontainer/unity) - Inversion of Control and Dependency Injection
* [Nuget](https://www.nuget.org/) - Dependency Management

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/Bhavint/SeatAssignment/tags). 

## Authors

* Bhavin Thakkar

See also the list of [contributors](https://github.com/Bhavint/SeatAssignment/contributors) who participated in this project.
