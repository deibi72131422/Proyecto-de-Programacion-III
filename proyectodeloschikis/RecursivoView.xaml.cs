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
    /// <summary>
    /// Lógica de interacción para RecursivoView.xaml
    /// </summary>
    public partial class RecursivoView : UserControl
    {
        private int ejercicioActual = 0;
        private List<int> vectorActual = new List<int>();

        public RecursivoView()
        {
            InitializeComponent();
        }

        // ====================== EVENTOS DE BOTONES DEL MENÚ ======================
        private void BtnCapicua_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(1, "1. Número Capicúa", mostrarVector: false);
        private void BtnSumaVector_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(2, "2. Suma de Vector", mostrarVector: true);
        private void BtnProductoVector_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(3, "3. Producto de Vector", mostrarVector: true);
        private void BtnMenorVector_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(4, "4. Número Menor del Vector", mostrarVector: true);
        private void BtnMayorVector_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(5, "5. Número Mayor del Vector", mostrarVector: true);
        private void BtnFactorial_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(6, "6. Factorial", mostrarVector: false);
        private void BtnFibonacci_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(7, "7. Fibonacci", mostrarVector: false);
        private void BtnInvertir_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(8, "8. Invertir Número", mostrarVector: false);
        private void BtnSumaDigitos_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(9, "9. Suma de Dígitos", mostrarVector: false);
        private void BtnSuma1hastaN_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(10, "10. Suma 1 hasta N", mostrarVector: false);
        private void BtnParImpar_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(11, "11. Par o Impar", mostrarVector: false);
        private void BtnPosNeg_Click(object sender, RoutedEventArgs e) => PrepararEjercicio(12, "12. Positivo o Negativo", mostrarVector: false);
        private void BtnHanoi_Click(object sender, RoutedEventArgs e)
        {
            lblTitulo.Text = "13. Torres de Hanói";

            // Ocultamos los controles de número y vector
            txtInput.Visibility = Visibility.Collapsed;
            lblInput.Visibility = Visibility.Collapsed;
            panelVector.Visibility = Visibility.Collapsed;

            // Cargamos el UserControl de Hanoi
            MainContent.Content = new UcHanoi();
        }

        private void PrepararEjercicio(int numEjercicio, string titulo, bool mostrarVector)
        {
            ejercicioActual = numEjercicio;
            lblTitulo.Text = titulo;
            txtResultado.Text = "Resultado: ";

            // <<<=== ESTO ES LO NUEVO ===>>>
            // Si había un UserControl de Hanoi, lo quitamos
            if (MainContent.Content is UcHanoi)
            {
                MainContent.Content = null;
            }

            if (mostrarVector)
            {
                panelVector.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Collapsed;
                lblInput.Visibility = Visibility.Collapsed;
                txtVector.Clear();
                lstVectorVisual.ItemsSource = null;
                vectorActual.Clear();
            }
            else
            {
                panelVector.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Visible;
                lblInput.Visibility = Visibility.Visible;
                txtInput.Clear();
            }
        }

        // Actualiza la lista visual cuando escribes en txtVector
        private void txtVector_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVector.Text))
            {
                lstVectorVisual.ItemsSource = null;
                vectorActual.Clear();
                return;
            }

            try
            {
                vectorActual = txtVector.Text.Split(',')
                    .Select(x => int.Parse(x.Trim()))
                    .ToList();

                lstVectorVisual.ItemsSource = vectorActual;
            }
            catch
            {
                // Si hay error de formato, no actualiza la lista
            }
        }

        // ====================== BOTÓN CALCULAR ======================
        private void BtnCalcular_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = "Resultado: ";

            try
            {
                switch (ejercicioActual)
                {
                    case 1: // Capicúa
                        if (int.TryParse(txtInput.Text, out int numCap))
                            txtResultado.Text = $"Resultado: {(EsCapicua(numCap) ? "Sí es capicúa" : "No es capicúa")}";
                        break;

                    case 2: // Suma Vector
                        if (vectorActual.Count > 0)
                            txtResultado.Text = $"Resultado: {SumaVector(vectorActual, 0)}";
                        break;

                    case 3: // Producto Vector
                        if (vectorActual.Count > 0)
                            txtResultado.Text = $"Resultado: {ProductoVector(vectorActual, 0)}";
                        break;

                    case 4: // Menor Vector
                        if (vectorActual.Count > 0)
                            txtResultado.Text = $"Resultado: {MenorVector(vectorActual, 0)}";
                        break;

                    case 5: // Mayor Vector
                        if (vectorActual.Count > 0)
                            txtResultado.Text = $"Resultado: {MayorVector(vectorActual, 0)}";
                        break;

                    case 6: // Factorial
                        if (int.TryParse(txtInput.Text, out int nFact) && nFact >= 0)
                            txtResultado.Text = $"Resultado: {Factorial(nFact)}";
                        break;

                    case 7: // Fibonacci
                        if (int.TryParse(txtInput.Text, out int nFib) && nFib >= 0)
                            txtResultado.Text = $"Resultado: {Fibonacci(nFib)}";
                        break;

                    case 8: // Invertir Número
                        if (int.TryParse(txtInput.Text, out int nInv))
                            txtResultado.Text = $"Resultado: {InvertirNumero(nInv)}";
                        break;

                    case 9: // Suma de Dígitos
                        if (int.TryParse(txtInput.Text, out int nDig))
                            txtResultado.Text = $"Resultado: {SumaDigitos(nDig)}";
                        break;

                    case 10: // Suma 1 hasta N
                        if (int.TryParse(txtInput.Text, out int nSum))
                            txtResultado.Text = $"Resultado: {Suma1HastaN(nSum)}";
                        break;

                    case 11: // Par o Impar
                        if (int.TryParse(txtInput.Text, out int nPar))
                            txtResultado.Text = $"Resultado: {(EsPar(nPar) ? "Par" : "Impar")}";
                        break;

                    case 12: // Positivo o Negativo
                        if (int.TryParse(txtInput.Text, out int nPos))
                            txtResultado.Text = $"Resultado: {(nPos > 0 ? "Positivo" : nPos < 0 ? "Negativo" : "Cero")}";
                        break;

                    default:
                        txtResultado.Text = "Resultado: Selecciona un ejercicio primero";
                        break;
                }
            }
            catch (Exception ex)
            {
                txtResultado.Text = $"Resultado: Error → {ex.Message}";
            }
        }

        // ====================== FUNCIONES RECURSIVAS ======================

        // 1. Capicúa (usando reverso)
        private bool EsCapicua(int n)
        {
            return n == Reverso(n, 0);
        }

        private int Reverso(int n, int temp)
        {
            if (n == 0) return temp;
            return Reverso(n / 10, temp * 10 + n % 10);
        }

        // 2. Suma Vector
        private int SumaVector(List<int> vec, int i)
        {
            if (i == vec.Count) return 0;
            return vec[i] + SumaVector(vec, i + 1);
        }

        // 3. Producto Vector
        private long ProductoVector(List<int> vec, int i)
        {
            if (i == vec.Count) return 1;
            return vec[i] * ProductoVector(vec, i + 1);
        }

        // 4. Menor Vector
        private int MenorVector(List<int> vec, int i)
        {
            if (i == vec.Count - 1) return vec[i];
            int menorResto = MenorVector(vec, i + 1);
            return vec[i] < menorResto ? vec[i] : menorResto;
        }

        // 5. Mayor Vector
        private int MayorVector(List<int> vec, int i)
        {
            if (i == vec.Count - 1) return vec[i];
            int mayorResto = MayorVector(vec, i + 1);
            return vec[i] > mayorResto ? vec[i] : mayorResto;
        }

        // 6. Factorial
        private long Factorial(int n)
        {
            if (n <= 1) return 1;
            return n * Factorial(n - 1);
        }

        // 7. Fibonacci
        private long Fibonacci(int n)
        {
            if (n <= 1) return n;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        // 8. Invertir Número
        private int InvertirNumero(int n)
        {
            return Reverso(Math.Abs(n), 0);   // reutilizamos la función Reverso
        }

        // 9. Suma de Dígitos
        private int SumaDigitos(int n)
        {
            if (n == 0) return 0;
            return (n % 10) + SumaDigitos(n / 10);
        }

        // 10. Suma 1 hasta N
        private int Suma1HastaN(int n)
        {
            if (n <= 0) return 0;
            return n + Suma1HastaN(n - 1);
        }

        // 11. Par o Impar (recursivo simple)
        private bool EsPar(int n)
        {
            if (n == 0) return true;
            if (n == 1) return false;
            return EsPar(n - 2);   // o n + 2 si es negativo, pero asumimos positivo
        }
    }
}
