using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

var app = builder.Build();

app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
{
    // var dal = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(dal.Listar());
});

app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
{
    // var dal = new DAL<Artista>(new ScreenSoundContext());
    var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (artista is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artista);
});

app.MapPost("/Artistas/", ([FromServices] DAL<Artista> dal, [FromBody]Artista artista) =>
{
    // var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

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

app.Run();
