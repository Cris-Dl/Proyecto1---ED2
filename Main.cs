using System; //Programa en general
using System.Collections.Generic; //Se llama para la interfaz grafica
using System.Windows; //Se llama para la interfaz grafica


public class NodoBPlus
{
    public bool Hoja { get; set; }
    public int[] Claves { get; set; }
    public NodoBPlus[] Hijos { get; set; }
    public int CantidadClaves { get; set; }
    public int CantidadHijos { get; set; }
    public NodoBPlus Siguiente { get; set; }
    public NodoBPlus(int orden, bool hoja = true)
    {
        Hoja = hoja;
        Claves = new int[orden];
        Hijos = new NodoBPlus[orden + 1];
        CantidadClaves = 0;
        CantidadHijos = 0;
        Siguiente = null;
    }
}

public class ArbolBPlus
{
    private int orden;
    private int maxClaves;
    private NodoBPlus raiz;
    public ArbolBPlus(int orden = 4)
    {
        this.orden = orden;
        this.maxClaves = orden - 1;
        this.raiz = new NodoBPlus(orden, hoja: true);
    }

    public void Insertar(int clave)
    {
        if (Buscar(clave))
        {
            Console.WriteLine($"La clave {clave} ya existe.");
            return;
        }
        var resultado = _Insertar(raiz, clave);
        if (resultado.HasValue)
        {
            var (claveGuia, nodoDerecho) = resultado.Value;
            NodoBPlus nuevaRaiz = new NodoBPlus(orden, hoja: false);
            nuevaRaiz.Claves[0] = claveGuia;
            nuevaRaiz.CantidadClaves = 1;
            nuevaRaiz.Hijos[0] = raiz;
            nuevaRaiz.Hijos[1] = nodoDerecho;
            nuevaRaiz.CantidadHijos = 2;
            raiz = nuevaRaiz;
        }
    }

    private (int, NodoBPlus)? _Insertar(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            int i = nodo.CantidadClaves - 1;
            while (i >= 0 && nodo.Claves[i] > clave)
            {
                nodo.Claves[i + 1] = nodo.Claves[i];
                i--;
            }
            nodo.Claves[i + 1] = clave;
            nodo.CantidadClaves++;
            if (nodo.CantidadClaves <= maxClaves)
                return null;
            return _DividirHoja(nodo);
        }
        int posicion = 0;
        while (posicion < nodo.CantidadClaves && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }
        var resultado = _Insertar(nodo.Hijos[posicion], clave);
        if (!resultado.HasValue)
            return null;
        var (claveGuia, nodoDerecho) = resultado.Value;
        int j = nodo.CantidadClaves - 1;
        while (j >= posicion)
        {
            nodo.Claves[j + 1] = nodo.Claves[j];
            nodo.Hijos[j + 2] = nodo.Hijos[j + 1]; 
            j--;
        }
        nodo.Claves[posicion] = claveGuia;
        nodo.Hijos[posicion + 1] = nodoDerecho;
        nodo.CantidadClaves++;
        nodo.CantidadHijos++;
        if (nodo.CantidadClaves <= maxClaves)
            return null;
        return _DividirInterno(nodo);
    }

    private (int, NodoBPlus) _DividirHoja(NodoBPlus hoja)
    {
        int punto = hoja.CantidadClaves / 2;
        NodoBPlus nuevaHoja = new NodoBPlus(orden, hoja: true);
        for (int i = punto; i < hoja.CantidadClaves; i++)
        {
            nuevaHoja.Claves[nuevaHoja.CantidadClaves] = hoja.Claves[i];
            nuevaHoja.CantidadClaves++;
        }
        hoja.CantidadClaves = punto;
        nuevaHoja.Siguiente = hoja.Siguiente;
        hoja.Siguiente = nuevaHoja;
        int claveGuia = nuevaHoja.Claves[0];
        return (claveGuia, nuevaHoja);
    }

    private (int, NodoBPlus) _DividirInterno(NodoBPlus nodo)
    {
        int centro = nodo.CantidadClaves / 2;
        int claveQueSube = nodo.Claves[centro];
        NodoBPlus nuevoNodo = new NodoBPlus(orden, hoja: false);
        for (int i = centro + 1; i < nodo.CantidadClaves; i++)
        {
            nuevoNodo.Claves[nuevoNodo.CantidadClaves] = nodo.Claves[i];
            nuevoNodo.CantidadClaves++;
        }
        for (int i = centro + 1; i < nodo.CantidadHijos; i++)
        {
            nuevoNodo.Hijos[nuevoNodo.CantidadHijos] = nodo.Hijos[i];
            nuevoNodo.CantidadHijos++;
        }
        nodo.CantidadClaves = centro;
        nodo.CantidadHijos = centro + 1;

        return (claveQueSube, nuevoNodo);
    }

    public bool Buscar(int clave)
    {
        return _Buscar(raiz, clave);
    }

    private bool _Buscar(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            for (int i = 0; i < nodo.CantidadClaves; i++)
            {
                if (nodo.Claves[i] == clave) return true;
            }
            return false;
        }
        int posicion = 0;
        while (posicion < nodo.CantidadClaves && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }
        return _Buscar(nodo.Hijos[posicion], clave);
    }

    public void Mostrar()
    {
        _Mostrar(raiz, 0);
    }

    private void _Mostrar(NodoBPlus nodo, int nivel)
    {
        string espacios = new string(' ', nivel * 4);
        string elementos = "";
        for (int i = 0; i < nodo.CantidadClaves; i++)
        {
            elementos += nodo.Claves[i];
            if (i < nodo.CantidadClaves - 1) elementos += ", ";
        }
        if (nodo.Hoja)
        {
            Console.WriteLine($"{espacios}Hoja: [{elementos}]");
        }
        else
        {
            Console.WriteLine($"{espacios}Interno: [{elementos}]");
            for (int i = 0; i < nodo.CantidadHijos; i++)
            {
                _Mostrar(nodo.Hijos[i], nivel + 1);
            }
        }
    }
}

