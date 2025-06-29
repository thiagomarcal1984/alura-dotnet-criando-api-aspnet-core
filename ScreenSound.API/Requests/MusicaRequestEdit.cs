namespace ScreenSound.API.Requests;

public record MusicaRequestEdit(
    int id,
    string nome, 
    int ArtistaId, 
    int anoLancamento
): MusicaRequest(nome, ArtistaId, anoLancamento);
