using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;

namespace OpenTK_U
{
    public class Window : GameWindow
    {
        private List<ULetter> _uLetters;
        private int _shader;
        private Matrix4 _mvp;
        private float _rotation = 0.0f;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.DarkOrange);
            GL.Enable(EnableCap.DepthTest);

            _shader = Shader.LoadShader("shader.vert", "shader.frag");

            _uLetters = new List<ULetter>();

            // Parámetros para la generación aleatoria
            int numLetters = 10; // Número de letras U
            float minX = -5f, maxX = 5f; // Rango para posiciones X
            float minY = -3f, maxY = 3f; // Rango para posiciones Y
            float minZ = -5f, maxZ = 5f; // Rango para posiciones Z
            float spacing = 1.5f; // Espaciado mínimo para evitar colisiones

            Random random = new Random();
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < numLetters; i++)
            {
                Vector3 newPosition;
                bool isOverlapping;

                do
                {
                    // Generar una posición aleatoria
                    float x = (float)(random.NextDouble() * (maxX - minX) + minX);
                    float y = (float)(random.NextDouble() * (maxY - minY) + minY);
                    float z = (float)(random.NextDouble() * (maxZ - minZ) + minZ);
                    newPosition = new Vector3(x, y, z);

                    // Verificar si la nueva posición se solapa con alguna existente
                    isOverlapping = false;
                    foreach (var pos in positions)
                    {
                        if (Vector3.Distance(newPosition, pos) < spacing)
                        {
                            isOverlapping = true;
                            break;
                        }
                    }
                } while (isOverlapping);

                // Agregar la posición válida
                positions.Add(newPosition);
                _uLetters.Add(new ULetter(newPosition));
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _rotation += 0.0002f;
            Matrix4 model = Matrix4.CreateRotationY(_rotation);
            Matrix4 view = Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.UnitY);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), 800f / 600f, 0.1f, 100f);
            _mvp = model * view * projection;

            int mvpLocation = GL.GetUniformLocation(_shader, "mvp");
            GL.UseProgram(_shader);
            GL.UniformMatrix4(mvpLocation, false, ref _mvp);

            foreach (var u in _uLetters)
                u.Draw(_shader);

            SwapBuffers();
        }
    }
}
