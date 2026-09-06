using System;

namespace MundialWPF
{
    public class MinHeap
    {
        private Jugador[] heap;
        private int cantidad;
        private string criterio; 

        public MinHeap(string criterio, int capacidadInicial = 10)
        {
            heap = new Jugador[capacidadInicial];
            cantidad = 0;
            this.criterio = criterio;
        }

        private void Redimensionar()
        {
            Jugador[] nuevoHeap = new Jugador[heap.Length * 2];
            for (int i = 0; i < cantidad; i++)
            {
                nuevoHeap[i] = heap[i];
            }
            heap = nuevoHeap;
        }

        public void Insertar(Jugador jugador)
        {
            if (cantidad == heap.Length)
            {
                Redimensionar();
            }
            heap[cantidad] = jugador;
            HeapifyUp(cantidad);
            cantidad++;
        }

        private void HeapifyUp(int index)
        {
            int padre = (index - 1) / 2;
            while (index > 0 && Comparar(heap[index], heap[padre]) < 0)
            {
                Jugador temp = heap[index];
                heap[index] = heap[padre];
                heap[padre] = temp;
                index = padre;
                padre = (index - 1) / 2;
            }
        }

        private int Comparar(Jugador j1, Jugador j2)
        {
            if (criterio == "Tarjetas")
                return j1.TarjetasRecibidas.CompareTo(j2.TarjetasRecibidas);
            else if (criterio == "Minutos")
                return j1.MinutosJugados.CompareTo(j2.MinutosJugados);
            else if (criterio == "Goles")
                return j1.Goles.CompareTo(j2.Goles);
            else if (criterio == "Asistencias")
                return j1.Asistencias.CompareTo(j2.Asistencias);

            return 0;
        }

        public Jugador[] ObtenerMinimos(int topN)
        {
            int limite = Math.Min(topN, cantidad);
            Jugador[] top = new Jugador[limite];
            Jugador[] copiaHeap = new Jugador[cantidad];
            Array.Copy(heap, copiaHeap, cantidad);
            int cantidadOriginal = cantidad;
            for (int i = 0; i < limite; i++)
            {
                top[i] = ExtraerMin();
            }
            heap = copiaHeap;
            cantidad = cantidadOriginal;
            return top;
        }

        public Jugador ExtraerMin()
        {
            if (cantidad == 0) return null;
            Jugador min = heap[0];
            heap[0] = heap[cantidad - 1];
            heap[cantidad - 1] = null;
            cantidad--;
            HeapifyDown(0);
            return min;
        }

        private void HeapifyDown(int index)
        {
            int menor = index;
            int izquierdo = 2 * index + 1;
            int derecho = 2 * index + 2;
            if (izquierdo < cantidad && Comparar(heap[izquierdo], heap[menor]) < 0)
                menor = izquierdo;
            if (derecho < cantidad && Comparar(heap[derecho], heap[menor]) < 0)
                menor = derecho;
            if (menor != index)
            {
                Jugador temp = heap[index];
                heap[index] = heap[menor];
                heap[menor] = temp;
                HeapifyDown(menor);
            }
        }
    }
}