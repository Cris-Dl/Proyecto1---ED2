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

public class Program
{
    public static void Main()
    {
        ArbolBPlus arbol = new ArbolBPlus(orden: 4);
        int[] codigos = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        foreach (int codigo in codigos)
        {
            arbol.Insertar(codigo);
        }
        Console.WriteLine("ESTRUCTURA DEL ÁRBOL");
        arbol.Mostrar();
        int codigoBuscado = 70;
        if (arbol.Buscar(codigoBuscado))
        {
            Console.WriteLine($"\nEl código {codigoBuscado} fue encontrado.");
        }
        else
        {
            Console.WriteLine($"\nEl código {codigoBuscado} no existe.");
        }
        codigoBuscado = 55;
        if (arbol.Buscar(codigoBuscado))
        {
            Console.WriteLine($"El código {codigoBuscado} fue encontrado.");
        }
        else
        {
            Console.WriteLine($"El código {codigoBuscado} no existe.");
        }
    }
}

public class MinHeap
{
    private List<int> heap;
    public MinHeap()
    {
        heap = new List<int>();
    }

    public void InsertarMin(int valor)
    {
        heap.Add(valor);
        int indice = heap.Count - 1;
        HeapifyUp(indice);
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
        if (heap.Count == 0) return null;
        return heap[0];
    }

    public int? EliminarMinimo()
    {
        if (heap.Count == 0) return null;
        if (heap.Count == 1)
        {
            int valor = heap[0];
            heap.RemoveAt(0);
            return valor;
        }
        int minimo = heap[0]; 
        int ultimo = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1); 
        heap[0] = ultimo; 
        HeapifyDown(0); 
        return minimo;
    }

    private void HeapifyDown(int indice)
    {
        int cantidad = heap.Count;
        while(true)
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
        return heap.Contains(valor);
    }

    public void Mostrar()
    {
        Console.WriteLine("[" + string.Join(", ", heap) + "]");
    }
}

public class MaxHeap
{
    private List<int> heap;

    public MaxHeap()
    {
        heap = new List<int>();
    }
}