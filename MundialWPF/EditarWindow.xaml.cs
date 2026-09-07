using System;
using System.Windows;
using MundialWPF; 

namespace MundialWPF
{
    public partial class EditarWindow : Window
    {
        public Jugador JugadorAEditar { get; private set; }

        public EditarWindow(Jugador jugador)
        {
            InitializeComponent();
            JugadorAEditar = jugador;
            lblNombre.Text = jugador.Nombre;
            txtGoles.Text = jugador.Goles.ToString();
            txtAsistencias.Text = jugador.Asistencias.ToString();
            txtTarjetas.Text = jugador.TarjetasRecibidas.ToString();
            txtMinutos.Text = jugador.MinutosJugados.ToString();
            txtPartidos.Text = jugador.PartidosJugados.ToString();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JugadorAEditar.Goles = int.Parse(txtGoles.Text);
                JugadorAEditar.Asistencias = int.Parse(txtAsistencias.Text);
                JugadorAEditar.TarjetasRecibidas = int.Parse(txtTarjetas.Text);
                JugadorAEditar.MinutosJugados = int.Parse(txtMinutos.Text);
                JugadorAEditar.PartidosJugados = int.Parse(txtPartidos.Text);
                this.DialogResult = true; 
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor, ingresa solo números enteros válidos.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}