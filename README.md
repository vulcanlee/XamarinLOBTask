# .NET Core 與  C# 開發實戰：新手上路篇
這裡將會存放 `.NET Core 與  C# 開發實戰：新手上路篇` 課程所會用到的課程練習與測驗之相關專案原始碼

練習專案說明

|專案名稱|專案說明|
|-|-|
|||
|||
|||
|||
|||
|||

# 實作練習說明重點摘要

# \[VS2017Core] 使用 VS 2017 建立 .NET Core 2.0 專案

* 開啟 Visual Studio 2017 
* 建立 VS2017Core 空白方案
* 建立 MyLibrary 類別庫(.NET Standard)
* 設計 Sum 類別與方法

```csharp
namespace MyLibrary
{
    public class MyClass
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}
```

* 建立 MyLibrary.Test 單元測試專案 (.NET Core)
* 在單元測試專案加入參考 MyLibrary 專案
* 設計測試類別 MyLibraryTest

```csharp
namespace MyLibrary.Test
{
    [TestClass]
    public class MyLibraryTest
    {
        [TestMethod]
        public void TestSum()
        {
            #region Arrange 排列
            MyClass fooMyLibrary = new MyClass();

            int expected = 5;
            #endregion

            #region Act 作用
            int actual = fooMyLibrary.Sum(2,3);
            #endregion

            #region Asset 判斷提示
            Assert.AreEqual(expected, actual);
            #endregion
        }
    }
}
```

* 使用測試總管進行單元測試
* 建立 MyConsole 主控台應用程式 (.NET Core)專案
* 在主控台應用程式專案加入參考 MyLibrary 專案
* 設計主控台應用程式

```csharp
namespace MyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello .NET Core");
            MyLibrary.MyClass foo = new MyLibrary.MyClass();
            Console.WriteLine($"2 + 3 = {foo.Sum(2,3)}");
            Console.ReadKey();
        }
    }
}
```

* 執行看結果

# \[CLICore] 使用 VS Code / CLI 建立 .NET Core 2.0 專案

## 使用 VS Code / CLI 建立 .NET Core 2.0 專案 - 1

* 開啟 Visual Studio Code
* 按一下左側功能表上的 [檔案總管] 圖示，然後按一下 [開啟資料夾]
* 從主功能表 檢視 > 整合的終端機，開啟整合式終端機
* dotnet new sln 
  > 建立方案
* dotnet new classlib -f netstandard2.0 -n MyLibrary
  > 建立類別庫
* dotnet sln CLICore.sln add MyLibrary/MyLibrary.csproj
  > 加入類別庫到方案
設計 Sum 類別與方法
dotnet build MyLibrary/MyLibrary.csproj
  > 建置該類別庫方案

## 使用 VS Code / CLI 建立 .NET Core 2.0 專案 - 2

* dotnet new mstest -n MyLibrary.Test
  > 建立單元測試專案
* dotnet sln CLICore.sln add MyLibrary.Test/MyLibrary.Test.csproj
  > 單元測試專案加入到方案
* dotnet add MyLibrary.Test/MyLibrary.Test.csproj reference MyLibrary/MyLibrary.csproj
  > 單元測試專案參考 MyLibrary 專案
* 設計單元測試程式碼
* dotnet build MyLibrary.Test/MyLibrary.Test.csproj
  > 建置測試專案
* dotnet test MyLibrary.Test/MyLibrary.Test.csproj
  > 執行單元測試


## 使用 VS Code / CLI 建立 .NET Core 2.0 專案 - 3

* dotnet new console -n MyConsole
  > 建立主控台應用程式專案
* dotnet sln CLICore.sln add MyConsole/MyConsole.csproj
  > 主控台應用程式專案加入到方案
* dotnet add MyConsole/MyConsole.csproj reference * MyLibrary/MyLibrary.csproj
  > 主控台應用程式專案參考 MyLibrary 專案
設計主控台應用程式程式碼
* dotnet build MyConsole/MyConsole.csproj
  > 主控台應用程式專案
* dotnet .\MyConsole\bin\Debug\netcoreapp2.0\MyConsole.dll 

  或者

  dotnet run -p MyConsole/MyConsole.csproj
  > 執行主控台應用程式




