using System;

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