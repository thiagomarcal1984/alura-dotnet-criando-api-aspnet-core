using Microsoft.AspNetCore.Mvc;
using ScreenSound.Modelos;
using ScreenSound.Banco;

namespace ScreenSound.API.Endpoints;

public static class ArtistasExtensions
{
    public static void AddEndpointsArtistas(this WebApplication app)
    {
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
        {
            return Results.Ok(dal.Listar());
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(artista);
        });

        app.MapPost("/Artistas/", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) =>
        {
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
            ) =>
            {
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
    }
}
