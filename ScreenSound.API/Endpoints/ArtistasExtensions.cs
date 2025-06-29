using Microsoft.AspNetCore.Mvc;
using ScreenSound.Modelos;
using ScreenSound.Banco;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;

namespace ScreenSound.API.Endpoints;

public static class ArtistasExtensions
{
    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)
    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }

    public static void AddEndpointsArtistas(this WebApplication app)
    {
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
        {
            return Results.Ok(
                EntityListToResponseList(dal.Listar())
            );
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(EntityToResponse(artista));
        });

        app.MapPost("/Artistas/", (
            [FromServices] DAL<Artista> dal,
            [FromBody] ArtistaRequest artistaRequest) =>
        {
            var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
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
                [FromBody] ArtistaRequestEdit artstaRequestEdit,
                int id
            ) =>
            {
                var artistaAAatualizar = dal.RecuperarPor(a => a.Id == id);
                if (artistaAAatualizar is null)
                {
                    return Results.NotFound();
                }

                artistaAAatualizar.Nome = artstaRequestEdit.nome;
                artistaAAatualizar.Bio = artstaRequestEdit.bio;

                dal.Atualizar(artistaAAatualizar);
                return Results.Ok();
            }
        );
    }
}
