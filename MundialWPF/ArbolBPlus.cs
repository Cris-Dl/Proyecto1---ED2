using System;

namespace MundialWPF
{
    public class NodoBPlus
    {
        public string[] Claves;
        public Jugador[] Valores;
        public NodoBPlus[] Hijos;
        public NodoBPlus Siguiente;
        public int Cantidad;
        public bool EsHoja;

        public NodoBPlus(int orden, bool esHoja)
        {
            EsHoja = esHoja;
            Cantidad = 0;
            Claves = new string[orden];
            if (esHoja) Valores = new Jugador[orden];
            else Hijos = new NodoBPlus[orden + 1];
        }
    }

    public class ArbolBPlus
    {
        private NodoBPlus raiz;
        private int orden;

        public ArbolBPlus(int orden = 4)
        {
            this.orden = orden;
            raiz = new NodoBPlus(orden, true);
        }

        public void Insertar(Jugador jugador)
        {
            NodoBPlus raizActual = raiz;
            if (raizActual.Cantidad == orden - 1)
            {
                NodoBPlus nuevaRaiz = new NodoBPlus(orden, false);
                raiz = nuevaRaiz;
                nuevaRaiz.Hijos[0] = raizActual;
                DividirHijo(nuevaRaiz, 0, raizActual);
                InsertarNoLleno(nuevaRaiz, jugador);
            }
            else
            {
                InsertarNoLleno(raizActual, jugador);
            }
        }

        private void DividirHijo(NodoBPlus padre, int i, NodoBPlus nodoLleno)
        {
            NodoBPlus nuevoNodo = new NodoBPlus(orden, nodoLleno.EsHoja);
            if (nodoLleno.EsHoja)
            {
                int cantIzquierda = (nodoLleno.Cantidad + 1) / 2;
                int cantDerecha = nodoLleno.Cantidad - cantIzquierda;
                nuevoNodo.Cantidad = cantDerecha;
                for (int j = 0; j < cantDerecha; j++)
                {
                    nuevoNodo.Claves[j] = nodoLleno.Claves[j + cantIzquierda];
                    nuevoNodo.Valores[j] = nodoLleno.Valores[j + cantIzquierda];
                }
                nodoLleno.Cantidad = cantIzquierda;
                nuevoNodo.Siguiente = nodoLleno.Siguiente;
                nodoLleno.Siguiente = nuevoNodo;
                for (int j = padre.Cantidad; j > i; j--)
                    padre.Hijos[j + 1] = padre.Hijos[j];
                padre.Hijos[i + 1] = nuevoNodo;
                for (int j = padre.Cantidad - 1; j >= i; j--)
                    padre.Claves[j + 1] = padre.Claves[j];
                padre.Claves[i] = nuevoNodo.Claves[0];
                padre.Cantidad++;
            }
            else
            {
                int mid = nodoLleno.Cantidad / 2;
                int cantIzquierda = mid;
                int cantDerecha = nodoLleno.Cantidad - mid - 1;
                nuevoNodo.Cantidad = cantDerecha;
                for (int j = 0; j < cantDerecha; j++)
                    nuevoNodo.Claves[j] = nodoLleno.Claves[j + mid + 1];
                for (int j = 0; j <= cantDerecha; j++)
                    nuevoNodo.Hijos[j] = nodoLleno.Hijos[j + mid + 1];
                nodoLleno.Cantidad = cantIzquierda;
                for (int j = padre.Cantidad; j > i; j--)
                    padre.Hijos[j + 1] = padre.Hijos[j];
                padre.Hijos[i + 1] = nuevoNodo;
                for (int j = padre.Cantidad - 1; j >= i; j--)
                    padre.Claves[j + 1] = padre.Claves[j];
                padre.Claves[i] = nodoLleno.Claves[mid];
                padre.Cantidad++;
            }
        }

        private void InsertarNoLleno(NodoBPlus nodo, Jugador jugador)
        {
            int i = nodo.Cantidad - 1;
            string clave = jugador.Nombre;
            if (nodo.EsHoja)
            {
                while (i >= 0 && string.Compare(clave, nodo.Claves[i], StringComparison.OrdinalIgnoreCase) < 0)
                {
                    nodo.Claves[i + 1] = nodo.Claves[i];
                    nodo.Valores[i + 1] = nodo.Valores[i];
                    i--;
                }
                nodo.Claves[i + 1] = clave;
                nodo.Valores[i + 1] = jugador;
                nodo.Cantidad++;
            }
            else
            {
                while (i >= 0 && string.Compare(clave, nodo.Claves[i], StringComparison.OrdinalIgnoreCase) < 0)
                {
                    i--;
                }
                i++;
                if (nodo.Hijos[i].Cantidad == orden - 1)
                {
                    DividirHijo(nodo, i, nodo.Hijos[i]);
                    if (string.Compare(clave, nodo.Claves[i], StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        i++;
                    }
                }
                InsertarNoLleno(nodo.Hijos[i], jugador);
            }
        }

        public Jugador Buscar(string nombre)
        {
            return BuscarRecursivo(raiz, nombre);
        }

        private Jugador BuscarRecursivo(NodoBPlus nodo, string nombre)
        {
            int i = 0;

            if (nodo.EsHoja)
            {
                while (i < nodo.Cantidad && string.Compare(nombre, nodo.Claves[i], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    i++;
                }
                if (i < nodo.Cantidad && nodo.Claves[i].Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return nodo.Valores[i];
                }
                return null;
            }
            else
            {
                while (i < nodo.Cantidad && string.Compare(nombre, nodo.Claves[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    i++;
                }
                return BuscarRecursivo(nodo.Hijos[i], nombre);
            }
        }

        public Jugador[] ObtenerTodos()
        {
            int total = 0;
            NodoBPlus actual = raiz;
            while (!actual.EsHoja)
                actual = actual.Hijos[0];
            NodoBPlus contador = actual;
            while (contador != null)
            {
                total += contador.Cantidad;
                contador = contador.Siguiente;
            }
            Jugador[] todos = new Jugador[total];
            int index = 0;
            while (actual != null)
            {
                for (int i = 0; i < actual.Cantidad; i++)
                {
                    todos[index++] = actual.Valores[i];
                }
                actual = actual.Siguiente;
            }
            return todos;
        }
    }
}