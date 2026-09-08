using System;

namespace MundialWPF 
{
    public class MinHeap //Clase MinHeap para manejar la estructura de datos de heap mínimo
    {
        private Jugador[] arreglo;
        private int capacidad;
        private int tamaño;
        private string criterio;

        public MinHeap(string criterio, int capacidad = 100) //Constructor que inicializa el heap con un criterio de ordenamiento y una capacidad máxima
        {
            this.criterio = criterio;
            this.capacidad = capacidad;
            this.tamaño = 0;
            this.arreglo = new Jugador[capacidad];
        }

        private int ObtenerValor(Jugador j)
        {
            if (criterio == "Tarjetas") return j.TarjetasRecibidas;
            if (criterio == "Minutos") return j.MinutosJugados;
            return 0;
        }

        public void Insertar(Jugador jugador) //Método para insertar un jugador en el heap
        {
            if (tamaño == capacidad) return;
            arreglo[tamaño] = jugador;
            HundirHaciaArriba(tamaño);
            tamaño++;
        }

        private void HundirHaciaArriba(int indice) //Método para mantener la propiedad del heap después de insertar un nuevo elemento
        {
            int indicePadre = (indice - 1) / 2;
            while (indice > 0 && ObtenerValor(arreglo[indice]) < ObtenerValor(arreglo[indicePadre]))
            {
                Jugador temp = arreglo[indice];
                arreglo[indice] = arreglo[indicePadre];
                arreglo[indicePadre] = temp;
                indice = indicePadre;
                indicePadre = (indice - 1) / 2;
            }
        }

        public Jugador ExtraerMinimo() //Método para extraer el jugador con el valor mínimo según el criterio de ordenamiento
        {
            if (tamaño <= 0) return null;
            if (tamaño == 1)
            {
                tamaño--;
                return arreglo[0];
            }
            Jugador raiz = arreglo[0];
            arreglo[0] = arreglo[tamaño - 1];
            tamaño--;
            HundirHaciaAbajo(0);
            return raiz;
        }

        private void HundirHaciaAbajo(int indice) //Método para mantener la propiedad del heap después de extraer el elemento mínimo
        {
            int indiceMenor = indice;
            int hijoIzquierdo = 2 * indice + 1;
            int hijoDerecho = 2 * indice + 2;
            if (hijoIzquierdo < tamaño && ObtenerValor(arreglo[hijoIzquierdo]) < ObtenerValor(arreglo[indiceMenor]))
            {
                indiceMenor = hijoIzquierdo;
            }
            if (hijoDerecho < tamaño && ObtenerValor(arreglo[hijoDerecho]) < ObtenerValor(arreglo[indiceMenor]))
            {
                indiceMenor = hijoDerecho;
            }
            if (indiceMenor != indice)
            {
                Jugador temp = arreglo[indice];
                arreglo[indice] = arreglo[indiceMenor];
                arreglo[indiceMenor] = temp;
                HundirHaciaAbajo(indiceMenor);
            }
        }

        public Jugador[] ObtenerTop(int cantidad) //Método para obtener los jugadores con los valores más bajos según el criterio de ordenamiento
        {
            if (tamaño == 0) return new Jugador[0];
            int numElementos = Math.Min(cantidad, tamaño);
            Jugador[] resultado = new Jugador[numElementos];
            MinHeap heapTemporal = new MinHeap(this.criterio, this.capacidad);
            heapTemporal.tamaño = this.tamaño;
            for (int i = 0; i < this.tamaño; i++)
            {
                heapTemporal.arreglo[i] = this.arreglo[i];
            }
            for (int i = 0; i < numElementos; i++)
            {
                resultado[i] = heapTemporal.ExtraerMinimo();
            }
            return resultado;
        }
     
        public void Eliminar(string nombre) //Método para eliminar un jugador del heap por su nombre
        {
            int posicion = -1;
            for (int i = 0; i < tamaño; i++)
            {
                if (arreglo[i].Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    posicion = i;
                    break;
                }
            }
            if (posicion == -1) return;
            arreglo[posicion] = arreglo[tamaño - 1];
            arreglo[tamaño - 1] = null; 
            tamaño--;
            if (posicion < tamaño)
            {
                HundirHaciaArriba(posicion);
                HundirHaciaAbajo(posicion);
            }
        }
    }
}