using System; //Programa en general
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

