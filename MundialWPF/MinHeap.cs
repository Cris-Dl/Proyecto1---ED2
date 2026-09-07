using System;

namespace MundialWPF
{
    public class MinHeap
    {
        private Jugador[] arreglo;
        private int capacidad;
        private int tamaño;
        private string criterio;

        public MinHeap(string criterio, int capacidad = 100)
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

        public void Insertar(Jugador jugador)
        {
            if (tamaño == capacidad) return;
            arreglo[tamaño] = jugador;
            HundirHaciaArriba(tamaño);
            tamaño++;
        }

        private void HundirHaciaArriba(int indice)
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

        public Jugador ExtraerMinimo()
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

        private void HundirHaciaAbajo(int indice)
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

        public Jugador[] ObtenerTop(int cantidad)
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
    }
}