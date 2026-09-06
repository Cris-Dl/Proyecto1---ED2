using System; //Programa en general

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

