# RPLidar4Net
Slamtec RPLidar API in C#

## Specifications
[Interface Protocol and Application Notes](https://download.slamtec.com/api/download/rplidar-protocol/2.1.1?lang=en)

Tested on RPLidar A1 http://www.slamtec.com/en/Lidar/A1
## Projects
### Libraries
| Name      | Description                                                              | TargetFramework    | NuGet                                                                                                                                      |
|-----------|--------------------------------------------------------------------------|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------|
| Api       | Data structures (Response Descriptors, Data Responses, ...), and Helpers | .Net Standard 2.0  | [![RPLidar NuGet version](https://img.shields.io/nuget/v/RPLidar4Net.Api.svg)](https://www.nuget.org/packages/RPLidar4Net.Api/)            |
| Core      | Base Library                                                             | .Net Standard 2.0  | [![RPLidar NuGet version](https://img.shields.io/nuget/v/RPLidar4Net.Core.svg)](https://www.nuget.org/packages/RPLidar4Net.Core/)          |
| Data.Dump | Scan Data Dump File Reading                                              | .Net Standard 2.0  | [![RPLidar NuGet version](https://img.shields.io/nuget/v/RPLidar4Net.DataDump.svg)](https://www.nuget.org/packages/RPLidar4Net.DataDump/)  |
| IO        | RPLidarSerialDevice (interactions with SerialPort)                       | .Net Standard 2.0  | [![RPLidar NuGet version](https://img.shields.io/nuget/v/RPLidar4Net.IO.svg)](https://www.nuget.org/packages/RPLidar4Net.IO/)              |

### Applications
| Name   | Description | Type            | TargetFramework      |
|--------|-------------|-----------------|----------------------|
| WpfApp | Tools       | WPF Application | .Net Framework 4.7.2 |

## Getting Started
### Prerequisites

- Visual Studio 2019

## Authors

* **Amael BERTEAU**

