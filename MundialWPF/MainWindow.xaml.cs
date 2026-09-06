using System;
using System.Collections.Generic;
using System.Windows;

namespace Proyecto1_ED2
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCargarArchivo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí integraremos la lectura del archivo CSV.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí abriremos una ventana o formulario para insertar un jugador manualmente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí implementaremos la búsqueda en el Árbol B+.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnTopGoles_Click(object sender, RoutedEventArgs e)
        {
            txtTituloSeccion.Text = "Top 5 Goleadores (Max Heap)";
        }

        private void btnTopAsistencias_Click(object sender, RoutedEventArgs e)
        {
            txtTituloSeccion.Text = "Top 5 Asistencias (Max Heap)";
        }

        private void btnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            txtTituloSeccion.Text = "Tabla General de Jugadores";
        }
    }
}