# \[TypeForwarding] 如何使用型別轉送

## 第一階段

* 建立組件 AssemblyA

  命名空間為 MyAssembly，定義出 SayHello 類別

```csharp
namespace MyAssembly
{
    public class SayHello
    {
        public static string Hello(string name)
        {
            return $"Hello {name}";
        }
    }
}
```

* 建立 .NET Core 主控台專案

  參考組件 AssemblyA，並且呼救 MyAssembly.SayHello.Hello() 方法，最後查看 MyAssembly.SayHello 所在組件詳細資訊

```csharp
using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MyAssembly.SayHello.Hello("Vulcan"));
            Console.WriteLine(typeof(MyAssembly.SayHello).AssemblyQualifiedName);
            Console.ReadKey();
        }
    }
}
```

* 執行結果

  請注意，這裡結果說明 MyAssembly.SayHello 這個方法，使用的是組件 AssemblyA 內的方法

```
Hello Vulcan
MyAssembly.SayHello, AssemblyA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
```

## 第二階段

* 建立組件 AssemblyB

  將組建 AssemblyA 內的類別定義全部搬移到組件 AssemblyB

* 在組件 AssemblyA 加入參考組件 AssemblyB

* 在組件 AssemblyA 使用 TypeForwardedToAttribute
 指定型別轉送

```csharp
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(MyAssembly.SayHello))]
```

* 執行結果

  請注意，這裡結果說明 MyAssembly.SayHello 這個方法，使用的是組件 AssemblyB 內的方法，雖然主控台應用程式只有參考到組件 AssemblyA，沒有參考到組件 AssemblyB，而且組件 AssemblyA 內是沒有任何關於 MyAssembly.SayHello 定義。

```
Hello Vulcan
MyAssembly.SayHello, AssemblyB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
```

# \[NETPackage] 使用物件瀏覽器查看查看中繼套件與實作組件

* 建立 .NET Framework 專案，使用物件瀏覽器查看 List<T>組件相依性，並確認實作API組件來源

* 建立 .NET Core 專案，使用物件瀏覽器查看 List<T>組件相依性，並確認實作API組件來源

* 建立 .NET Standard 類別庫專案，使用物件瀏覽器查看 List<T>組件相依性

* 建立 PCL 類別庫專案，使用物件瀏覽器查看 List<T>組件相依性

* 在 .NET Core 與 .NET Framework 主控台程式中，加入底下程式碼

```csharp
static void Main(string[] args)
{
    Console.WriteLine("Hello .NET Core");
    Console.WriteLine("List<string> 使用組件的資訊");
    Console.WriteLine(
       typeof(System.Collections.Generic.List<string>).AssemblyQualifiedName);
    Console.WriteLine();
    Console.WriteLine("List<string> 使用組件所在路徑");
    Console.WriteLine(
       typeof(System.Collections.Generic.List<string>).Assembly.Location);
    Console.ReadKey();
}
```

# \[NETClassLibrary] 不同平台下需要共用類別庫

* 在 .NET Core 專案內

  這個專案內有參考到 .NET Core / .NET Framework / .NET Standard 的類別庫

  Program.cs 檔案內，若執行到 `new NETFrameworkClassLibrary.FrameworkClass();` 這個敘述，由於類別 FrameworkClass 是定義在 .NET Framework 類別庫內，在這裡使用了 .NET Framework 內的 Bitmap 類別，而在 .NET Core 中，是沒有實作這個類別，因此，會發生問題。

  不過，參考與使用 .NET Standard 的任何自訂類別與方法，是不會產生問題的。

# \[NETSAP] 使用共用的專案來開發 

體驗與學習 SAP Shared Asset Project 的開發過程

* Starter 資料夾內，已經建立了 .NET Framework / .NET Core 2.0 / Xamarin.Forms (Android / iOS / UWP) 這五種可執行專案

