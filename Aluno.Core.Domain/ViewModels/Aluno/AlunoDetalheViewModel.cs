using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aluno.Core.Domain.Models;

namespace Aluno.Core.Domain.ViewModels.Aluno;

public class AlunoDetalheViewModel
{
    public int Id { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }
    public string Contato { get; set; }
    public string Email { get; set; }
    public int ResponsavelId { get; set; }
    public int EmpresaId { get; set; }
    public int EnderecoPartidaId { get; set; }
    public int EnderecoRetornoId { get; set; }
    public int EnderecoDestinoId { get; set; }

    public EnderecoViewModel EnderecoPartida { get; set; }
    public EnderecoViewModel EnderecoDestino { get; set; }
    public EnderecoViewModel EnderecoRetorno { get; set; }
    public UsuarioDetalheViewModel Responsavel { get; set; }
}