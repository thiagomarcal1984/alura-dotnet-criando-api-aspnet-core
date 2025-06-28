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
## Criação do projeto Shared.Modelos
Vamos criar um projeto de Biblioteca de Classes (nome de template `classlib`) para concentrar a comunicação com o banco de dados.

1) Criação do diretório do projeto:
```
PS D:\alura\dotnet-criando-api-aspnet-core> mkdir ScreenSound.Shared.Modelos


    Diretório: D:\alura\dotnet-criando-api-aspnet-core


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        17/06/2025     21:00                ScreenSound.Shared.Modelos
```

2) Criação do projeto do tipo `classlib` (Biblioteca de Classes):
```
PS D:\alura\dotnet-criando-api-aspnet-core> cd .\ScreenSound.Shared.Modelos\
PS D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.Shared.Modelos> dotnet new classlib
O modelo "Biblioteca de Classes" foi criado com êxito.

Processando ações pós-criação...
Restaurando D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.Shared.Modelos\ScreenSound.Shared.Modelos.csproj:
A restauração foi bem-sucedida.

```

3) Acrescentando o projeto à solução:
```
PS D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.Shared.Modelos> cd ..
PS D:\alura\dotnet-criando-api-aspnet-core> dotnet solution add .\ScreenSound.Shared.Modelos\
O projeto ‘ScreenSound.Shared.Modelos\ScreenSound.Shared.Modelos.csproj’ foi adicionado à solução.
PS D:\alura\dotnet-criando-api-aspnet-core> 
```

A ideia agora é mover o diretório `Modelos` do projeto `ScreenSound` para `ScreenSound.Shared.Modelos`. Naturalmente o código vai quebrar porque será necessário atualizar as dependências entre os dois projetos. Para isso, clique com o botão direito no nó `Dependências` no projeto `ScreeSound.csproj` e escolha a opção `Adicionar Referência de Projeto...`.

Note que, ao usar esse comando, o arquivo `ScreendSound.csproj` passará a conter as seguintes linhas:
```XML
<Project Sdk="Microsoft.NET.Sdk">

  <!-- Resto do código -->

  <ItemGroup>
    <ProjectReference Include="..\ScreenSound.Shared.Modelos\ScreenSound.Shared.Modelos.csproj" />
  </ItemGroup>

</Project>
```

Após confirmar as alterações, você pode fazer o build (ou executar o projeto):
```
PS D:\alura\dotnet-criando-api-aspnet-core> dotnet build --no-incremental
Restauração concluída (1,0s)
  ScreenSound.Shared.Modelos êxito (0,4s) → ScreenSound.Shared.Modelos\bin\Debug\net9.0\ScreenSound.Shared.Modelos.dll
  ScreenSound.API êxito (0,7s) → ScreenSound.API\bin\Debug\net9.0\ScreenSound.API.dll
  ScreenSound êxito (3,2s) → ScreenSound\bin\Debug\net9.0\ScreenSound.dll

Construir êxito em 10,4s
PS D:\alura\dotnet-criando-api-aspnet-core>
```
## Criação do projeto Shared.Data
Vamos mover os diretórios `Banco` e `Migrations` que estão no projeto `ScreenSound` para um novo projeto de Biblioteca de Classes que se chamará `ScreenSound.Shared.Dados`. Para isso, vamos repetir os passos da linha de comando:

1. Criar o diretório `ScreenSound.Shared.Dados`;
```
PS D:\alura\dotnet-criando-api-aspnet-core> mkdir ScreenSound.Shared.Dados


    Diretório: D:\alura\dotnet-criando-api-aspnet-core


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----        21/06/2025     11:02                ScreenSound.Shared.Dados

```

