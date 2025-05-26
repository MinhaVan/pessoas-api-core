using Pessoas.Core.Domain.Enums;

namespace Pessoas.Core.Domain.ViewModels;

public class EnderecoViewModel
{
    public int Id { get; set; }
    public string Complemento { get; set; }
    public string Numero { get; set; }
    public string CEP { get; set; }
    public string Rua { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Pais { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public TipoEnderecoEnum TipoEndereco { get; set; }
}