using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace OpenTK_U
{
    public class Window : GameWindow
    {
        private List<ULetter>? _uLetters;                                   // Lista que guarda todas las letras U de la escena
        private int _shader;                                                // ID del programa shader cargado
        private Matrix4 _view;                                              // Matriz de vista (cámara)
        private Matrix4 _projection;                                       // Matriz de proyección (perspectiva)

        private enum TransformMode { Translate, Rotate, Scale }            // Modo de transformación actual
        private TransformMode _currentMode = TransformMode.Translate;      // Por defecto se empieza en modo mover

        private int _selectedLetterIndex = -1;                      // Índice de la letra seleccionada
        private int _selectedPillarIndex = -1;                      // Índice del pilar seleccionado
        private bool _controlAllPillars = false;                    // Si es true, se transforman todos los pilares de la letra
        private bool _modoGlobalActivo = false;                     // Si es true, se transforman automáticamente todas las letras

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.DarkOrange);
            GL.Enable(EnableCap.DepthTest);

            _shader = Shader.LoadShader("shader.vert", "shader.frag");

            _uLetters = new List<ULetter>();
            var escenario = new Escenario("modelo_u.json");      // Carga el modelo de la letra U desde archivo JSON          

            int numLetters = 3; // <----------------------------- CANTIDAD DE U - AQUI PUEDES AUMENTAR
            float minX = -5f, maxX = 5f, minY = -3f, maxY = 3f, minZ = -5f, maxZ = 5f;
            float spacing = 2f;
            Random random = new Random();
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < numLetters; i++)
            {
                Vector3 newPosition;                           // Nueva posición aleatoria de la letra
                bool isOverlapping;                            // Bandera para evitar superposición con otras letras

                do
                {
                    float x = (float)(random.NextDouble() * (maxX - minX) + minX);
                    float y = (float)(random.NextDouble() * (maxY - minY) + minY);
                    float z = (float)(random.NextDouble() * (maxZ - minZ) + minZ);
                    newPosition = new Vector3(x, y, z);

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

                positions.Add(newPosition);
                _uLetters.Add(new ULetter(newPosition, escenario.Vertices, escenario.Indices));     // Crea una nueva letra U y la agrega a la lista
            }

            _view = Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.UnitY);             // Configura la cámara mirando al centro
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), 800f / 600f, 0.1f, 100f);  // Matriz de proyección en perspectiva
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (_uLetters == null) return;

            float delta = (float)args.Time;
            const float moveSpeed = 2f;
            const float rotSpeed = 50f;
            const float scaleSpeed = 1f;

            if (KeyboardState.IsKeyPressed(Keys.T)) _currentMode = TransformMode.Translate;     // Cambia al modo Mover
            if (KeyboardState.IsKeyPressed(Keys.R)) _currentMode = TransformMode.Rotate;        // Rotar
            if (KeyboardState.IsKeyPressed(Keys.E)) _currentMode = TransformMode.Scale;         // Escalar

            if (KeyboardState.IsKeyPressed(Keys.M))
                _modoGlobalActivo = !_modoGlobalActivo;                 // Alterna el modo "Mover Escenario"

            if (_modoGlobalActivo)
            {
                foreach (var letra in _uLetters)                 // Si el modo "Mover Escenario" está activo, aplica transformaciones automáticas a todas las letras
                {
                    foreach (var pilar in letra.Pilares)
                    {
                        AplicarTransformacion(pilar, delta, moveSpeed, rotSpeed, scaleSpeed);
                    }
                }

                return; // Ya transformamos todo, no hace falta seguir
            }

            if (KeyboardState.IsKeyPressed(Keys.A))
                _controlAllPillars = !_controlAllPillars;              // Alterna entre controlar 1 pilar o todos

            if (KeyboardState.IsKeyPressed(Keys.D0)) _selectedLetterIndex = 0;    // <-------- CANTIDAD DE U - Aumentar mas lineas si se quiere añadir mas
            if (KeyboardState.IsKeyPressed(Keys.D1)) _selectedLetterIndex = 1;
            if (KeyboardState.IsKeyPressed(Keys.D2)) _selectedLetterIndex = 2;

            if (KeyboardState.IsKeyPressed(Keys.Z)) _selectedPillarIndex = 0;
            if (KeyboardState.IsKeyPressed(Keys.X)) _selectedPillarIndex = 1;
            if (KeyboardState.IsKeyPressed(Keys.C)) _selectedPillarIndex = 2;     

            if (_selectedLetterIndex >= 0 && _selectedLetterIndex < _uLetters.Count)
            {
                var selected = _uLetters[_selectedLetterIndex];
                List<Pilar> pilaresToTransform = _controlAllPillars ? selected.Pilares : new List<Pilar>();

                if (!_controlAllPillars && _selectedPillarIndex >= 0 && _selectedPillarIndex < selected.Pilares.Count)
                {
                    pilaresToTransform.Add(selected.Pilares[_selectedPillarIndex]);
                }

                foreach (var pilar in pilaresToTransform)
                {
                    AplicarTransformacion(pilar, delta, moveSpeed, rotSpeed, scaleSpeed);    // Aplica la transformación correspondiente al pilar
                }
            }
        }

        private void AplicarTransformacion(Pilar pilar, float delta, float moveSpeed, float rotSpeed, float scaleSpeed)
        {
            switch (_currentMode)
            {
                case TransformMode.Translate:
                    if (KeyboardState.IsKeyDown(Keys.Up)) pilar.Position += new Vector3(0, moveSpeed * delta, 0);
                    if (KeyboardState.IsKeyDown(Keys.Down)) pilar.Position -= new Vector3(0, moveSpeed * delta, 0);
                    if (KeyboardState.IsKeyDown(Keys.Left)) pilar.Position -= new Vector3(moveSpeed * delta, 0, 0);
                    if (KeyboardState.IsKeyDown(Keys.Right)) pilar.Position += new Vector3(moveSpeed * delta, 0, 0);
                    if (KeyboardState.IsKeyDown(Keys.W)) pilar.Position -= new Vector3(0, 0, moveSpeed * delta);
                    if (KeyboardState.IsKeyDown(Keys.S)) pilar.Position += new Vector3(0, 0, moveSpeed * delta);
                    break;

                case TransformMode.Rotate:
                    if (KeyboardState.IsKeyDown(Keys.Up)) pilar.Rotation += new Vector3(rotSpeed * delta, 0, 0);
                    if (KeyboardState.IsKeyDown(Keys.Down)) pilar.Rotation -= new Vector3(rotSpeed * delta, 0, 0);
                    if (KeyboardState.IsKeyDown(Keys.Left)) pilar.Rotation += new Vector3(0, rotSpeed * delta, 0);
                    if (KeyboardState.IsKeyDown(Keys.Right)) pilar.Rotation -= new Vector3(0, rotSpeed * delta, 0);
                    if (KeyboardState.IsKeyDown(Keys.W)) pilar.Rotation += new Vector3(0, 0, rotSpeed * delta);
                    if (KeyboardState.IsKeyDown(Keys.S)) pilar.Rotation -= new Vector3(0, 0, rotSpeed * delta);
                    break;

                case TransformMode.Scale:
                    if (KeyboardState.IsKeyDown(Keys.Up)) pilar.Scale += new Vector3(0, scaleSpeed * delta, 0);
                    if (KeyboardState.IsKeyDown(Keys.Down)) pilar.Scale -= new Vector3(0, scaleSpeed * delta, 0);
                    if (KeyboardState.IsKeyDown(Keys.Left)) pilar.Scale -= new Vector3(scaleSpeed * delta, 0, 0);
                    if (KeyboardState.IsKeyDown(Keys.Right)) pilar.Scale += new Vector3(scaleSpeed * delta, 0, 0);
                    if (KeyboardState.IsKeyDown(Keys.W)) pilar.Scale += new Vector3(0, 0, scaleSpeed * delta);
                    if (KeyboardState.IsKeyDown(Keys.S)) pilar.Scale -= new Vector3(0, 0, scaleSpeed * delta);

                    pilar.Scale = Vector3.ComponentMax(pilar.Scale, new Vector3(0.1f));         // Previene que la escala se vuelva cero o negativa
                    break;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            if (_uLetters == null) return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);       // Limpia la pantalla antes de renderizar
            GL.UseProgram(_shader);                                                      // Activa el shader antes de dibujar
            int mvpLocation = GL.GetUniformLocation(_shader, "mvp");

            foreach (var u in _uLetters)
            {
                foreach (var pilar in u.Pilares)
                {
                    Matrix4 model =
                        Matrix4.CreateScale(pilar.Scale) *                              // Calcula la matriz de modelo (transformaciones del objeto)
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(pilar.Rotation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(pilar.Rotation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(pilar.Rotation.Z)) *            
                        Matrix4.CreateTranslation(pilar.Position + u.Position);                 

                    Matrix4 mvp = model * _view * _projection;                      // Multiplica modelo, vista y proyección para obtener la matriz final
                    GL.UniformMatrix4(mvpLocation, false, ref mvp);                 // Envía la matriz MVP al shader

                    pilar.Draw();                                           // Dibuja el pilar en pantalla con OpenGL
                }
            }

            SwapBuffers();                          // Intercambia los buffers para mostrar el frame dibujado
        }
    }
}
