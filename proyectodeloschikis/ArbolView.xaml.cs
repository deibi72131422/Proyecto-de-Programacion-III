using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace proyectodeloschikis
{
    // esta clase es para crear cada bolita del arbol
    public class Nodo
    {
        public string Valor { get; set; }
        public Nodo Izquierdo { get; set; }
        public Nodo Derecho { get; set; }

        public Nodo(string v)
        {
            Valor = v;
        }
    }
    public partial class ArbolView : UserControl
    {
        private Nodo raiz = null; // aqui guardamos el inicio de todo el arbol
        public ArbolView()
        {
            InitializeComponent();
        }

        // esta funcion sirve para comparar si los datos son numeros o letras
        private int Comparar(string v1, string v2)
        {
            // si marcamos enteros los convierte y compara cual es mas grande
            if (rbEnteros.IsChecked == true)
            {
                if (int.TryParse(v1, out int n1) && int.TryParse(v2, out int n2))
                    return n1.CompareTo(n2);
            }

            // si no son numeros los compara como texto normal
            return string.Compare(v1, v2, StringComparison.Ordinal);
        }

        // este es el boton para meter un dato nuevo
        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;
            raiz = InsertarRec(raiz, txtValor.Text);
            txtValor.Clear();
            DibujarArbol(); // despues de insertar volvemos a dibujar
        }

        // funcion que busca el lugar vacio para poner el nuevo nodo
        private Nodo InsertarRec(Nodo n, string v)
        {
            if (n == null) return new Nodo(v);
            int c = Comparar(v, n.Valor);
            if (c < 0) n.Izquierdo = InsertarRec(n.Izquierdo, v);
            else if (c > 0) n.Derecho = InsertarRec(n.Derecho, v);
            return n;
        }

        // boton para borrar un numero o letra
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;
            raiz = EliminarRec(raiz, txtValor.Text);
            txtValor.Clear();
            DibujarArbol();
        }

        // funcion que busca el nodo y lo quita acomodando los hijos
        private Nodo EliminarRec(Nodo n, string v)
        {
            if (n == null) return null;
            int c = Comparar(v, n.Valor);
            if (c < 0) n.Izquierdo = EliminarRec(n.Izquierdo, v);
            else if (c > 0) n.Derecho = EliminarRec(n.Derecho, v);
            else
            {
                if (n.Izquierdo == null) return n.Derecho;
                if (n.Derecho == null) return n.Izquierdo;
                Nodo m = n.Derecho;
                while (m.Izquierdo != null) m = m.Izquierdo;
                n.Valor = m.Valor;
                n.Derecho = EliminarRec(n.Derecho, m.Valor);
            }
            return n;
        }

        // este boton arregla el arbol para que no este mas largo de un lado
        private void Balancear_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) return;
            List<string> l = new List<string>();
            PasarALista(raiz, l); // primero pasamos todo a una lista
            raiz = CrearBalanceado(l, 0, l.Count - 1); // luego lo armamos bien parejo
            DibujarArbol();
            txtResultado.Text = "Arbol equilibrado correctamente";
        }

        // mete todos los datos del arbol en una lista ordenada
        private void PasarALista(Nodo n, List<string> l)
        {
            if (n == null) return;
            PasarALista(n.Izquierdo, l);
            l.Add(n.Valor);
            PasarALista(n.Derecho, l);
        }

        // agarra la lista y pone el del medio como raiz para que este balanceado
        private Nodo CrearBalanceado(List<string> l, int ini, int fin)
        {
            if (ini > fin) return null;
            int m = (ini + fin) / 2;
            Nodo n = new Nodo(l[m]);
            n.Izquierdo = CrearBalanceado(l, ini, m - 1);
            n.Derecho = CrearBalanceado(l, m + 1, fin);
            return n;
        }

        // busca si un valor existe o no
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;
            txtResultado.Text = (Encontrar(raiz, txtValor.Text) != null) ? "Si existe" : "No existe";
        }

        // baja por las ramas buscando el valor
        private Nodo Encontrar(Nodo n, string v)
        {
            if (n == null || n.Valor == v) return n;
            int c = Comparar(v, n.Valor);
            return c < 0 ? Encontrar(n.Izquierdo, v) : Encontrar(n.Derecho, v);
        }

        // este boton nos dice en que piso esta el numero
        private void Nivel_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;
            int niv = GetNiv(raiz, txtValor.Text, 1);
            txtResultado.Text = niv != -1 ? "Nivel: " + niv : "No encontrado";
        }

        // va contando mientras baja para saber el nivel
        private int GetNiv(Nodo n, string v, int niv)
        {
            if (n == null) return -1;
            int c = Comparar(v, n.Valor);
            if (c == 0) return niv;
            int sig = c < 0 ? GetNiv(n.Izquierdo, v, niv + 1) : GetNiv(n.Derecho, v, niv + 1);
            return sig;
        }

        // calcula que tan alto es el arbol
        private void Altura_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = "Altura: " + GetAlt(raiz);
        }

        private int GetAlt(Nodo n)
        {
            if (n == null) return 0;
            return 1 + Math.Max(GetAlt(n.Izquierdo), GetAlt(n.Derecho));
        }

        // busca el valor mas chico Tener encuenta que los menores van a la izquierda y los mayores por enden a la derecha
        private void Minimo_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) return;
            Nodo aux = raiz;
            while (aux.Izquierdo != null) aux = aux.Izquierdo;
            txtResultado.Text = "Minimo: " + aux.Valor;
        }

        // busca el valor mas grande siempre ubicados en la derecha
        private void Maximo_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) return;
            Nodo aux = raiz;
            while (aux.Derecho != null) aux = aux.Derecho;
            txtResultado.Text = "Maximo: " + aux.Valor;
        }

        // muestra el arbol en orden: izquierda, raiz, derecha
        private void InOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            PasarALista(raiz, l);
            txtResultado.Text = "InOrden: " + string.Join(", ", l);
        }

        // muestra el arbol en pre orden: raiz, izquierda, derecha
        private void PreOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            Pre(raiz, l);
            txtResultado.Text = "PreOrden: " + string.Join(", ", l);
        }
        private void Pre(Nodo n, List<string> l) { if (n == null) return; l.Add(n.Valor); Pre(n.Izquierdo, l); Pre(n.Derecho, l); }

        // muestra el arbol en post orden: izquierda, derecha, raiz
        private void PostOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            Post(raiz, l);
            txtResultado.Text = "PostOrden: " + string.Join(", ", l);
        }
        private void Post(Nodo n, List<string> l) { if (n == null) return; Post(n.Izquierdo, l); Post(n.Derecho, l); l.Add(n.Valor); }

        // borra todo para empezar de cero
        private void Reiniciar_Click(object sender, RoutedEventArgs e)
        {
            raiz = null;
            canvasArbol.Children.Clear();
            txtResultado.Clear();
        }

        // esta parte limpia el dibujo y lo vuelve a pintar
        private void DibujarArbol()
        {
            canvasArbol.Children.Clear();
            if (raiz != null)
            {
                // usamos 400 para que empiece en el centro de la pantalla
                DibujarNodo(raiz, 400, 40, 160);
            }
        }

        // esta funcion dibuja cada circulo y las lineas que los unen
        private void DibujarNodo(Nodo n, double x, double y, double s)
        {
            if (n == null) return;

            // dibuja la linea para el hijo izquierdo
            if (n.Izquierdo != null)
            {
                Line l = new Line
                {
                    X1 = x,
                    Y1 = y,
                    X2 = x - s,
                    Y2 = y + 60,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };
                canvasArbol.Children.Add(l);
                DibujarNodo(n.Izquierdo, x - s, y + 60, s / 2);
            }
            // dibuja la linea para el hijo derecho
            if (n.Derecho != null)
            {
                Line l = new Line
                {
                    X1 = x,
                    Y1 = y,
                    X2 = x + s,
                    Y2 = y + 60,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };
                canvasArbol.Children.Add(l);
                DibujarNodo(n.Derecho, x + s, y + 60, s / 2);
            }

            // crea el circulo morado del nodo
            Ellipse el = new Ellipse { Width = 32, Height = 32, Fill = Brushes.MediumPurple, Stroke = Brushes.White, StrokeThickness = 1.5 };
            Canvas.SetLeft(el, x - 16);
            Canvas.SetTop(el, y - 16);
            canvasArbol.Children.Add(el);

            // pone el valor adentro de la bolita
            TextBlock t = new TextBlock { Text = n.Valor, Foreground = Brushes.White, FontWeight = FontWeights.Bold, FontSize = 13 };
            Canvas.SetLeft(t, x - 9);
            Canvas.SetTop(t, y - 9);
            canvasArbol.Children.Add(t);
        }
    }
}