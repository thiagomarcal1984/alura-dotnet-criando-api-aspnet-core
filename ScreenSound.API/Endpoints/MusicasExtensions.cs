using Microsoft.AspNetCore.Mvc;
using ScreenSound.Modelos;
using ScreenSound.Banco;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicaResponse EntityToResponse(Musica musica)
    {
        return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
    }
    
    public static void AddEndpointsMusicas(this WebApplication app)
    {
        app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
        {
            return Results.Ok(EntityListToResponseList(dal.Listar()));
        });

        app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (musica is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(musica));
        });

        app.MapPost("/Musicas/", (
            [FromServices] DAL<Musica> dal,
            [FromServices] DAL<Artista> dalArtistas,
            [FromBody]MusicaRequest musicaRequest) =>
        {
            var musica = new Musica(musicaRequest.nome);
            musica.AnoLancamento = musicaRequest.anoLancamento;
            musica.Artista = dalArtistas.RecuperarPor(a => a.Id.Equals(musicaRequest.ArtistaId));
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
                [FromServices] DAL<Artista> dalArtista,
                [FromBody] MusicaRequestEdit musicaRequestEdit,
                int id
            ) => {
                var musicaAAatualizar = dal.RecuperarPor(a => a.Id == id);
                if (musicaAAatualizar is null)
                {
                    return Results.NotFound();
                }

                var artista = dalArtista.RecuperarPor(a => a.Id == musicaRequestEdit.ArtistaId);
                musicaAAatualizar.Nome = musicaRequestEdit.nome;
                musicaAAatualizar.AnoLancamento = musicaRequestEdit.anoLancamento;
                musicaAAatualizar.Artista = artista;

                dal.Atualizar(musicaAAatualizar);
                return Results.Ok();
            }
        );

    }
}