* Solution 資料夾內將會
  
  * 加入一個 SAP 共用專案

  * 建立一個 SayHello 類別與 Hello 方法(會傳回一個字串)

    Hello 方法會依據使用不同條件式編譯指示詞，回傳不同字串內容

    `練習在 Visual Studio 內的程式碼編輯器，如何切換不同編譯指示詞`

  * 在這五個專案內，加入參考此 SAP 專案

  * 使用這 SAP 共用專案提供的類別方法

  * 實際執行並查看結果 (.NET Framework / .NET Core / UWP / Android)

# \[NETPCL] 使用可攜式類別庫專案來開發 

* Starter 資料夾內，已經建立了 .NET Framework / .NET Core 2.0 / Xamarin.Forms (Android / iOS / UWP) 這五種可執行專案

* Solution 資料夾內將會
  
  * 加入一個 PCL 可攜式專案 ( PCLLibrary )

    加入可攜式類別庫對話窗，可以隨便選取

    修正使用 Profile259

  * 建立一個 SayHello 類別與 Hello 方法(會傳回一個字串)

  * 在這五個專案內，加入參考此 PCL 可攜式專案

  * 使用這 PCL 可攜式專案提供的類別方法

  * 實際執行並查看結果 (.NET Framework / .NET Core / UWP / Android)

# \[MultiTarget] 多目標架構專案開發

* 設計一個類別函數，可以針對不同架構，回傳不同的字串內容

* 建立三個專案：.NET Standard / .NET Core / .NET Framework 4.6.1 ，並且在兩個 Console 專案中，加入參考 .NET Standard 專案，實際執行看看結果 (可以參考 Starter 資料夾)

* 在 .NET Standard 專案加入支援多架構 net45 / netcoreapp2.0 (需要修正 .csproj)

```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>netstandard2.0;netcoreapp2.0;net45</TargetFrameworks>
  </PropertyGroup>
</Project> 
```

且修正類別原始碼，加入條件編譯指示詞，看看執行結果

# \[MultiTarget] 多目標架構專案開發 更多不同版本架構參考深入研究

* 建立一個 .NET Framework 4.5 專案加入 .NET Standard，看看執行結果

  `執行結果`

  '..\NETStandardLibrary\NETStandardLibrary.csproj' 以 'netstandard2.0;netcoreapp2.0;net461' 為目標，無法供目標為 '.NETFramework,Version=v4.5' 的專案參考。	NETFramework45Console


* 建立一個 .NET Framework 4.7 專案加入 .NET Standard，看看執行結果

  `執行結果`
  
  This is a .NET Framework 4.7 Console App

  Hi, NET461

* 建立一個 .NET Core 1.1 專案加入 .NET Standard，看看執行結果

  `執行結果`
  
  專案 NETStandardLibrary 與 netcoreapp1.1 (.NETCoreApp,Version=v1.1) 不相容。 
  
  專案 NETStandardLibrary 支援:
  - net461 (.NETFramework,Version=v4.6.1)
  - netcoreapp2.0 (.NETCoreApp,Version=v2.0)
  - netstandard2.0 (.NETStandard,Version=v2.0)

# \[WebHttpClient] 使用跨平台工具開發程式庫 WebClient /HttpClient

* 修正 NETStandardLibrary 專案內的 csproj 定義

```XML
<Project Sdk="Microsoft.NET.Sdk">

  <!--<PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>-->

  <PropertyGroup>
    <TargetFrameworks>netstandard2.0;net40;net45</TargetFrameworks>
  </PropertyGroup>

  <!-- 在這裡，指明 .NET Framework 4.0 架構下，需要參考 System.Net 組件 -->
  <ItemGroup Condition="'$(TargetFramework)' == 'net40'">
    <Reference Include="System.Net" />
  </ItemGroup>

  <!-- 在這裡，指明 .NET Framework 4.5 架構下，需要參考 System.Net.Http 與 System.Threading.Tasks 組件 -->
  <ItemGroup Condition="'$(TargetFramework)' == 'net45'">
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Threading.Tasks" />
  </ItemGroup>
</Project>
```

* 將 MultitargetLib 類別，使用前置處理指示詞，設計出 .NET 4.0 / .NET 4.5 用的抓取網頁資料的方法。

  在 .NET 4.0 裡面，需要使用 WebClient

  在 .NET 4.5 裡面，需要使用 HttpClient