2. Criar um projeto de Biblioteca de Classes (`classlib`) nesse diretório;
```
PS D:\alura\dotnet-criando-api-aspnet-core> cd .\ScreenSound.Shared.Dados\   
PS D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.Shared.Dados> dotnet new classlib
O modelo "Biblioteca de Classes" foi criado com êxito.

Processando ações pós-criação...
Restaurando D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.Shared.Dados\ScreenSound.Shared.Dados.csproj:
A restauração foi bem-sucedida.
```
3. Adicionar esse projeto à solução (`ScreenSound.sln`);
```
PS D:\alura\dotnet-criando-api-aspnet-core\ScreenSound.Shared.Dados> cd ..
PS D:\alura\dotnet-criando-api-aspnet-core> dotnet solution add .\ScreenSound.Shared.Dados\   
O projeto ‘ScreenSound.Shared.Dados\ScreenSound.Shared.Dados.csproj’ foi adicionado à solução.
PS D:\alura\dotnet-criando-api-aspnet-core> 
```

4. Moveremos o conteúdo dos diretórios `Banco` e `Migrations` para `ScreenSound.Shared.Dados`;
```
PS D:\alura\dotnet-criando-api-aspnet-core> move .\ScreenSound\Banco\ .\ScreenSound.Shared.Dados\   
PS D:\alura\dotnet-criando-api-aspnet-core> move .\ScreenSound\Migrations\ .\ScreenSound.Shared.Dados\
```

5. Incluiremos no novo projeto `ScreenSound.Shared.Dados` a dependência para o projeto `ScreenSound.Shared.Modelos.csproj`. Além da dependência de projeto, será necessário também referenciar as dependências do NuGet neste arquivo, movendo-os do `ScreenSound.proj` para `ScreenSound.Shared.Dados.csproj`.
```XML
<!-- Arquivo ScreenSound.Shared.Dados.csproj -->
<Project Sdk="Microsoft.NET.Sdk">

  <!-- Resto do código -->

  <!-- Este ItemGroup foi movido do projeto ScreenSound.csproj -->
  <ItemGroup>
    <PackageReference Include="Azure.Identity" Version="1.11.4" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="7.0.14" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.14" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="7.0.14">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.IdentityModel.JsonWebTokens" Version="8.0.1" />
    <PackageReference Include="System.Formats.Asn1" Version="6.0.1" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.0.1" />
    <PackageReference Include="System.Text.Json" Version="8.0.5" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ScreenSound.Shared.Modelos\ScreenSound.Shared.Modelos.csproj" />
  </ItemGroup>

</Project>
```

As classes `DAL` e `ScreenSoundContext` do projeto `ScreendSound.Shared.Dados` estão anotadas como `internal`. Para que elas possam ser usadas pelos outros projetos, precisamos declará-las como públicas:
```Csharp
// ScreendSound.Shared.Dados/Banco/DAL.cs
// Resto do código
namespace ScreenSound.Banco;
// Antes era: internal class DAL<T> where T : class
public class DAL<T> where T : class
{
  // Resto do código
}
```

```Csharp
// ScreendSound.Shared.Dados/Banco/ScreendSoundContext.cs
// Resto do código
namespace ScreenSound.Banco;
// Antes era: internal class ScreenSoundContext: DbContext
public class ScreenSoundContext: DbContext
{
  // Resto do código
}
```