public class MinHeap
{
    private int[] heap;     
    private int cantidad;   
    public MinHeap(int capacidadInicial = 10)
    {
        heap = new int[capacidadInicial];
        cantidad = 0;
    }

    private void Redimensionar()
    {
        int[] nuevoHeap = new int[heap.Length * 2];
        for (int i = 0; i < cantidad; i++)
        {
            nuevoHeap[i] = heap[i];
        }
        heap = nuevoHeap;
    }

    public void Insertar(int valor)
    {
        if (cantidad == heap.Length)
        {
            Redimensionar(); 
        }
        heap[cantidad] = valor; 
        HeapifyUp(cantidad);    
        cantidad++;             
    }

    private void HeapifyUp(int indice)
    {
        while (indice > 0)
        {
            int padre = (indice - 1) / 2;
            if (heap[indice] < heap[padre])
            {
                int temp = heap[indice];
                heap[indice] = heap[padre];
                heap[padre] = temp;
                indice = padre;
            }
            else
            {
                break;
            }
        }
    }

    public int? ObtenerMinimo()
    {
        if (cantidad == 0) return null;
        return heap[0];
    }

    public int? EliminarMinimo()
    {
        if (cantidad == 0) return null;
        int minimo = heap[0];
        heap[0] = heap[cantidad - 1];
        cantidad--;
        if (cantidad > 0)
        {
            HeapifyDown(0);
        }
        return minimo;
    }

    private void HeapifyDown(int indice)
    {
        while (true)
        {
            int menor = indice;
            int hijoIzquierdo = 2 * indice + 1;
            int hijoDerecho = 2 * indice + 2;
            if (hijoIzquierdo < cantidad && heap[hijoIzquierdo] < heap[menor])
            {
                menor = hijoIzquierdo;
            }
            if (hijoDerecho < cantidad && heap[hijoDerecho] < heap[menor])
            {
                menor = hijoDerecho;
            }
            if (menor == indice) break;
            int temp = heap[indice];
            heap[indice] = heap[menor];
            heap[menor] = temp;
            indice = menor;
        }
    }

    public bool Buscar(int valor)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (heap[i] == valor) return true;
        }
        return false;
    }

    public void Mostrar()
    {
        Console.Write("[");
        for (int i = 0; i < cantidad; i++)
        {
            Console.Write(heap[i] + (i < cantidad - 1 ? ", " : ""));
        }
        Console.WriteLine("]");
    }
}

public class MaxHeap
{
    private int[] heap;
    private int cantidad;
    public MaxHeap(int capacidadInicial = 10)
    {
        heap = new int[capacidadInicial];
        cantidad = 0;
    }

    private void Redimensionar()
    {
        int[] nuevoHeap = new int[heap.Length * 2];
        for (int i = 0; i < cantidad; i++)
        {
            nuevoHeap[i] = heap[i];
        }
        heap = nuevoHeap;
    }

    public void Insertar(int valor)
    {
        if (cantidad == heap.Length)
        {
            Redimensionar();
        }
        heap[cantidad] = valor;
        HeapifyUp(cantidad);
        cantidad++;
    }

