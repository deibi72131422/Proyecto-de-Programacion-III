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
using System.Data;

namespace proyectodeloschikis
{
    // Clase para las conexiones entre los nodos
    public class Arista
    {
        public Vertice VerticeDestino { get; set; }
        public int Peso { get; set; } = 1;

        public Arista(Vertice destino)
        {
            VerticeDestino = destino;
        }
    }

    // Clase para representar cada punto o circulo del grafo
    public class Vertice
    {
        public string Nombre { get; set; }
        public Point Posicion { get; set; }
        public List<Arista> ListaAristas { get; set; } = new List<Arista>();

        public Vertice(string nombre, Point posicion)
        {
            Nombre = nombre;
            Posicion = posicion;
        }
    }

    public partial class GrafosView : UserControl
    {
        private Vertice D_verticeSeleccionado = null;
        private bool D_estaArrastrando = false;

        private List<Vertice> listaVertices = new List<Vertice>();
        private int contadorId = 1;
        private const double radioNodo = 15;

        public GrafosView()
        {
            InitializeComponent();
        }

        // Busca un vertice por su nombre ignorando mayusculas
        private Vertice BuscarVertice(string nombre)
        {
            foreach (var vertice in listaVertices)
            {
                if (string.Equals(vertice.Nombre?.Trim(), nombre, StringComparison.OrdinalIgnoreCase))
                    return vertice;
            }
            return null;
        }

