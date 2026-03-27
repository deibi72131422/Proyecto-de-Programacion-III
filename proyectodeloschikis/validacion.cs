using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace proyectodeloschikis
{
    internal static class validacion
    {
        // VALIDAR VACÍO
        public static bool NoVacio(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show(" No puede estar vacío");
                return false;
            }
            return true;
        }

        // VALIDAR ENTERO (ANTI ERRORES)
        public static bool EsEntero(string texto, out int valor)
        {
            texto = texto.Trim();

            if (!NoVacio(texto))
            {
                valor = 0;
                return false;
            }

            if (!int.TryParse(texto, out valor))
            {
                MessageBox.Show(" Debe ingresar un número válido");
                return false;
            }

            return true;
        }

        // VALIDAR CARÁCTER
        public static bool EsCaracter(string texto, out char valor)
        {
            texto = texto.Trim();

            if (!NoVacio(texto))
            {
                valor = '\0';
                return false;
            }

            if (texto.Length != 1)
            {
                MessageBox.Show(" Solo un carácter permitido");
                valor = '\0';
                return false;
            }

            if (!char.IsLetter(texto[0]))
            {
                MessageBox.Show(" Solo se permiten letras");
                valor = '\0';
                return false;
            }

            valor = texto[0];
            return true;
        }

        //  VALIDAR STRING GENERAL (para grafos)
        public static bool EsTextoValido(string texto, out string valor)
        {
            texto = texto.Trim();

            if (!NoVacio(texto))
            {
                valor = "";
                return false;
            }

            valor = texto;
            return true;
        }

        //  VALIDAR DOS CAMPOS (ORIGEN - DESTINO)
        public static bool ValidarDos(string t1, string t2, out string v1, out string v2)
        {
            v1 = "";
            v2 = "";

            if (!EsTextoValido(t1, out v1)) return false;
            if (!EsTextoValido(t2, out v2)) return false;

            return true;
        }

        //  MENSAJE PERSONALIZADO (por si quieres usarlo)
        public static void Error(string mensaje)
        {
            MessageBox.Show("⚠️ " + mensaje);
        }

        //  LIMPIAR TEXTO
        public static string Limpiar(string texto)
        {
            return texto.Trim();
        }

        //  VALIDAR ALFA-NUMERICO (letras y dígitos, sin espacios) con longitud máxima
        public static bool EsAlfaNumerico(string texto, out string valor, int maxLen = 50)
        {
            texto = texto?.Trim() ?? "";

            if (!NoVacio(texto))
            {
                valor = "";
                return false;
            }

            if (texto.Length > maxLen)
            {
                MessageBox.Show($"Texto demasiado largo (máx. {maxLen} caracteres)");
                valor = "";
                return false;
            }

            if (!texto.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("Solo letras y dígitos");
                valor = "";
                return false;
            }

            valor = texto;
            return true;
        }

        //  VALIDAR RANGO (por si lo necesitas)
        public static bool EnRango(int valor, int min, int max)
        {
            if (valor < min || valor > max)
            {
                MessageBox.Show($" Valor fuera de rango ({min} - {max})");
                return false;
            }
            return true;
        }

        //  TRY-CATCH GLOBAL (ANTI CRASH)
        public static void EjecutarSeguro(Action accion)
        {
            try
            {
                accion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Error controlado: " + ex.Message);
            }
        }
    }
}
