using System.Collections.Generic;

namespace Aluno.Core.Domain.Models;

public class Empresa : Entity
{
    public string CNPJ { get; set; }
    public string NomeExibicao { get; set; }
    public string Descricao { get; set; }
    public string NomeFantasia { get; set; }
    public string RazaoSocial { get; set; }
    public string Apelido { get; set; }
}