# Arquitetura mínima
## Centralizando as informações através de uma API
Como estou usando o a versão 9.0.300 do .NET, precisei atualizar as dependências do projeto (e o framework alvo também).
```XML
<!-- Arquivo ScreenSound.csproj -->
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <!-- Resto do código -->
  </PropertyGroup>
  
  <!-- Resto do código -->

  <ItemGroup>
    <PackageReference Include="Azure.Identity" Version="1.11.4" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.3" />
    <!-- Resto do código -->
    <PackageReference Include="Microsoft.IdentityModel.JsonWebTokens" Version="8.0.1" />
    <PackageReference Include="System.Formats.Asn1" Version="6.0.1" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.0.1" />
    <PackageReference Include="System.Text.Json" Version="8.0.5" />
  </ItemGroup>

</Project>
```

Agora, vamos criar uma migration chamada `PreparandoBanco` e aplicar todas as migrações com o comando `Update-Database` no Console do Gerenciador de Pacotes:
```bash
PM> Add-Migration PreparandoBanco
Build started...
Build succeeded.
To undo this action, use Remove-Migration.

PM> Update-Database
Build started...
Build succeeded.
Applying migration '20231201055531_projetoInicial'.
Applying migration '20231201060246_PopularTabela'.
Applying migration '20231201060709_AdicionarColunaAnoLancamento'.
Applying migration '20231201063138_PopularMusicas'.
Applying migration '20231201064222_RelacionarArtistaMusica'.
Applying migration '20250617230243_PreparandoBanco'.
Done.
PM> 
```
## Centralizando as informações através de uma API
Vamos criar um projeto cujo template se chama `ASP.NET Core vazio`. O projeto vai isolar o código referente à API.

É possível criar o projeto a partir da linha de comando ao invés de usar a IDE. Basicamente precisaremos: 
1. Criar o diretório com o nome do projeto;
2. Copiar um template de projeto para o diretório recém criado; e
3. Adicionar o diretório com o projeto na solução.

A criação do diretório é simples:
```bash
PS D:\alura\dotnet-criando-api-aspnet-core> mkdir ScreenSound.API


    Diretório: D:\alura\dotnet-criando-api-aspnet-core


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        17/06/2025     20:18                ScreenSound.API
```

O comando da CLI `dotnet new list` lista todos os tipos de projeto existentes. No caso do template `ASP.NET Core vazio`, o nome dele é `web`. Então, usaremos o comando a seguir para criar um novo projeto ASP.NET Core vazio:

```bash
PS D:\alura\dotnet-criando-api-aspnet-core> cd ScreenSound.API
PS D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.API> dotnet new web
O modelo "ASP.NET Core Vazio" foi criado com êxito.

Processando ações pós-criação...
Restaurando D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.API\ScreenSound.API.csproj:
A restauração foi bem-sucedida.
```

Agora, voltaremos ao diretório raiz da solução e adicionaremos o projeto recém criado à solução:
```bash
PS D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.API> cd ..
PS D:\alura\dotnet-criando-api-aspnet-core> dotnet solution add .\ScreenSound.API\
O projeto ‘ScreenSound.API\ScreenSound.API.csproj’ foi adicionado à solução.
PS D:\alura\dotnet-criando-api-aspnet-core> dotnet new list
```

