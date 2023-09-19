using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "Titulo obrigatorio")] 
    public string Titulo {  get; set; }
    [Required(ErrorMessage = "Genero obrigatorio")]
    [MaxLength(50, ErrorMessage = "Tamanho do negnero nao pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "Duracao obrigatoria")]
    [Range(70, 600, ErrorMessage = "Duracao invalida")]
    public int Duracao { get; set; }
  
}