```csharp
using System;
using System.Text.RegularExpressions;
#if NET40
// This only compiles for the .NET Framework 4 targets
using System.Net;
#else
// This compiles for all other targets
using System.Net.Http;
using System.Threading.Tasks;
#endif

namespace NETStandardLibrary
{
    public class MultitargetLib
    {
#if NET40
        private readonly WebClient _client = new WebClient();
        private readonly object _locker = new object();
#else
        private readonly HttpClient _client = new HttpClient();
#endif

#if NET40
        // .NET Framework 4.0 並沒有支援 async/await 用法
        public string GetDotNetCount(string url)
        {
            var uri = new Uri(url);

            string result = "";

            // 避免多執行緒同時呼叫這個方法，因此，要修正這個方法具有執行緒安全的特性 
            lock (_locker)
            {
                result = _client.DownloadString(uri);
            }

            int dotNetCount = Regex.Matches(result, ".NET").Count;

            Console.WriteLine("這裡執行的方法是 GetDotNetCount");
            return $"在這裡提到 .NET 共有 {dotNetCount} 次!";
        }
#else
        // .NET 4.5+ 就有支援非同步程式設計 async/await!
        public async Task<string> GetDotNetCountAsync(string url)
        {
            // HttpClient 是執行緒安全的，因此，不需要特別做 lock
            var result = await _client.GetStringAsync(url);

            int dotNetCount = Regex.Matches(result, ".NET").Count;

            Console.WriteLine("這裡執行的方法是 GetDotNetCountAsync");
            return $"在這裡提到 .NET 共有 {dotNetCount} 次!";
        }
#endif
    }
}
```

* 修正 Console App 的 Main 方法

  * 若需要使用 async / await，需要設定使用 C# 7.1

  * 根據 Visual Studio 提示，點選燈泡做修正

  * .NET 4.0 同步方法呼叫 

```csharp
static void Main(string[] args)
{
    Console.WriteLine("This is a .NET Framework 4.0 Console App");
    var foo = new NETStandardLibrary.MultitargetLib();
    var fooResult = foo.GetDotNetCount("http://www.dotnetfoundation.org/");
    Console.WriteLine(fooResult);
    Console.WriteLine("Press any key for continuing...");
    Console.ReadKey();
}

```

  * .NET 4.5 非同步呼叫

```csharp
static async Task Main(string[] args)
{
    Console.WriteLine("This is a .NET Framework 4.6.1 Console App");
    var foo = new NETStandardLibrary.MultitargetLib();
    var fooResult = await foo.GetDotNetCountAsync("http://www.dotnetfoundation.org/");
    Console.WriteLine(fooResult);
    Console.WriteLine("Press any key for continuing...");
    Console.ReadKey();
}
```

# \[NETCoreFDD] 使用 Visual Studio / CLI 相依的部署 (FDD)

* Visual Studio 2017

  滑鼠右擊專案節點 > [發行]

* CLI
  切換到專案目錄

  dotnet publish -c Release

  dotnet publish -c Debug

# \[NETCoreSCD] 使用 CLI 自封式部署 (SCD)

## 產生自封式部署檔案

* 切換到該專案目錄下

* dotnet publish -c Release -r win10-x64

* dotnet publish -c Release -r linux-x64

* dotnet publish -c Release -r rhel-x64

* dotnet publish -c Release -r osx-x64

## 建立 Azure RedHat 的 VM 相關資料紀錄

* Red Hat Enterprise Linux 7.4

* Name : NetCoreCourseLinux

* user name : vulcan

* Authentication type => password : DX2s!s842#yO@s24

* IP : 52.191.173.168

* 切換成為 Root : sudo su -

## 使用 scp 工具，將 publish 目錄上傳到 vulcan 的根目錄下

* cd publish

* chmod +x NETCoreSCD

## 第一次執行 NETCoreSCD 執行檔案

* ./NETCoreSCD

* 會出現

```
Failed to loadnc, error: libunwind.so.8: cannot open shared object file: No such file or directory
Failed to bind to CoreCLR at '/home/vulcan/publish/libcoreclr.so'
```

