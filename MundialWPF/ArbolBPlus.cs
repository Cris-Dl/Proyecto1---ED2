using System;

namespace MundialWPF
{
    public class NodoBPlus
    {
        public string[] Claves;
        public Jugador[]? Valores;
        public NodoBPlus[]? Hijos;
        public NodoBPlus? Siguiente;
        public NodoBPlus? Padre;
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

        public ArbolBPlus(int orden)
        {
            this.orden = orden;
            raiz = new NodoBPlus(orden, true);
        }

        public Jugador Buscar(string nombre)
        {
            NodoBPlus hoja = BuscarHoja(nombre);
            for (int i = 0; i < hoja.Cantidad; i++)
            {
                if (hoja.Claves[i].Equals(nombre, StringComparison.OrdinalIgnoreCase))
                    return hoja.Valores[i];
            }
            return null;
        }

        private NodoBPlus BuscarHoja(string clave)
        {
            NodoBPlus actual = raiz;
            while (!actual.EsHoja)
            {
                int i = 0;
                while (i < actual.Cantidad && string.Compare(clave, actual.Claves[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    i++;
                }
                actual = actual.Hijos[i];
            }
            return actual;
        }

        public Jugador[] ObtenerTodos()
        {
            NodoBPlus actual = raiz;
            while (!actual.EsHoja)
            {
                actual = actual.Hijos[0];
            }
            int total = 0;
            NodoBPlus temp = actual;
            while (temp != null)
            {
                total += temp.Cantidad;
                temp = temp.Siguiente;
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

        public void Insertar(Jugador jugador)
        {
            string clave = jugador.Nombre;
            NodoBPlus hoja = BuscarHoja(clave);
            int pos = 0;
            while (pos < hoja.Cantidad && string.Compare(clave, hoja.Claves[pos], StringComparison.OrdinalIgnoreCase) > 0)
            {
                pos++;
            }
            for (int i = hoja.Cantidad; i > pos; i--)
            {
                hoja.Claves[i] = hoja.Claves[i - 1];
                hoja.Valores[i] = hoja.Valores[i - 1];
            }
            hoja.Claves[pos] = clave;
            hoja.Valores[pos] = jugador;
            hoja.Cantidad++;
            if (hoja.Cantidad == orden)
            {
                DividirHoja(hoja);
            }
        }

        private void DividirHoja(NodoBPlus hoja)
        {
            int mitad = hoja.Cantidad / 2;
            NodoBPlus nuevaHoja = new NodoBPlus(orden, true);
            nuevaHoja.Padre = hoja.Padre; 
            int index = 0;
            for (int i = mitad; i < hoja.Cantidad; i++)
            {
                nuevaHoja.Claves[index] = hoja.Claves[i];
                nuevaHoja.Valores[index] = hoja.Valores[i];
                hoja.Claves[i] = null;
                hoja.Valores[i] = null;
                index++;
            }
            nuevaHoja.Cantidad = hoja.Cantidad - mitad;
            hoja.Cantidad = mitad;
            nuevaHoja.Siguiente = hoja.Siguiente;
            hoja.Siguiente = nuevaHoja;
            InsertarEnPadre(hoja, nuevaHoja.Claves[0], nuevaHoja);
        }

        private void InsertarEnPadre(NodoBPlus izquierdo, string claveGuia, NodoBPlus derecho)
        {
            if (izquierdo == raiz)
            {
                NodoBPlus nuevaRaiz = new NodoBPlus(orden, false);
                nuevaRaiz.Claves[0] = claveGuia;
                nuevaRaiz.Hijos[0] = izquierdo;
                nuevaRaiz.Hijos[1] = derecho;
                nuevaRaiz.Cantidad = 1;
                izquierdo.Padre = nuevaRaiz;
                derecho.Padre = nuevaRaiz;
                raiz = nuevaRaiz;
                return;
            }
            NodoBPlus padre = izquierdo.Padre;
            int pos = 0;
            while (pos < padre.Cantidad && padre.Hijos[pos] != izquierdo)
            {
                pos++;
            }
            for (int i = padre.Cantidad; i > pos; i--)
            {
                padre.Claves[i] = padre.Claves[i - 1];
                padre.Hijos[i + 1] = padre.Hijos[i];
            }
            padre.Claves[pos] = claveGuia;
            padre.Hijos[pos + 1] = derecho;
            padre.Cantidad++;
            derecho.Padre = padre; 
            if (padre.Cantidad == orden)
            {
                DividirInterno(padre);
            }
        }

        private void DividirInterno(NodoBPlus interno)
        {
            int mitad = interno.Cantidad / 2;
            NodoBPlus nuevoInterno = new NodoBPlus(orden, false);
            nuevoInterno.Padre = interno.Padre;
            string claveSube = interno.Claves[mitad];
            int index = 0;
            for (int i = mitad + 1; i < interno.Cantidad; i++)
            {
                nuevoInterno.Claves[index] = interno.Claves[i];
                interno.Claves[i] = null;
                index++;
            }
            index = 0;
            for (int i = mitad + 1; i <= interno.Cantidad; i++)
            {
                nuevoInterno.Hijos[index] = interno.Hijos[i];
                if (nuevoInterno.Hijos[index] != null)
                {
                    nuevoInterno.Hijos[index].Padre = nuevoInterno;
                }
                interno.Hijos[i] = null;
                index++;
            }
            nuevoInterno.Cantidad = interno.Cantidad - mitad - 1;
            interno.Cantidad = mitad;

            InsertarEnPadre(interno, claveSube, nuevoInterno);
        }

        public bool Eliminar(string clave)
        {
            NodoBPlus hoja = BuscarHoja(clave);
            int pos = 0;
            while (pos < hoja.Cantidad && string.Compare(hoja.Claves[pos], clave, StringComparison.OrdinalIgnoreCase) < 0)
            {
                pos++;
            }
            if (pos >= hoja.Cantidad || !hoja.Claves[pos].Equals(clave, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            for (int i = pos; i < hoja.Cantidad - 1; i++)
            {
                hoja.Claves[i] = hoja.Claves[i + 1];
                hoja.Valores[i] = hoja.Valores[i + 1];
            }
            hoja.Cantidad--;
            if (hoja == raiz) return true;
            int minClaves = (int)Math.Ceiling((orden - 1) / 2.0);
            if (hoja.Cantidad < minClaves)
            {
                RepararHoja(hoja);
            }
            RecalcularGuias(raiz);
            return true;
        }

        private void RepararHoja(NodoBPlus hoja)
        {
            NodoBPlus padre = hoja.Padre;
            if (padre == null) return;
            int pos = 0;
            while (pos < padre.Cantidad && padre.Hijos[pos] != hoja) pos++;
            NodoBPlus izq = (pos > 0) ? padre.Hijos[pos - 1] : null;
            NodoBPlus der = (pos + 1 <= padre.Cantidad) ? padre.Hijos[pos + 1] : null;
            int minClaves = (int)Math.Ceiling((orden - 1) / 2.0);
            if (izq != null && izq.Cantidad > minClaves)
            {
                for (int i = hoja.Cantidad; i > 0; i--)
                {
                    hoja.Claves[i] = hoja.Claves[i - 1];
                    hoja.Valores[i] = hoja.Valores[i - 1];
                }
                hoja.Claves[0] = izq.Claves[izq.Cantidad - 1];
                hoja.Valores[0] = izq.Valores[izq.Cantidad - 1];
                hoja.Cantidad++;
                izq.Cantidad--;
                return;
            }
            if (der != null && der.Cantidad > minClaves)
            {
                hoja.Claves[hoja.Cantidad] = der.Claves[0];
                hoja.Valores[hoja.Cantidad] = der.Valores[0];
                hoja.Cantidad++;

                for (int i = 0; i < der.Cantidad - 1; i++)
                {
                    der.Claves[i] = der.Claves[i + 1];
                    der.Valores[i] = der.Valores[i + 1];
                }
                der.Cantidad--;
                return;
            }
            if (izq != null)
            {
                for (int i = 0; i < hoja.Cantidad; i++)
                {
                    izq.Claves[izq.Cantidad + i] = hoja.Claves[i];
                    izq.Valores[izq.Cantidad + i] = hoja.Valores[i];
                }
                izq.Cantidad += hoja.Cantidad;
                izq.Siguiente = hoja.Siguiente;

                for (int i = pos; i < padre.Cantidad; i++)
                {
                    padre.Hijos[i] = padre.Hijos[i + 1];
                    padre.Claves[i - 1] = padre.Claves[i];
                }
                padre.Cantidad--;
            }
            else if (der != null)
            {
                for (int i = 0; i < der.Cantidad; i++)
                {
                    hoja.Claves[hoja.Cantidad + i] = der.Claves[i];
                    hoja.Valores[hoja.Cantidad + i] = der.Valores[i];
                }
                hoja.Cantidad += der.Cantidad;
                hoja.Siguiente = der.Siguiente;

                for (int i = pos + 1; i < padre.Cantidad; i++)
                {
                    padre.Hijos[i] = padre.Hijos[i + 1];
                    padre.Claves[i - 1] = padre.Claves[i];
                }
                padre.Cantidad--;
            }
        }

        private void RecalcularGuias(NodoBPlus nodo)
        {
            if (nodo.EsHoja) return;
            for (int i = 0; i <= nodo.Cantidad; i++)
            {
                if (nodo.Hijos[i] != null) RecalcularGuias(nodo.Hijos[i]);
            }
            for (int i = 0; i < nodo.Cantidad; i++)
            {
                if (nodo.Hijos[i + 1] != null)
                    nodo.Claves[i] = MinimoSubarbol(nodo.Hijos[i + 1]);
            }
        }

        private string MinimoSubarbol(NodoBPlus nodo)
        {
            while (!nodo.EsHoja) nodo = nodo.Hijos[0];
            return nodo.Claves[0];
        }
    }
}