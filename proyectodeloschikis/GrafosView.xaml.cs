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
    // Clase para las conexiones de los nodos
    public class Arista
    {
        public Vertice vDestino { get; set; }
        public int pPeso { get; set; } = 1;
        public Arista(Vertice d) { vDestino = d; }
    }

    // Clase para los circulos o nodos
    public class Vertice
    {
        public string nNombre { get; set; }
        public Point pPosicion { get; set; }
        public List<Arista> lAristas { get; set; } = new List<Arista>();
        public Vertice(string n, Point p) { nNombre = n; pPosicion = p; }
    }

    public partial class GrafosView : UserControl
    {
        // Variables globales del grafo
        private List<Vertice> listaVertices = new List<Vertice>();
        private int contadorId = 1;
        private const double radioNodo = 15;

        public GrafosView()
        {
            InitializeComponent();
        }

        // Evento para crear un nuevo nodo en el dibujo
        private void NuevoVertice_Click(object sender, RoutedEventArgs e)
        {
            // Si el cuadro de texto esta vacio usa el contador D
            string nD = string.IsNullOrEmpty(D_txtNuevoNodo.Text) ? contadorId.ToString() : D_txtNuevoNodo.Text;

            // Revisa que el nombre del nodo no este repetido en la lista
            if (!listaVertices.Any(v => v.nNombre == nD))
            {
                listaVertices.Add(new Vertice(nD, new Point(0, 0)));
                if (string.IsNullOrEmpty(D_txtNuevoNodo.Text)) contadorId++;

                // Ordena y dibuja de nuevo
                AcomodarNodosEnCirculo();
                DibujarGrafoCompleto();
            }
            D_txtNuevoNodo.Clear();
        }

        // Evento para conectar dos nodos existentes
        private void NuevaArista_Click(object sender, RoutedEventArgs e)
        {
            // Busca los nodos por el nombre que pusiste en los TXT
            var vO = listaVertices.FirstOrDefault(v => v.nNombre == D_txtO.Text);
            var vD = listaVertices.FirstOrDefault(v => v.nNombre == D_txtD.Text);

            // Solo crea la linea si ambos existen y no son el mismo
            if (vO != null && vD != null && vO != vD)
            {
                if (!vO.lAristas.Any(a => a.vDestino == vD))
                {
                    vO.lAristas.Add(new Arista(vD));
                    DibujarGrafoCompleto();
                }
            }
            LimpiarEntradasTexto();
        }

        // Evento para borrar un nodo especifico
        private void EliminarNodo_Click(object sender, RoutedEventArgs e)
        {
            // Busca el nodo segun el texto ingresado
            var vE = listaVertices.FirstOrDefault(v => v.nNombre == D_txtNuevoNodo.Text);
            if (vE != null)
            {
                // Borra el nodo y tambien las flechas que apuntaban a el
                listaVertices.Remove(vE);
                foreach (var vT in listaVertices) vT.lAristas.RemoveAll(a => a.vDestino == vE);

                AcomodarNodosEnCirculo();
                DibujarGrafoCompleto();
            }
            D_txtNuevoNodo.Clear();
        }

        // Evento para crear un lazo (apunta a si mismo)
        private void AgregarLazo_Click(object sender, RoutedEventArgs e)
        {
            var vN = listaVertices.FirstOrDefault(v => v.nNombre == D_txtO.Text);
            if (vN != null && !vN.lAristas.Any(a => a.vDestino == vN))
            {
                vN.lAristas.Add(new Arista(vN));
                DibujarGrafoCompleto();
            }
            LimpiarEntradasTexto();
        }

        // Limpia toda la pantalla y resetea datos
        private void VaciarGrafo_Click(object sender, RoutedEventArgs e)
        {
            listaVertices.Clear();
            contadorId = 1;
            D_txtV.Text = "0";
            D_txtA.Text = "0";
            canvasGrafo.Children.Clear();
        }

        // Recorrido BFS usando una cola
        private void BFS_Click(object sender, RoutedEventArgs e)
        {
            var vI = listaVertices.FirstOrDefault(v => v.nNombre == D_txtO.Text);
            if (vI == null) return;

            var vis = new List<Vertice>();
            var col = new Queue<Vertice>();
            col.Enqueue(vI);
            vis.Add(vI);

            while (col.Count > 0)
            {
                var vA = col.Dequeue();
                foreach (var a in vA.lAristas)
                {
                    if (!vis.Contains(a.vDestino)) { vis.Add(a.vDestino); col.Enqueue(a.vDestino); }
                }
            }
            MessageBox.Show("Ruta BFS: " + string.Join(" -> ", vis.Select(v => v.nNombre)));
        }

        // Algoritmo Dijkstra para buscar costos
        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            var vO = listaVertices.FirstOrDefault(v => v.nNombre == D_txtO.Text);
            var vD = listaVertices.FirstOrDefault(v => v.nNombre == D_txtD.Text);
            if (vO == null || vD == null) return;

            var dist = new Dictionary<Vertice, int>();
            var lP = new List<Vertice>(listaVertices);

            // Inicializa distancias en un numero muy alto
            foreach (var v in listaVertices) dist[v] = 99999;
            dist[vO] = 0;

            while (lP.Count > 0)
            {
                var vA = lP.OrderBy(v => dist[v]).First();
                lP.Remove(vA);
                if (vA == vD) break;

                foreach (var a in vA.lAristas)
                {
                    int nD = dist[vA] + a.pPeso;
                    if (nD < dist[a.vDestino]) dist[a.vDestino] = nD;
                }
            }
            MessageBox.Show("Costo total Dijkstra: " + dist[vD]);
        }

        // Metodo para que los nodos no queden uno encima de otro
        private void AcomodarNodosEnCirculo()
        {
            if (listaVertices.Count == 0) return;
            double cX = canvasGrafo.ActualWidth / 2;
            double cY = canvasGrafo.ActualHeight / 2;
            double rC = 180; // Radio del circulo imaginario
            double aP = 2 * Math.PI / listaVertices.Count;

            for (int i = 0; i < listaVertices.Count; i++)
            {
                double x = cX + rC * Math.Cos(i * aP);
                double y = cY + rC * Math.Sin(i * aP);
                listaVertices[i].pPosicion = new Point(x, y);
            }
        }

        // Dibuja todo el contenido del canvas
        private void DibujarGrafoCompleto()
        {
            canvasGrafo.Children.Clear();

            // Primero dibuja todas las aristas
            foreach (var v in listaVertices)
            {
                foreach (var a in v.lAristas)
                {
                    if (v == a.vDestino) DibujarAristaLazo(v);
                    else DibujarLineaConFlecha(v.pPosicion, a.vDestino.pPosicion);
                }
            }

            // dibuja los  (nodos) encima
            foreach (var v in listaVertices)
            {
                Ellipse eN = new Ellipse { 
                    Width = 30,
                    Height = 30,
                    Fill = Brushes.MediumPurple,
                    Stroke = Brushes.White,
                    StrokeThickness = 1 };
                Canvas.SetLeft(eN, v.pPosicion.X - radioNodo);
                Canvas.SetTop(eN, v.pPosicion.Y - radioNodo);
                canvasGrafo.Children.Add(eN);

                // Nombre del nodo

                TextBlock tN = new TextBlock { 
                    Text = v.nNombre,
                    Foreground = Brushes.White, 
                    FontWeight = FontWeights.Bold };
                Canvas.SetLeft(tN, v.pPosicion.X - 6);
                Canvas.SetTop(tN, v.pPosicion.Y - 8);
                canvasGrafo.Children.Add(tN);
            }

            // Actualiza los contadores de la interfaz
            D_txtV.Text = listaVertices.Count.ToString();
            D_txtA.Text = listaVertices.Sum(v => v.lAristas.Count).ToString();
        }

        // Crea la linea y la punta de la flecha
        private void DibujarLineaConFlecha(Point pI, Point pF)
        {
            canvasGrafo.Children.Add(new Line { 
                X1 = pI.X,
                Y1 = pI.Y, 
                X2 = pF.X, 
                Y2 = pF.Y, Stroke = Brushes.White, StrokeThickness = 2 });

            // Calculo para poner la flecha al final de la linea
            double ang = Math.Atan2(pF.Y - pI.Y, pF.X - pI.X);
            Point pP = new Point(pF.X - radioNodo * Math.Cos(ang), pF.Y - radioNodo * Math.Sin(ang));
            Point p1 = new Point(pP.X - 10 * Math.Cos(ang + Math.PI / 6), pP.Y - 10 * Math.Sin(ang + Math.PI / 6));
            Point p2 = new Point(pP.X - 10 * Math.Cos(ang - Math.PI / 6), pP.Y - 10 * Math.Sin(ang - Math.PI / 6));

            canvasGrafo.Children.Add(new Polygon { Fill = Brushes.White, Points = new PointCollection { pP, p1, p2 } });
        }

        // Crea la curva para el lazo
        private void DibujarAristaLazo(Vertice v)
        {
            Point pI = new Point(v.pPosicion.X - 8, v.pPosicion.Y - 13);
            Point pF = new Point(v.pPosicion.X + 8, v.pPosicion.Y - 13);

            PathFigure pf = new PathFigure { StartPoint = pI };
            pf.Segments.Add(new BezierSegment
            {
                Point1 = new Point(v.pPosicion.X - 25, v.pPosicion.Y - 45),
                Point2 = new Point(v.pPosicion.X + 25, v.pPosicion.Y - 45),
                Point3 = pF
            });

            canvasGrafo.Children.Add(new Path { Data = new PathGeometry { Figures = { pf } }, Stroke = Brushes.White, StrokeThickness = 2 });

            // Flecha del lazo
            canvasGrafo.Children.Add(new Polygon { Fill = Brushes.White, Points = new PointCollection { pF, new Point(pF.X - 5, pF.Y - 10), new Point(pF.X + 10, pF.Y - 2) } });
        }

        private void LimpiarEntradasTexto() { D_txtO.Clear(); D_txtD.Clear(); D_txtNuevoNodo.Clear(); }
    }
}