## 安裝需要的套件

* 安裝 yum install libunwind

## 第二次執行 NETCoreSCD 執行檔案

* ./NETCoreSCD

  得到底下錯誤訊息

```
FailFast: Couldn't find a valid ICU package installed on the system. Set the configuration flag System.Globalization.Invariant to true if you want to run with no globalization support.

   at System.Environment.FailFast(System.String)
   at System.Globalization.GlobalizationMode.GetGlobalizationInvariantMode()
   at System.Globalization.GlobalizationMode..cctor()
   at System.Globalization.CultureData.CreateCultureWithInvariantData()
   at System.Globalization.CultureData.get_Invariant()
   at System.Globalization.CultureData.GetCultureData(System.String, Boolean)
   at System.Globalization.CultureInfo.InitializeFromName(System.String, Boolean)
   at System.Globalization.CultureInfo.Init()
   at System.Globalization.CultureInfo..cctor()
   at System.StringComparer..cctor()
   at System.AppDomainSetup.SetCompatibilitySwitches(System.Collections.Generic.IEnumerable`1<System.String>)
   at System.AppDomain.PrepareDataForSetup(System.String, System.AppDomainSetup, System.Security.Policy.Evidence, System.Security.Policy.Evidence, IntPtr, System.String, System.String[], System.String[])
Aborted (core dumped)
```

* 安裝 yum install libicu

## 第三次執行 NETCoreSCD 執行檔案

* ./NETCoreSCD

  可以正常執行，得到底下輸出

```
./NETCoreSCD
Hello World! 自封式部署 (SCD)
```

## 這是 RedHat 需要的相關套件

* https://github.com/dotnet/core/blob/master/Documentation/build-and-install-rhel6-prerequisites.md

* yum install epel-release libunwind openssl libnghttp2 libidn krb5 libuuid lttng-ust zlib libicu

```
Dependency Installed:
  libicu.x86_64 0:50.1.2-15.el7
  rh-dotnet20-dotnet.x86_64 0:2.0.3-4.el7
  rh-dotnet20-dotnet-host.x86_64 0:2.0.3-4.el7
  rh-dotnet20-dotnet-runtime-2.0.x86_64 0:2.0.3-4.el7
  rh-dotnet20-dotnet-sdk-2.0.x86_64 0:2.0.3-4.el7
  rh-dotnet20-libcurl.x86_64 0:7.47.1-1.3.el7
  rh-dotnet20-lttng-ust.x86_64 0:2.8.1-3.el7
  rh-dotnet20-runtime.x86_64 0:2.0-6.el7
  rh-dotnet20-userspace-rcu.x86_64 0:0.9.2-3.el7
```

## 執行這個 SCD 執行檔案

* ./NETCoreSCD

# 建立與發佈 Nuget 套件

## 建立 .NET Standard 套件專案

* 專案名稱
 
  ContosoLibrary

* 方案名稱

  ContosoPack

* 建立類別與方法

```csharp
public class SayHello
{
    public string Hello()
    {
        return $"Hello Vulcan";
    }
}
```

## 編輯 .csproj

* 設定該 NuGet 套件的說明與參考資訊

* 設定儲存庫來源與型態

* 建置時候，要建立有除錯符號的 NuGet 套件

* 加入可以修正使用來源連結 Source Link 支援功能的套件

```XML
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    
    <Company>Contoso Inc.</Company>
    <Authors>Vulcan Lee</Authors>
    <Version>1.0.0</Version>
    <Description>展示如何建立、發佈、原始碼除錯 NuGet 套件的練習範例專案</Description>

    <PackageProjectUrl>https://github.com/vulcanlee/DotnetCoreContosoPack</PackageProjectUrl>
    
    <RepositoryType>git</RepositoryType>
    <RepositoryUrl>https://github.com/vulcanlee/DotnetCoreContosoPack.git</RepositoryUrl>
    
    <IncludeSymbols>true</IncludeSymbols>
    <IncludeSource>true</IncludeSource>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="SourceLink.Create.CommandLine" Version="2.6.0" PrivateAssets="All" />
  </ItemGroup>
