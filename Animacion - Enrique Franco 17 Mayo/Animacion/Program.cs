using System;

namespace AnimacionAuto
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Ventana ventana = new Ventana(1080, 720, "Animacion Objeto - Tarea 17 Mayo - Enrique Franco"))
            {
                ventana.Run();
            }
        }
    }
}