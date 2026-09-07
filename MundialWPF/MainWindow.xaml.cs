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

        public MainWindow()
        {
            InitializeComponent();
            arbolJugadores = new ArbolBPlus(4);
            topGoleadores = new MaxHeap("Goles", 100);
            topAsistencias = new MaxHeap("Asistencias", 100);
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
                    int inicio = lineas[0].Contains("Nombre") || lineas[0].Contains("nombre") ? 1 : 0;
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
                        }
                    }
                    MessageBox.Show("¡Datos cargados y organizados correctamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgJugadores.ItemsSource = arbolJugadores.ObtenerTodos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo. Verifica que los números no tengan letras.\nDetalle: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            Jugador[] todos = arbolJugadores.ObtenerTodos();
            if (todos != null && todos.Length > 0)
            {
                dgJugadores.ItemsSource = todos;
                MessageBox.Show($"Se encontraron {todos.Length} jugadores en el Árbol B+.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("El sistema está vacío. Carga un archivo CSV primero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e) { }
        private void btnBuscar_Click(object sender, RoutedEventArgs e) { }
        private void btnTopGoles_Click(object sender, RoutedEventArgs e) { }
        private void btnTopAsistencias_Click(object sender, RoutedEventArgs e) { }
    }
}