        // Revisa si el nombre del nodo ya existe en la lista
        private bool ExisteVertice(string nombre)
        {
            foreach (var vertice in listaVertices)
            {
                if (string.Equals(vertice.Nombre?.Trim(), nombre, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        // Verifica si ya hay una linea conectando dos nodos especificos
        private bool ExisteArista(Vertice origen, Vertice destino)
        {
            foreach (var arista in origen.ListaAristas)
            {
                if (arista.VerticeDestino == destino)
                    return true;
            }
            return false;
        }

        // Suma todas las conexiones creadas en el grafo
        private int ContarAristas()
        {
            int total = 0;
            foreach (var vertice in listaVertices)
            {
                total = total + vertice.ListaAristas.Count;
            }
            return total;
        }

        // Formatea la lista de nodos para mostrarla como una ruta de texto
        private string ObtenerRuta(List<Vertice> lista)
        {
            List<string> nombres = new List<string>();
            foreach (var vertice in lista)
            {
                nombres.Add(vertice.Nombre);
            }
            return string.Join(" -> ", nombres);
        }

        // Crea un nuevo nodo y lo posiciona en la pantalla
        public void NuevoVertice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = string.IsNullOrWhiteSpace(D_txtNuevoNodo.Text) ? contadorId.ToString() : D_txtNuevoNodo.Text;
                if (ExisteVertice(nombre))
                {
                    MessageBox.Show("Nombre duplicado");
                    return;
                }
                listaVertices.Add(new Vertice(nombre, new Point(50, 50)));
                contadorId++;
                AcomodarNodosEnCirculo();
                DibujarGrafoCompleto();
                D_txtNuevoNodo.Clear();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Crea una conexion entre el nodo origen y el destino
        public void NuevaArista_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vertice origen = BuscarVertice(D_txtO.Text);
                Vertice destino = BuscarVertice(D_txtD.Text);
                if (origen == null || destino == null) return;
                if (!ExisteArista(origen, destino))
                    origen.ListaAristas.Add(new Arista(destino));
                DibujarGrafoCompleto();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Crea una conexion del nodo consigo mismo
        public void AgregarLazo_Click(object sender, RoutedEventArgs e)
        {
            Vertice origen = BuscarVertice(D_txtO.Text);
            if (origen != null)
            {
                if (!ExisteArista(origen, origen))
                    origen.ListaAristas.Add(new Arista(origen));
                DibujarGrafoCompleto();
            }
        }

        // Genera la representacion visual de la matriz con V1, V2, V3, V4
        public void MatrizAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            int n = listaVertices.Count;
            if (n == 0) return;

            string D_res = "MATRIZ DE ADYACENCIA\n\n      ";

            for (int i = 0; i < n; i++)
            {
                D_res = D_res + ("V" + (i + 1)).PadRight(5);
            }

            D_res = D_res + "\n" + new string('-', (n * 5) + 6) + "\n";

            for (int i = 0; i < n; i++)
            {
                string D_filaEtiqueta = "V" + (i + 1);
                D_res = D_res + D_filaEtiqueta.PadRight(4) + "| ";

                for (int j = 0; j < n; j++)
                {
                    int D_valor = ExisteArista(listaVertices[i], listaVertices[j]) ? 1 : 0;
                    D_res = D_res + D_valor.ToString().PadRight(5);
                }
                D_res = D_res + "\n";
            }

            MessageBox.Show(D_res);
        }

        // Muestra la lista de conexiones para cada vertice
        public void ListaAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            int n = listaVertices.Count;
            if (n == 0) return;

            string D_res = "LISTA DE ADYACENCIA\n\n";

            for (int i = 0; i < n; i++)
            {
                // Identificador del nodo actual
                D_res = D_res + "V" + (i + 1) + ": ";

                List<string> conexiones = new List<string>();
                foreach (var arista in listaVertices[i].ListaAristas)
                {
                    // Buscamos la posicion del destino para mostrarlo como V
                    int indiceDestino = listaVertices.IndexOf(arista.VerticeDestino) + 1;
                    conexiones.Add("V" + indiceDestino);
                }

                D_res = D_res + string.Join(", ", conexiones) + "\n";
            }

            MessageBox.Show(D_res);
        }

        // Borra un nodo y quita todas las flechas que apuntaban a el
        public void EliminarNodo_Click(object sender, RoutedEventArgs e)
        {
            Vertice vEliminar = BuscarVertice(D_txtO.Text);
            if (vEliminar != null)
            {
                listaVertices.Remove(vEliminar);
                foreach (var vActual in listaVertices)
                {
                    for (int i = vActual.ListaAristas.Count - 1; i >= 0; i--)
                    {
                        if (vActual.ListaAristas[i].VerticeDestino == vEliminar)
                        {
                            vActual.ListaAristas.RemoveAt(i);
                        }
                    }
                }
                DibujarGrafoCompleto();
            }
        }

        // Limpia todo el panel de dibujo y reinicia los datos
        public void VaciarGrafo_Click(object sender, RoutedEventArgs e)
        {
            listaVertices.Clear();
            contadorId = 1;
            DibujarGrafoCompleto();
        }

        // Recorrido a lo ancho usando una cola
        public void BFS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vertice inicio = BuscarVertice(D_txtO.Text);
                if (inicio == null) return;
                List<Vertice> visitados = new List<Vertice>();
                Queue<Vertice> cola = new Queue<Vertice>();
                cola.Enqueue(inicio);
                visitados.Add(inicio);
                while (cola.Count > 0)
                {
                    Vertice actual = cola.Dequeue();
                    foreach (var arista in actual.ListaAristas)
                    {
                        if (!visitados.Contains(arista.VerticeDestino))
                        {
                            visitados.Add(arista.VerticeDestino);
                            cola.Enqueue(arista.VerticeDestino);
                        }
                    }
                }
                MessageBox.Show("BFS: " + ObtenerRuta(visitados));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Recorrido en profundidad usando recursion
        public void DFS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vertice inicio = BuscarVertice(D_txtO.Text);
                if (inicio == null) return;
                HashSet<Vertice> visitados = new HashSet<Vertice>();
                List<Vertice> recorrido = new List<Vertice>();
                void DFSRecursivo(Vertice actual)
                {
                    visitados.Add(actual);
                    recorrido.Add(actual);
                    foreach (var arista in actual.ListaAristas)
                    {
                        if (!visitados.Contains(arista.VerticeDestino)) { DFSRecursivo(arista.VerticeDestino); }
                    }
                }
                DFSRecursivo(inicio);
                MessageBox.Show("DFS: " + ObtenerRuta(recorrido));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Algoritmo para encontrar el camino mas corto
        public void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vertice origen = BuscarVertice(D_txtO.Text);
                Vertice destino = BuscarVertice(D_txtD.Text);
                if (origen == null || destino == null) return;
                Dictionary<Vertice, int> distancias = new Dictionary<Vertice, int>();
                List<Vertice> pendientes = new List<Vertice>();
                foreach (var v in listaVertices) { distancias[v] = 99999; pendientes.Add(v); }
                distancias[origen] = 0;
                while (pendientes.Count > 0)
                {
                    Vertice actual = pendientes[0];
                    foreach (var vP in pendientes) { if (distancias[vP] < distancias[actual]) actual = vP; }
                    pendientes.Remove(actual);
                    if (distancias[actual] == 99999) break;
                    foreach (var arista in actual.ListaAristas)
                    {
                        int nuevaDistancia = distancias[actual] + arista.Peso;
                        if (nuevaDistancia < distancias[arista.VerticeDestino]) distancias[arista.VerticeDestino] = nuevaDistancia;
                    }
                }
                MessageBox.Show("Costo Dijkstra: " + distancias[destino]);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void CanvasGrafo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(canvasGrafo);
            D_verticeSeleccionado = null;
            foreach (var v in listaVertices)
            {
                double d = Math.Sqrt(Math.Pow(v.Posicion.X - p.X, 2) + Math.Pow(v.Posicion.Y - p.Y, 2));
                if (d < radioNodo + 10) { D_verticeSeleccionado = v; break; }
            }
            if (D_verticeSeleccionado != null) D_estaArrastrando = true;
        }

        public void CanvasGrafo_MouseMove(object sender, MouseEventArgs e)
        {
            if (D_estaArrastrando && D_verticeSeleccionado != null)
            {
                D_verticeSeleccionado.Posicion = e.GetPosition(canvasGrafo);
                DibujarGrafoCompleto();
            }
        }

        public void CanvasGrafo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            D_estaArrastrando = false;
            D_verticeSeleccionado = null;
        }

