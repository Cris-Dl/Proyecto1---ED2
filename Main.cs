using System;
using System.Collections.Generic;

public class NodoBPlus
{
    public bool Hoja { get; set; }

    public List<int> Claves { get; set; }

    public List<NodoBPlus> Hijos { get; set; }

    public NodoBPlus Siguiente { get; set; }

    public NodoBPlus(bool hoja = true)
    {
        Hoja = hoja;
        Claves = new List<int>();
        Hijos = new List<NodoBPlus>();
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
        this.raiz = new NodoBPlus(hoja: true);
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

            NodoBPlus nuevaRaiz = new NodoBPlus(hoja: false);
            nuevaRaiz.Claves.Add(claveGuia);
            nuevaRaiz.Hijos.Add(raiz);
            nuevaRaiz.Hijos.Add(nodoDerecho);

            raiz = nuevaRaiz;
        }
    }

    private (int, NodoBPlus)? _Insertar(NodoBPlus nodo, int clave)
    {
        if (nodo.Hoja)
        {
            nodo.Claves.Add(clave);
            nodo.Claves.Sort();

            if (nodo.Claves.Count <= maxClaves)
                return null;

            return _DividirHoja(nodo);
        }

        int posicion = 0;

        while (posicion < nodo.Claves.Count && clave >= nodo.Claves[posicion])
        {
            posicion++;
        }

        var resultado = _Insertar(nodo.Hijos[posicion], clave);

        if (!resultado.HasValue)
            return null;

        var (claveGuia, nodoDerecho) = resultado.Value;

        nodo.Claves.Insert(posicion, claveGuia);
        nodo.Hijos.Insert(posicion + 1, nodoDerecho);

        if (nodo.Claves.Count <= maxClaves)
            return null;

        return _DividirInterno(nodo);
    }

    private (int, NodoBPlus) _DividirHoja(NodoBPlus hoja)
    {
        int punto = hoja.Claves.Count / 2;

        NodoBPlus nuevaHoja = new NodoBPlus(hoja: true);

        nuevaHoja.Claves = hoja.Claves.GetRange(punto, hoja.Claves.Count - punto);
        hoja.Claves = hoja.Claves.GetRange(0, punto);
        nuevaHoja.Siguiente = hoja.Siguiente;
        hoja.Siguiente = nuevaHoja;

        int claveGuia = nuevaHoja.Claves[0];

        return (claveGuia, nuevaHoja);
    }

    private (int, NodoBPlus) _DividirInterno(NodoBPlus nodo)
    {
        int centro = nodo.Claves.Count / 2;
        int claveQueSube = nodo.Claves[centro];

        NodoBPlus nuevoNodo = new NodoBPlus(hoja: false);

        nuevoNodo.Claves = nodo.Claves.GetRange(centro + 1, nodo.Claves.Count - (centro + 1));
        nuevoNodo.Hijos = nodo.Hijos.GetRange(centro + 1, nodo.Hijos.Count - (centro + 1));

        nodo.Claves = nodo.Claves.GetRange(0, centro);
        nodo.Hijos = nodo.Hijos.GetRange(0, centro + 1);

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
            return nodo.Claves.Contains(clave);
        }

        int posicion = 0;
        while (posicion < nodo.Claves.Count && clave >= nodo.Claves[posicion])
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
        if (nodo.Hoja)
        {
            Console.WriteLine($"{espacios}Hoja: [{string.Join(", ", nodo.Claves)}]");
        }
        else
        {
            Console.WriteLine($"{espacios}Interno: [{string.Join(", ", nodo.Claves)}]");
            foreach (var hijo in nodo.Hijos)
            {
                _Mostrar(hijo, nivel + 1);
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