using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Aluno.Core.Domain.Enums;

namespace Aluno.Core.Domain.Models;

public class Endereco : Entity
{
    public string Pais { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string CEP { get; set; }
    public string Complemento { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public TipoEnderecoEnum TipoEndereco { get; set; }
    public bool EnderecoPrincipal { get; set; }
    public int? UsuarioId { get; set; }
    public int? EmpresaId { get; set; }

    // Relacionamentos
    public virtual List<AjusteAlunoRota> AjusteAlunoRotasPartida { get; set; } = new List<AjusteAlunoRota>();
    public virtual List<AjusteAlunoRota> AjusteAlunoRotasRetorno { get; set; } = new List<AjusteAlunoRota>();
    public virtual List<AjusteAlunoRota> AjusteAlunoRotasDestino { get; set; } = new List<AjusteAlunoRota>();
    // public virtual List<Rota> Rotas { get; set; } = new List<Rota>();
    public virtual List<Aluno> EnderecosPartidas { get; set; } = new List<Aluno>();
    public virtual List<Aluno> EnderecosRetornos { get; set; } = new List<Aluno>();
    public virtual List<Aluno> EnderecosDestinos { get; set; } = new List<Aluno>();
    public virtual Usuario Usuario { get; set; }
    public virtual Empresa Empresa { get; set; }
}