</Project>
```

## 建立 GitHub 儲存庫 Repository 並且 Commit 進去

這個動作一定要做，否則，底下的步驟在執行過程中，會有錯誤產生

## 修正 NuGet 套件能夠有連結來源支援

* 在專案目錄下，開啟命令提示視窗

* 輸入底下 CLI 指令

```
dotnet restore
dotnet build /p:ci=true /v:n
```

* 建置時的 CLI 輸出文字

```
D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library>dotnet restore
  Restore completed in 20.78 ms for D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\Contoso.Library.csproj.

D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library>dotnet build /p:ci=true /v:n
Microsoft (R) Build Engine for .NET Core 15.5.179.9764 版
Copyright (C) Microsoft Corporation. 著作權所有，並保留一切權利。

已經開始建置於 2017/12/12 下午 05:07:52。
     1>節點 1 (Restore 目標) 上的專案 "D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\Contoso.Library.csproj"。
     1>Restore:
         Committing restore...
         Assets file has not changed. Skipping assets file writing. Path: D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\obj\project.assets.json
         Restore completed in 14.54 ms for D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\Contoso.Library.csproj.

         NuGet Config files used:
             C:\Users\vulca\AppData\Roaming\NuGet\NuGet.Config
             C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config
             C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.Fallback.config

         Feeds used:
             C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\
             https://api.nuget.org/v3/index.json
             https://www.myget.org/F/netcorepublic/api/v3/index.json
     1>專案 "D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\Contoso.Library.csproj" (Restore 目標) 建置完成。
  1:10>節點 1 (Build 目標) 上的專案 "D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\Contoso.Library.csproj"。
     1>SourceLinkCreate:
         git rev-parse --show-toplevel
         D:/Vulcan/GitHub/DotnetCoreContosoPack0
         git config --get remote.origin.url
         https://github.com/vulcanlee/DotnetCoreContosoPack
         git rev-parse HEAD
         eb0aa6366c0e4eb9caf4cc8b929a4ea5cd352fe6
         SourceLinkUrl: https://raw.githubusercontent.com/vulcanlee/DotnetCoreContosoPack/{commit}/*
       GenerateTargetFrameworkMonikerAttribute:
       將略過目標 "GenerateTargetFrameworkMonikerAttribute"，因為所有輸出檔對於其輸入檔而言都已更新。
       CoreGenerateAssemblyInfo:
       將略過目標 "CoreGenerateAssemblyInfo"，因為所有輸出檔對於其輸入檔而言都已更新。
       CoreCompile:
         C:\Program Files\dotnet\dotnet.exe "C:\Program Files\dotnet\sdk\2.1.2\Roslyn\bincore\csc.dll" /noconfig /unsafe- /checked- /nowarn:1701,1702,1705,1701,1702 /nostdlib+ /errorreport:prompt /warn:4 /define:TRACE;DEBUG;NETSTANDARD2_0 /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\Microsoft.Win32.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\mscorlib.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\netstandard.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.AppContext.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Collections.Concurrent.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Collections.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Collections.NonGeneric.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Collections.Specialized.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ComponentModel.Composition.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ComponentModel.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ComponentModel.EventBasedAsync.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ComponentModel.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ComponentModel.TypeConverter.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Console.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Core.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Data.Common.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Data.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.Contracts.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.Debug.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.FileVersionInfo.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.Process.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.StackTrace.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.TextWriterTraceListener.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.Tools.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.TraceSource.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Diagnostics.Tracing.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Drawing.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Drawing.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Dynamic.Runtime.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Globalization.Calendars.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Globalization.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Globalization.Extensions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.Compression.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.Compression.FileSystem.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.Compression.ZipFile.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.FileSystem.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.FileSystem.DriveInfo.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.FileSystem.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.FileSystem.Watcher.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.IsolatedStorage.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.MemoryMappedFiles.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.Pipes.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.IO.UnmanagedMemoryStream.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Linq.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Linq.Expressions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Linq.Parallel.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Linq.Queryable.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.Http.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.NameResolution.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.NetworkInformation.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.Ping.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.Requests.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.Security.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.Sockets.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.WebHeaderCollection.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.WebSockets.Client.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Net.WebSockets.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Numerics.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ObjectModel.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Reflection.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Reflection.Extensions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Reflection.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Resources.Reader.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Resources.ResourceManager.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Resources.Writer.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.CompilerServices.VisualC.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Extensions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Handles.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.InteropServices.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.InteropServices.RuntimeInformation.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Numerics.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Serialization.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Serialization.Formatters.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Serialization.Json.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Serialization.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Runtime.Serialization.Xml.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Claims.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Cryptography.Algorithms.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Cryptography.Csp.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Cryptography.Encoding.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Cryptography.Primitives.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Cryptography.X509Certificates.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.Principal.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Security.SecureString.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ServiceModel.Web.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Text.Encoding.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Text.Encoding.Extensions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Text.RegularExpressions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.Overlapped.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.Tasks.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.Tasks.Parallel.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.Thread.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.ThreadPool.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Threading.Timer.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Transactions.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.ValueTuple.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Web.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Windows.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.Linq.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.ReaderWriter.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.Serialization.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.XDocument.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.XmlDocument.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.XmlSerializer.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.XPath.dll /reference:C:\Users\vulca\.nuget\packages\netstandard.library\2.0.0\build\netstandard2.0\ref\System.Xml.XPath.XDocument.dll /debug+ /debug:portable /filealign:512 /nologo /optimize- /out:obj\Debug\netstandard2.0\Contoso.Library.dll /target:library /warnaserror- /utf8output /deterministic+ /sourcelink:obj\Debug\netstandard2.0\sourcelink.json SayHello.cs "C:\Users\vulca\AppData\Local\Temp\.NETStandard,Version=v2.0.AssemblyAttributes.cs" obj\Debug\netstandard2.0\Contoso.Library.AssemblyInfo.cs /warnaserror+:NU1605
       CopyFilesToOutputDirectory:
         正在將檔案從 "obj\Debug\netstandard2.0\Contoso.Library.dll" 複製到 "bin\Debug\netstandard2.0\Contoso.Library.dll"。
         Contoso.Library -> D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\bin\Debug\netstandard2.0\Contoso.Library.dll
         正在將檔案從 "obj\Debug\netstandard2.0\Contoso.Library.pdb" 複製到 "bin\Debug\netstandard2.0\Contoso.Library.pdb"。
       GenerateNuspec:
         已成功建立封裝 'D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\bin\Debug\Contoso.Library.1.0.0.nupkg' 。

         正在嘗試為 'Contoso.Library.csproj' 建置符號封裝。
         已成功建立封裝 'D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\bin\Debug\Contoso.Library.1.0.0.symbols.nupkg'。
     1>專案 "D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library\Contoso.Library.csproj" (Build 目標) 建置完成。