    private void HeapifyUp(int indice)
    {
        while (indice > 0)
        {
            int padre = (indice - 1) / 2;
            if (heap[indice] > heap[padre]) 
            {
                int temp = heap[indice];
                heap[indice] = heap[padre];
                heap[padre] = temp;
                indice = padre;
            }
            else
            {
                break;
            }
        }
    }

    public int? ObtenerMaximo()
    {
        if (cantidad == 0) return null;
        return heap[0];
    }

    public int? EliminarMaximo()
    {
        if (cantidad == 0) return null;
        int maximo = heap[0];
        heap[0] = heap[cantidad - 1];
        cantidad--;
        if (cantidad > 0)
        {
            HeapifyDown(0);
        }
        return maximo;
    }

    private void HeapifyDown(int indice)
    {
        while (true)
        {
            int mayor = indice;
            int hijoIzquierdo = 2 * indice + 1;
            int hijoDerecho = 2 * indice + 2;
            if (hijoIzquierdo < cantidad && heap[hijoIzquierdo] > heap[mayor])
            {
                mayor = hijoIzquierdo;
            }
            if (hijoDerecho < cantidad && heap[hijoDerecho] > heap[mayor])
            {
                mayor = hijoDerecho;
            }
            if (mayor == indice) break;
            int temp = heap[indice];
            heap[indice] = heap[mayor];
            heap[mayor] = temp;
            indice = mayor;
        }
    }

    public bool Buscar(int valor)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (heap[i] == valor) return true;
        }
        return false;
    }

    public void Mostrar()
    {
        Console.Write("[");
        for (int i = 0; i < cantidad; i++)
        {
            Console.Write(heap[i] + (i < cantidad - 1 ? ", " : ""));
        }
        Console.WriteLine("]");
    }
}

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

< Window x: Class = "Proyecto1_ED2.MainWindow"
        xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns: x = "http://schemas.microsoft.com/winfx/2006/xaml"
        Title = "Gestión del Mundial - Estructura de Datos II" Height = "600" Width = "900" WindowStartupLocation = "CenterScreen" >

    < Grid >
        < Grid.ColumnDefinitions >
            < !--Columna del menú lateral -->
            <ColumnDefinition Width="220"/>
            <!-- Columna del contenido principal -->
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        < !--Columna del menú lateral -->
        <Border Grid.Column="0" Background="#2C3E50" Padding="15">
            <StackPanel>
                <TextBlock Text="MENÚ MUNDIAL" Foreground="White" FontSize="20" FontWeight="Bold" HorizontalAlignment="Center" Margin="0,0,0,30"/>
                <Button x:Name = "btnCargarArchivo" Content = "Cargar Datos (CSV)" Height = "40" Margin = "0,0,0,10" Background = "#34495E" Foreground = "White" Click = "btnCargarArchivo_Click" />
                < Button x: Name = "btnRegistrar" Content = "Registrar Jugador" Height = "40" Margin = "0,0,0,10" Background = "#34495E" Foreground = "White" Click = "btnRegistrar_Click" />
                < Button x: Name = "btnBuscar" Content = "Buscar Jugador" Height = "40" Margin = "0,0,0,10" Background = "#34495E" Foreground = "White" Click = "btnBuscar_Click" />
                < TextBlock Text = "REPORTES" Foreground = "#BDC3C7" FontSize = "14" FontWeight = "SemiBold" Margin = "0,20,0,10" />
                < Button x: Name = "btnTopGoles" Content = "Top 5 Goleadores" Height = "40" Margin = "0,0,0,10" Background = "#E67E22" Foreground = "White" Click = "btnTopGoles_Click" />
                < Button x: Name = "btnTopAsistencias" Content = "Top 5 Asistencias" Height = "40" Margin = "0,0,0,10" Background = "#E67E22" Foreground = "White" Click = "btnTopAsistencias_Click" />
                < Button x: Name = "btnMostrarTodos" Content = "Mostrar Todos" Height = "40" Margin = "0,0,0,10" Background = "#2980B9" Foreground = "White" Click = "btnMostrarTodos_Click" />
            </ StackPanel >
        </ Border >

        < !--Área de Contenido Central -->
        <Grid Grid.Column="1" Background="#ECF0F1" Padding="20">
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>
            
            <TextBlock x:Name = "txtTituloSeccion" Grid.Row = "0" Text = "Tabla General de Jugadores" FontSize = "24" FontWeight = "Bold" Foreground = "#2C3E50" Margin = "0,0,0,15" />


            < !--Tabla para mostrar los datos -->
            <DataGrid x:Name = "dgJugadores" Grid.Row = "1" AutoGenerateColumns = "True" IsReadOnly = "True" HeadersVisibility = "Column" Background = "White" RowHeight = "30" FontSize = "14" />
        </ Grid >
    </ Grid >
</ Window >

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