Finalmente, o arquivo `ScreendSound.csproj` precisa acrescentar a dependência do novo projeto `ScreenSound.Shared.Dados`:
```XML
<Project Sdk="Microsoft.NET.Sdk">

  <!-- Resto do código -->

  <ItemGroup>
    <ProjectReference Include="..\ScreenSound.Shared.Modelos\ScreenSound.Shared.Modelos.csproj" />
    <ProjectReference Include="..\ScreenSound.Shared.Dados\ScreenSound.Shared.Dados.csproj" />
  </ItemGroup>

</Project>
```
## Retornando artistas pela API
Vamos fazer a API se comunicar com o banco. Para isso, primeiramente vamos incluir no projeto `ScreenSound.API` as dependências dos projetos `ScreenSound.Shared.Modelos` e `ScreenSound.Shared.Dados`:
```XML
<!-- Arquivo ScreenSound.Api.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Web">

  <!-- Resto do código -->

  <ItemGroup>
    <ProjectReference Include="..\ScreenSound.Shared.Modelos\ScreenSound.Shared.Modelos.csproj" />
    <ProjectReference Include="..\ScreenSound.Shared.Dados\ScreenSound.Shared.Dados.csproj" />
  </ItemGroup>

</Project>
```
Vamos mudar o arquivo `Program.cs` do projeto `ScreendSound.API` para exibir a lista de artistas no caminho raiz:
```CSharp
// ScreendSound.API\Program.cs
using ScreenSound.Banco;
using ScreenSound.Modelos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    return dal.Listar();
});

app.Run();
```
Mas se compilarmos o projeto da API (`dotnet run --project .\ScreenSound.API\`) e acessarmos a aplicação no localhost, vai aparecer um erro de recursividade (Artista referencia Música, que referencia Artista e assim sucessivamente).

Para corrigir isso, vamos acrescentar um serviço para evitar essas buscas cíclicas. Eis o código de `Program.cs`:
```CSharp
// ScreendSound.API\Program.cs
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization; // Novo código

var builder = WebApplication.CreateBuilder(args);

// Início do novo código
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
// Fim do novo código

var app = builder.Build();

app.MapGet("/", () =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    return dal.Listar();
});

app.Run();
```
Agora a compilação funciona sem problemas.

# Montando uma API mínima
## Entendendo rotas e códigos de resposta
O arquivo `Program.cs` do projeto `ScreendSound.API` vai ser modificado: vamos criar duas rotas para retorno dos artistas:
1. A primeira (que era a rota raiz) passará a ser `/Artistas` e exibirá todos os artistas; e
2. A segunda será `/Artistas/{nome do artista}` e exibirá algum artista específico.


Vamos ao código de definição das rotas:
```Csharp
// ScreendSound.API\Program.cs

// Resto do código
app.MapGet("/Artistas", () =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(dal.Listar());
});

app.MapGet("/Artistas/{nome}", (string nome) =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (artista is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artista);
});

app.Run();
```

> Observações:
> 1. Uniformize os tipos de retorno em uma rota (`app.MapGet("{rota}")`). Por exemplo: se eu usar um condicional que retorna string em um ramo e retorna um objeto de modelo em outro, ocorre erro de compilação;
> 2. Um meio de fazer essa uniformização é sempre usar os métodos da classe `Results` para qualquer retorno da API (`Results.NotFound()`, `Results.Ok(objetoDeModelo)`, etc.).

A segunda rota criada pode dar erro se não houver um construtor sem parâmetros na classe Artista. O erro exibido é este:
> NotSupportedException: The deserialization constructor for type 'Castle.Proxies.ArtistaProxy' contains parameters with null names. This might happen because the parameter names have been trimmed by ILLink. Consider using the source generated serializer instead.
Acrescente o construtor `public Artista(){}` na classe `ScreenSound.Modelos.Artista` para não mostrar esse erro.

## Adicionando o Artista
Nova rota para acréscimo de artista:
```Csharp
// ScreendSound.API\Program.cs
using Microsoft.AspNetCore.Mvc;

// Resto do código
app.MapPost("/Artistas/", ([FromBody]Artista artista) =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

// Resto do código
```

Vamos mudar o construtor vazio de `Artista` para gerar a foto de perfil e assim não tornar esse campo obrigatório para preenchimento:
```Csharp
// ScreenSound.Shared.Modelos\Artista.cs
// Resto do código
public class Artista {
  // Resto do código
      public Artista()
      {
          FotoPerfil = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png";
      }
  // Resto do código
}
```

Payload em formato JSON:
```JSON
{
  "Nome" : "White Stripes",
  "Bio" : "Bio dos White Stripes"
}
```

1. A classe `FromBody` vem da biblioteca `Microsoft.AspNetCore.Mvc` e serve para inferir um objeto de modelo a partir da requisição POST!
2. O retorno desta rota estará vazio mesmo.
3. Você pode testar esta rota da API com o ThunderClient do Visual Studio (ou Postman, ou cUrl, a ferramenta que você quiser).

## Injeção de dependência
O Asp.Net permite injeção de dependência no builder da aplicação web. Vamos usar os métodos `AddDbContext` e `AddTransient` do objeto `builder.Services` para definir o que será injetado. Depois, injetamos as dependências em cada rota com o atributo `[FromServices]` da biblioteca `Microsoft.AspNetCore.Mvc`.

Veja o código a seguir:
```Csharp
// ScreendSound.API\Program.cs
// Resto do código
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();

var app = builder.Build();

app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
{
    // Código excluído: var dal = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(dal.Listar());
});