O comando abaixo mostra todos os templates disponíveis para criação:
```
PS D:\alura\dotnet-criando-api-aspnet-core> dotnet new list
Esses modelos correspondem à sua entrada: 

Nome do modelo                                          Nome Curto                  Idioma      Tags
------------------------------------------------------  --------------------------  ----------  ----------------------------------
API Web do ASP.NET Core                                 webapi                      [C#],F#     Web/WebAPI/Web API/API/Service    
Aplicativo Autônomo Blazor WebAssembly                  blazorwasm                  [C#]        Web/Blazor/WebAssembly/PWA        
Aplicativo do Console                                   console                     [C#],F#,VB  Common/Console
Aplicativo do Windows Forms                             winforms                    [C#],VB     Common/WinForms
Aplicativo Web ASP.NET Core                             webapp,razor                [C#]        Web/MVC/Razor Pages
Aplicativo Web Blazor                                   blazor                      [C#]        Web/Blazor/WebAssembly
Aplicativo Web do ASP.NET Core (Model-View-Controller)  mvc                         [C#],F#     Web/MVC
Aplicativo WPF                                          wpf                         [C#],VB     Common/WPF
Arquivo de Buffer de Protocolo                          proto                                   Web/gRPC
Arquivo de Configuração da Web                          webconfig                               Config
arquivo de dotnet gitignore                             gitignore,.gitignore                    Config
Arquivo de manifesto da ferramenta local Dotnet         tool-manifest                           Config
Arquivo de Solução                                      sln,solution                            Solution
Arquivo EditorConfig                                    editorconfig,.editorconfig              Config
arquivo global.json                                     globaljson,global.json                  Config
Arquivo MSBuild Directory.Build.props                   buildprops                              MSBuild/props
Arquivo MSBuild Directory.Build.targets                 buildtargets                            MSBuild/props
Arquivo MSBuild Directory.Packages.props                packagesprops                           MSBuild/packages/props/CPM
ASP.NET Core API Web (native AOT)                       webapiaot                   [C#]        Web/Web API/API/Service
ASP.NET Core Vazio                                      web                         [C#],F#     Web/Empty
ASP.NET Core with Angular                               angular                     [C#]        Web/MVC/SPA
ASP.NET Core with React.js                              react                       [C#]        Web/MVC/SPA
Biblioteca de Classes                                   classlib                    [C#],F#,VB  Common/Library
Biblioteca de Classes do Windows Forms                  winformslib                 [C#],VB     Common/WinForms
Biblioteca de Classes Razor                             razorclasslib               [C#]        Web/Razor/Library
Biblioteca de Classes WPF                               wpflib                      [C#],VB     Common/WPF
Biblioteca de Controles de Usuário do WPF               wpfusercontrollib           [C#],VB     Common/WPF
Biblioteca de Controles do Windows Forms                winformscontrollib          [C#],VB     Common/WinForms
Biblioteca de Controles Personalizados do WPF           wpfcustomcontrollib         [C#],VB     Common/WPF
Blazor Server App                                       blazorserver                [C#]        Web/Blazor
Classe de teste MSTest                                  mstest-class                [C#],F#,VB  Test/MSTest
Componente Razor                                        razorcomponent              [C#]        Web/ASP.NET
Configuração do NuGet                                   nugetconfig,nuget.config                Config
Controlador de API                                      apicontroller               [C#]        Web/ASP.NET
Controlador MVC                                         mvccontroller               [C#]        Web/ASP.NET
Exibição do Razor                                       view                        [C#]        Web/ASP.NET
Item de Teste NUnit 3                                   nunit-test                  [C#],F#,VB  Test/NUnit
                                                                                    [C#],F#,VB  Test/NUnit
MVC ViewImports                                         viewimports                 [C#]        Web/ASP.NET
MVC ViewStart                                           viewstart                   [C#]        Web/ASP.NET
Projeto de Teste MSTest                                 mstest                      [C#],F#,VB  Test/MSTest/Desktop/Web
Projeto de Teste MSTest do Playwright                   mstest-playwright           [C#]        Test/MSTest/Playwright/Desktop/Web
Projeto de Teste NUnit 3                                nunit                       [C#],F#,VB  Test/NUnit
                                                                                    [C#],F#,VB  Test/NUnit/Desktop/Web
Projeto de Teste NUnit do Playwright                    nunit-playwright            [C#]        Test/NUnit/Playwright/Desktop/Web
Projeto de Teste xUnit                                  xunit                       [C#],F#,VB  Test/xUnit/Desktop/Web
Página Razor                                            page                        [C#]        Web/ASP.NET
Serviço de Trabalho                                     worker                      [C#],F#     Common/Worker/Web
Serviço gRPC do ASP.NET Core                            grpc                        [C#]        Web/gRPC/API/Service

PS D:\alura\dotnet-criando-api-aspnet-core> 
```
