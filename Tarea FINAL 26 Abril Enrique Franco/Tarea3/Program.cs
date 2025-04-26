using System;
using OpenTK.Windowing.Desktop;

namespace OpenTK_U
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "Letra U 3D - FINAL - 26 ABRIL - Enrique Franco Moron",
            };

            // Crea una instancia de la clase Window y la ejecuta
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();  // Inicia el ciclo principal del juego (renderizado, actualización de frames, etc.)
            }
        }
    }
}