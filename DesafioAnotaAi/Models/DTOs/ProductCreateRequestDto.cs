namespace DesafioAnotaAi.Models.DTOs;

public record ProductCreateRequestDto(string Title, string Description, double Price, string IdCategory, string IdOwner);
