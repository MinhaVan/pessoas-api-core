using System.Collections.Generic;

namespace Aluno.Core.Domain.Models;

public class Aluno : Entity
{
    public string PrimeiroNome { get; set; }
    public string CPF { get; set; }
    public string UltimoNome { get; set; }
    public string Contato { get; set; }
    public string Email { get; set; }
    public int ResponsavelId { get; set; }
    public int EmpresaId { get; set; }
    public int EnderecoPartidaId { get; set; }
    public int EnderecoDestinoId { get; set; }
    public int? EnderecoRetornoId { get; set; }

    public string NomeInteiro() => this.PrimeiroNome.Trim() + " " + this.UltimoNome.Trim();
}