建置成功。
    0 個警告
    0 個錯誤

經過時間 00:00:02.60

D:\Vulcan\GitHub\DotnetCoreContosoPack0\Contoso.Library>
```

## 發佈 NuGet 套件到 MyGet

```
dotnet nuget push Contoso.Library.1.0.0.nupkg -k e04cbf5b-d5f3-4881-ac2d-0e002b67d879 -s https://www.myget.org/F/netcorepublic/api/v2/package

dotnet nuget push Contoso.Library.1.0.0.symbols.nupkg -k e04cbf5b-d5f3-4881-ac2d-0e002b67d879 -s https://www.myget.org/F/netcorepublic/symbols/api/v2/package
```


[![I ♥ Xamarin](https://4.bp.blogspot.com/-hS_XgJO3OGg/Wq0Gn0kPU2I/AAAAAAAANKs/G-SXFj-evrE8lGdcicWv7SC3-f6wyi4sgCEwYBhgL/s320/ILoveXamarin.png)](https://mylabtw.blogspot.com)

[Xamarin 實驗室 部落格](http://mylabtw.blogspot.com/) 是作者本身的部落格，這個部落格將會專注於 Xamarin 之跨平台 (Android / iOS / UWP) 方面的各類開技術探討、研究與分享的文章，最重要的是，它是全繁體中文。

