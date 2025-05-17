using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AnimacionAuto
{
    public class Ventana : GameWindow
    {
        private Escena escena;

        public Ventana(int ancho, int alto, string titulo)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (ancho, alto), Title = titulo })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            escena = new Escena();
            escena.CargarContenido();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            escena.Dibujar();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            escena.Actualizar((float)args.Time);
        }
    }
}
