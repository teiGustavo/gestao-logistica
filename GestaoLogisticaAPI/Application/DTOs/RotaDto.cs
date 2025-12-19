namespace GestaoLogisticaAPI.Application.DTOs;

public class RotaDto
{
    public int CodRota { get; set; }
    public int Origem { get; set; }
    public int Destino { get; set; }
    public decimal? DistanciaKm { get; set; }
    public DateTime? CriadoEm { get; set; }
}