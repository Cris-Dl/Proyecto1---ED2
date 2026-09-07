using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using MundialWPF; 

namespace Proyecto1_ED2
{
    public partial class MainWindow : Window
    {
        private ArbolBPlus arbolJugadores;
        private MaxHeap topGoleadores;
        private MaxHeap topAsistencias;
        private MinHeap topMenosTarjetas;

        public MainWindow()
        {
            InitializeComponent();
            arbolJugadores = new ArbolBPlus(4);
            topGoleadores = new MaxHeap("Goles", 100);
            topAsistencias = new MaxHeap("Asistencias", 100);
            topMenosTarjetas = new MinHeap("Tarjetas", 100);
        }

        private void btnCargarArchivo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos CSV (*.csv)|*.csv|Archivos de texto (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string[] lineas = File.ReadAllLines(openFileDialog.FileName);
                    int inicio = lineas[0].Contains("Nombre", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
                    for (int i = inicio; i < lineas.Length; i++)
                    {
                        string[] datos = lineas[i].Split(',');
                        if (datos.Length >= 8)
                        {
                            Jugador nuevoJugador = new Jugador(
                                datos[0].Trim(),
                                datos[1].Trim(),
                                datos[2].Trim(),
                                int.Parse(datos[3].Trim()),
                                int.Parse(datos[4].Trim()),
                                int.Parse(datos[5].Trim()),
                                int.Parse(datos[6].Trim()),
                                int.Parse(datos[7].Trim())
                            );
                            arbolJugadores.Insertar(nuevoJugador);
                            topGoleadores.Insertar(nuevoJugador);
                            topAsistencias.Insertar(nuevoJugador);
                            topMenosTarjetas.Insertar(nuevoJugador);
                        }
                    }
                    MessageBox.Show("¡Datos cargados y organizados correctamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgJugadores.ItemsSource = arbolJugadores.ObtenerTodos();
                    ResaltarColumna("");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo.\nDetalle: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            RegistroWindow ventanaRegistro = new RegistroWindow();
            ventanaRegistro.Owner = this;
            if (ventanaRegistro.ShowDialog() == true)
            {
                Jugador nuevoJugador = ventanaRegistro.JugadorCreado;
                arbolJugadores.Insertar(nuevoJugador);
                topGoleadores.Insertar(nuevoJugador);
                topAsistencias.Insertar(nuevoJugador);
                topMenosTarjetas.Insertar(nuevoJugador);
                dgJugadores.ItemsSource = arbolJugadores.ObtenerTodos();
                ResaltarColumna(""); 
                MessageBox.Show($"El jugador {nuevoJugador.Nombre} ha sido registrado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string nombreBuscado = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(nombreBuscado))
            {
                MessageBox.Show("Por favor, ingresa el nombre del jugador que deseas buscar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Jugador encontrado = arbolJugadores.Buscar(nombreBuscado);
            if (encontrado != null)
            {
                dgJugadores.ItemsSource = new Jugador[] { encontrado };
                ResaltarColumna(""); 
                MessageBox.Show($"¡Jugador encontrado!\n\nSelección: {encontrado.Seleccion}\nGoles: {encontrado.Goles}\nAsistencias: {encontrado.Asistencias}",
                                "Búsqueda Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                txtBuscar.Clear();
            }
            else
            {
                MessageBox.Show($"No se encontró ningún jugador con el nombre '{nombreBuscado}'.",
                                "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnTopGoles_Click(object sender, RoutedEventArgs e)
        {
            Jugador[] top5 = topGoleadores.ObtenerTop(5);
            if (top5 != null && top5.Length > 0)
            {
                dgJugadores.ItemsSource = top5;
                ResaltarColumna("Goles"); 
                MessageBox.Show("Mostrando el Top 5 de Máximos Goleadores (Max Heap).", "Reporte Generado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("El sistema está vacío.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnTopAsistencias_Click(object sender, RoutedEventArgs e)
        {
            Jugador[] top5 = topAsistencias.ObtenerTop(5);
            if (top5 != null && top5.Length > 0)
            {
                dgJugadores.ItemsSource = top5;
                ResaltarColumna("Asistencias"); 
                MessageBox.Show("Mostrando el Top 5 de Jugadores con más Asistencias (Max Heap).", "Reporte Generado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("El sistema está vacío.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnMenosTarjetas_Click(object sender, RoutedEventArgs e)
        {
            Jugador[] top5 = topMenosTarjetas.ObtenerTop(5);
            if (top5 != null && top5.Length > 0)
            {
                dgJugadores.ItemsSource = top5;
                ResaltarColumna("TarjetasRecibidas");
                MessageBox.Show("Mostrando el Top 5 de jugadores con menos tarjetas recibidas (Min Heap).", "Reporte Generado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("El sistema está vacío.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            Jugador[] todos = arbolJugadores.ObtenerTodos();
            if (todos != null && todos.Length > 0)
            {
                dgJugadores.ItemsSource = todos;
                ResaltarColumna(""); 
                MessageBox.Show($"Se encontraron {todos.Length} jugadores en el Árbol B+.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("El sistema está vacío.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResaltarColumna(string nombreColumna)
        {
            dgJugadores.UpdateLayout();
            Style estiloResaltado = new Style(typeof(System.Windows.Controls.DataGridCell));
            estiloResaltado.Setters.Add(new Setter(System.Windows.Controls.Control.BackgroundProperty, System.Windows.Media.Brushes.LightGoldenrodYellow));
            estiloResaltado.Setters.Add(new Setter(System.Windows.Controls.Control.FontWeightProperty, FontWeights.Bold));
            estiloResaltado.Setters.Add(new Setter(System.Windows.Controls.Control.ForegroundProperty, System.Windows.Media.Brushes.DarkRed));
            foreach (var columna in dgJugadores.Columns)
            {
                if (columna.Header != null && columna.Header.ToString() == nombreColumna)
                {
                    columna.CellStyle = estiloResaltado; 
                }
                else
                {
                    columna.CellStyle = null;
                }
            }
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            string nombreBuscado = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(nombreBuscado))
            {
                MessageBox.Show("Escribe el nombre del jugador a eliminar en la caja de búsqueda.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Jugador jugador = arbolJugadores.Buscar(nombreBuscado);
            if (jugador != null)
            {
                arbolJugadores.Eliminar(nombreBuscado);
                topGoleadores.Eliminar(nombreBuscado);
                topAsistencias.Eliminar(nombreBuscado);
                topMenosTarjetas.Eliminar(nombreBuscado);
                dgJugadores.ItemsSource = arbolJugadores.ObtenerTodos();
                txtBuscar.Clear();
                ResaltarColumna("");
                MessageBox.Show($"El jugador {nombreBuscado} ha sido eliminado del sistema.", "Eliminación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No se encontró al jugador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}