app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
{
    // Código excluído: var dal = new DAL<Artista>(new ScreenSoundContext());
    var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (artista is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artista);
});

app.MapPost("/Artistas/", ([FromServices] DAL<Artista> dal, [FromBody]Artista artista) =>
{
    // Código excluído: var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

app.Run();
```
## Removendo Artistas
```CSharp
// ScreendSound.API\Program.cs

// Resto do código
app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
{
    var artista = dal.RecuperarPor(a => a.Id == id);
    if (artista is null)
    {
        return Results.NotFound();
    }
    dal.Deletar(artista);

    return Results.NoContent();
});
// Resto do código
```
> A novidade é o tipo de retorno quando a exclusão da entidade é efetivada: é o `Results.NoContent` (código HTTP 204). Fora isso, nada novo: injeção da dependência do DAL e o uso dos seus métodos para interagir com o banco de dados.

## Atualizando o Artista
Atualizar uma entidade é mais complexo porque a rota exige o payload (atributo `[FromBody] Entidade entidade`) e o identificador da entidade que será modificada.

> Veja que é necessário mapear cada propriedade da entidade com o payload. Flask é MUITO melhor pra trabalhar com a atualização de entidades...

```CSharp
// ScreendSound.API\Program.cs

// Resto do código
app.MapPut(
    "/Artistas/{id}", 
    (
        [FromServices] DAL<Artista> dal,
        [FromBody] Artista artista,
        int id
    ) => {
        var artistaAAatualizar = dal.RecuperarPor(a => a.Id == id);
        if (artistaAAatualizar is null)
        {
            return Results.NotFound();
        }

        artistaAAatualizar.Nome = artista.Nome;
        artistaAAatualizar.Bio = artista.Bio;
        artistaAAatualizar.FotoPerfil = artista.FotoPerfil;

        dal.Atualizar(artistaAAatualizar);
        return Results.Ok();
    }
);
// Resto do código
```

## Mão na massa: criando os endpoints de música
```Csharp
// ScreendSound.API\Program.cs

// Resto do código
builder.Services.AddTransient<DAL<Musica>>(); 
// Foi necessário acrescentar outro DAL para música.

// Resto do código
app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
{
    return Results.Ok(dal.Listar());
});

app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
{
    var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (musica is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(musica);
});

app.MapPost("/Musicas/", ([FromServices] DAL<Musica> dal, [FromBody]Musica musica) =>
{
    dal.Adicionar(musica);
    return Results.Ok();
});

app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) =>
{
    var musica = dal.RecuperarPor(a => a.Id == id);
    if (musica is null)
    {
        return Results.NotFound();
    }
    dal.Deletar(musica);

    return Results.NoContent();
});

app.MapPut(
    "/Musicas/{id}", 
    (
        [FromServices] DAL<Musica> dal,
        [FromBody] Musica musica,
        int id
    ) => {
        var musicaAAatualizar = dal.RecuperarPor(a => a.Id == id);
        if (musicaAAatualizar is null)
        {
            return Results.NotFound();
        }

        musicaAAatualizar.Nome = musica.Nome;
        musicaAAatualizar.AnoLancamento = musica.AnoLancamento;
        musicaAAatualizar.Artista = musica.Artista;

        dal.Atualizar(musicaAAatualizar);
        return Results.Ok();
    }
);
// Resto do código
```
> Problema: de alguma maneira, o artista não é associado à música quando ela é inserida no banco de dados.
