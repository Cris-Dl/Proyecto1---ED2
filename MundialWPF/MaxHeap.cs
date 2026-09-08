using System;

namespace MundialWPF 
{
    public class MaxHeap //Clase MaxHeap para manejar la estructura de datos de heap máximo
    {
        private Jugador[] heap;
        private int cantidad;
        private string criterio; 
        public MaxHeap(string criterio, int capacidadInicial = 10)
        {
            heap = new Jugador[capacidadInicial];
            cantidad = 0;
            this.criterio = criterio;
        }

        private void Redimensionar() //Método para redimensionar el arreglo del heap cuando se llena
        {
            Jugador[] nuevoHeap = new Jugador[heap.Length * 2];
            for (int i = 0; i < cantidad; i++)
            {
                nuevoHeap[i] = heap[i];
            }
            heap = nuevoHeap;
        }

        public void Insertar(Jugador jugador) //Método para insertar un jugador en el heap
        {
            if (cantidad == heap.Length)
            {
                Redimensionar();
            }
            heap[cantidad] = jugador;
            HeapifyUp(cantidad);
            cantidad++;
        }

        private void HeapifyUp(int index) //Método para mantener la propiedad del heap después de insertar un nuevo elemento
        {
            int padre = (index - 1) / 2;
            while (index > 0 && Comparar(heap[index], heap[padre]) > 0)
            {
                Jugador temp = heap[index];
                heap[index] = heap[padre];
                heap[padre] = temp;

                index = padre;
                padre = (index - 1) / 2;
            }
        }

        private int Comparar(Jugador j1, Jugador j2) //Método para comparar dos jugadores según el criterio de ordenamiento
        {
            if (criterio == "Goles")
            {
                return j1.Goles.CompareTo(j2.Goles);
            }
            else if (criterio == "Asistencias")
            {
                return j1.Asistencias.CompareTo(j2.Asistencias);
            }
            return 0;
        }

        public Jugador[] ObtenerTop(int topN) //Método para obtener los N mejores jugadores según el criterio de ordenamiento
        {
            int limite = Math.Min(topN, cantidad);
            Jugador[] top = new Jugador[limite];
            Jugador[] copiaHeap = new Jugador[cantidad];
            Array.Copy(heap, copiaHeap, cantidad);
            int cantidadOriginal = cantidad;

            for (int i = 0; i < limite; i++)
            {
                top[i] = ExtraerMax();
            }
            heap = copiaHeap;
            cantidad = cantidadOriginal;
            return top;
        }

        public Jugador ExtraerMax() //Método para extraer el jugador con el valor máximo según el criterio de ordenamiento
        {
            if (cantidad == 0) return null;
            Jugador max = heap[0];
            heap[0] = heap[cantidad - 1];
            heap[cantidad - 1] = null;
            cantidad--;
            HeapifyDown(0);
            return max;
        }

        private void HeapifyDown(int index) //Método para mantener la propiedad del heap después de extraer el elemento máximo
        {
            int mayor = index;
            int izquierdo = 2 * index + 1;
            int derecho = 2 * index + 2;
            if (izquierdo < cantidad && Comparar(heap[izquierdo], heap[mayor]) > 0)
                mayor = izquierdo;
            if (derecho < cantidad && Comparar(heap[derecho], heap[mayor]) > 0)
                mayor = derecho;
            if (mayor != index)
            {
                Jugador temp = heap[index];
                heap[index] = heap[mayor];
                heap[mayor] = temp;
                HeapifyDown(mayor);
            }
        }
        public void Eliminar(string nombre) //Método para eliminar un jugador del heap por su nombre
        {
            int posicion = -1;

            for (int i = 0; i < cantidad; i++)
            {
                if (heap[i].Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    posicion = i;
                    break;
                }
            }
            if (posicion == -1) return;
            heap[posicion] = heap[cantidad - 1];
            heap[cantidad - 1] = null;
            cantidad--;
            if (posicion < cantidad)
            {
                HeapifyUp(posicion);
                HeapifyDown(posicion);
            }
        }

    }
}