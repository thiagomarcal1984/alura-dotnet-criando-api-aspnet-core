using Microsoft.AspNetCore.Mvc;
using ScreenSound.Modelos;
using ScreenSound.Banco;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndpointsMusicas(this WebApplication app)
    {
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

    }
}
