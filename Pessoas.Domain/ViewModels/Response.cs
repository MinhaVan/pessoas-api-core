using System.Collections.Generic;

namespace Aluno.Core.Domain.ViewModels;

public class BaseResponse<T>
{
    public bool Sucesso { get; set; }
    public T Data { get; set; }
    public string Mensagem { get; set; }
    public List<string> Erros { get; set; } = new List<string>();
}