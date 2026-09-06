using System; //Programa en general
public class Jugador
{
    public string Nombre { get; set; }
    public string Seleccion { get; set; }
    public string Posicion { get; set; }
    public int MinutosJugados { get; set; }
    public int Goles { get; set; }
    public int Asistencias { get; set; }
    public int TarjetasRecibidas { get; set; }
    public int PartidosJugados { get; set; }
    public Jugador() { }
    public Jugador(string nombre, string seleccion, string posicion, int minutosJugados, int goles, int asistencias, int tarjetasRecibidas, int partidosJugados)
    {
        Nombre = nombre;
        Seleccion = seleccion;
        Posicion = posicion;
        MinutosJugados = minutosJugados;
        Goles = goles;
        Asistencias = asistencias;
        TarjetasRecibidas = tarjetasRecibidas;
        PartidosJugados = partidosJugados;
    }
}

