using System;
using System.Windows;

namespace MundialWPF
{
    public partial class RegistroWindow : Window
    {
        public Jugador JugadorCreado { get; private set; }

        public RegistroWindow()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string seleccion = txtSeleccion.Text.Trim();
                string posicion = txtPosicion.Text.Trim();
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(seleccion))
                {
                    MessageBox.Show("El nombre y la selección son obligatorios.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int minutos = int.Parse(txtMinutos.Text.Trim());
                int goles = int.Parse(txtGoles.Text.Trim());
                int asistencias = int.Parse(txtAsistencias.Text.Trim());
                int tarjetas = int.Parse(txtTarjetas.Text.Trim());
                int partidos = int.Parse(txtPartidos.Text.Trim());
                JugadorCreado = new Jugador(nombre, seleccion, posicion, minutos, goles, asistencias, tarjetas, partidos);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor, asegúrate de que las estadísticas sean números enteros.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}