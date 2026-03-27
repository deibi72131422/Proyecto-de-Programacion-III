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
// chavales esto era para ver si funcionaba y ns como mierda funciona aaaaaaaaaaa, te extraño  karla volve --ENSERIO PIENSA Q VA A VOLVER
namespace proyectodeloschikis
{
    /// <summary>
    /// Lógica de interacción para UcHanoi.xaml
    /// </summary>
    public partial class UcHanoi : UserControl
    {
        private int contadorMovimientos = 0;
        private List<Rectangle> discos = new List<Rectangle>();
        private List<Stack<Rectangle>> torres = new List<Stack<Rectangle>>();
        private int numDiscos = 5;
        private bool resolviendo = false;

        public UcHanoi()
        {
            InitializeComponent();
            torres.Add(new Stack<Rectangle>()); // Torre A (0)
            torres.Add(new Stack<Rectangle>()); // Torre B (1)
            torres.Add(new Stack<Rectangle>()); // Torre C (2)
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (resolviendo) return;
            numDiscos = int.Parse((cmbDiscos.SelectedItem as ComboBoxItem).Content.ToString());
            IniciarTorres();
        }

        private async void BtnResolver_Click(object sender, RoutedEventArgs e)
        {
            if (resolviendo || torres[0].Count == 0) return;

            resolviendo = true;
            lstMovimientos.Items.Clear();
            await HanoiRecursivo(numDiscos, 0, 2, 1);   // Mover de A(0) a C(2) usando B(1)
            resolviendo = false;
        }

        private void IniciarTorres()
        {
            // Limpiar todo
            discos.Clear();
            foreach (var torre in torres) torre.Clear();
            canvasA.Children.Clear();
            canvasB.Children.Clear();
            canvasC.Children.Clear();
            lstMovimientos.Items.Clear();
            contadorMovimientos = 0;
            txtContador.Text = "Movimientos: 0";

            double anchoBase = 160;
            double alturaDisco = 28;

            for (int i = numDiscos; i >= 1; i--)
            {
                double ancho = anchoBase * (i / (double)numDiscos);
                Rectangle disco = new Rectangle
                {
                    Width = ancho,
                    Height = alturaDisco,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)(100 + i * 20), (byte)(50 + i * 15), 255)),
                    RadiusX = 8,
                    RadiusY = 8,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };

                // Posicionar en la base de la Torre A
                Canvas.SetLeft(disco, (180 - ancho) / 2);
                Canvas.SetTop(disco, 280 - (numDiscos - i + 1) * 28 - 10);

                canvasA.Children.Add(disco);
                discos.Add(disco);
                torres[0].Push(disco);
            }
        }

        // ====================== RECURSIVIDAD CLÁSICA CON ANIMACIÓN ======================
        private async Task HanoiRecursivo(int n, int origen, int destino, int auxiliar)
        {
            if (n == 1)
            {
                await MoverDisco(origen, destino);
                return;
            }

            await HanoiRecursivo(n - 1, origen, auxiliar, destino);
            await MoverDisco(origen, destino);
            await HanoiRecursivo(n - 1, auxiliar, destino, origen);
        }

        private async Task MoverDisco(int desde, int hacia)
        {
            if (torres[desde].Count == 0) return;

            Rectangle disco = torres[desde].Pop();
            torres[hacia].Push(disco);

            Canvas canvasDesde = desde == 0 ? canvasA : desde == 1 ? canvasB : canvasC;
            Canvas canvasHacia = hacia == 0 ? canvasA : hacia == 1 ? canvasB : canvasC;

            // Quitar del canvas viejo
            canvasDesde.Children.Remove(disco);

            // Agregar al nuevo canvas
            canvasHacia.Children.Add(disco);

            double alturaDisco = 28;

            // Nueva posición
            double nuevaY = 280 - torres[hacia].Count * alturaDisco - 10;
            double nuevaX = (180 - disco.Width) / 2;

            // 🔥 TELETRANSPORTE DIRECTO
            Canvas.SetLeft(disco, nuevaX);
            Canvas.SetTop(disco, nuevaY);

            // Registro
            lstMovimientos.Items.Add($"Mover disco de {(char)('A' + desde)} → {(char)('A' + hacia)}");
            contadorMovimientos++;
            txtContador.Text = "Movimientos: " + contadorMovimientos;
            lstMovimientos.ScrollIntoView(lstMovimientos.Items[lstMovimientos.Items.Count - 1]);

            await Task.Delay(200); // opcional (para que se vea el proceso)
        }
    }
}