        private void AcomodarNodosEnCirculo()
        {
            if (listaVertices.Count == 0) return;
            double centroX = 400, centroY = 300, radio = 180;
            double anguloPaso = 2 * Math.PI / listaVertices.Count;
            for (int i = 0; i < listaVertices.Count; i++)
            {
                double x = centroX + radio * Math.Cos(i * anguloPaso);
                double y = centroY + radio * Math.Sin(i * anguloPaso);
                listaVertices[i].Posicion = new Point(x, y);
            }
        }

        // Dibuja las flechas y nodos
        private void DibujarGrafoCompleto()
        {
            canvasGrafo.Children.Clear();

            foreach (var vOrigen in listaVertices)
            {
                foreach (var a in vOrigen.ListaAristas)
                {
                    Vertice vDestino = a.VerticeDestino;

                    if (vOrigen == vDestino)
                    {
                        // Dibujo del Lazo (Bucle)
                        Ellipse lazo = new Ellipse { Width = 25, Height = 25, Stroke = Brushes.White, StrokeThickness = 1.5 };
                        Canvas.SetLeft(lazo, vOrigen.Posicion.X - 12);
                        Canvas.SetTop(lazo, vOrigen.Posicion.Y - 35);
                        canvasGrafo.Children.Add(lazo);
                    }
                    else
                    {
                        // Calculo para la flecha
                        double x1 = vOrigen.Posicion.X;
                        double y1 = vOrigen.Posicion.Y;
                        double x2 = vDestino.Posicion.X;
                        double y2 = vDestino.Posicion.Y;

                        Line linea = new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.White, StrokeThickness = 1.5 };
                        canvasGrafo.Children.Add(linea);

                        double angulo = Math.Atan2(y2 - y1, x2 - x1);
                        double puntaX = x2 - 15 * Math.Cos(angulo);
                        double puntaY = y2 - 15 * Math.Sin(angulo);

                        Polygon flecha = new Polygon { Fill = Brushes.White };
                        flecha.Points.Add(new Point(puntaX, puntaY));
                        flecha.Points.Add(new Point(puntaX - 10 * Math.Cos(angulo - 0.4), puntaY - 10 * Math.Sin(angulo - 0.4)));
                        flecha.Points.Add(new Point(puntaX - 10 * Math.Cos(angulo + 0.4), puntaY - 10 * Math.Sin(angulo + 0.4)));
                        canvasGrafo.Children.Add(flecha);
                    }
                }
            }

            foreach (var v in listaVertices)
            {
                Ellipse eN = new Ellipse { Width = 30, Height = 30, Fill = Brushes.MediumPurple, Stroke = Brushes.White, StrokeThickness = 2 };
                Canvas.SetLeft(eN, v.Posicion.X - 15);
                Canvas.SetTop(eN, v.Posicion.Y - 15);
                canvasGrafo.Children.Add(eN);

                TextBlock tN = new TextBlock { Text = v.Nombre, Foreground = Brushes.White, FontWeight = FontWeights.Bold };
                Canvas.SetLeft(tN, v.Posicion.X - 6);
                Canvas.SetTop(tN, v.Posicion.Y - 8);
                canvasGrafo.Children.Add(tN);
            }

            D_txtV.Text = listaVertices.Count.ToString();
            D_txtA.Text = ContarAristas().ToString();
        }
    }
}