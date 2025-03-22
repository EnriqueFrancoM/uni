using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTK_U
{
    public class Window : GameWindow
    {
        private int _vao, _vbo, _ebo, _shader;
        private Matrix4 _mvp;
        private float _rotation = 0.0f;
        private Vector3 _centerAxis = new Vector3(0f, 0f, 0f);

        private readonly uint[] indices = {
            // Front face
            0, 1, 2, 0, 2, 3,
            4, 5, 6, 4, 6, 7,

            // Back face
            8, 9, 10, 8, 10, 11,
            12, 13, 14, 12, 14, 15,

            // Sides
            0, 1, 9, 0, 9, 8,
            1, 2, 10, 1, 10, 9,
            2, 3, 11, 2, 11, 10,
            3, 0, 8, 3, 8, 11,
            4, 5, 13, 4, 13, 12,
            5, 6, 14, 5, 14, 13,
            6, 7, 15, 6, 15, 14,
            7, 4, 12, 7, 12, 15,

            // Base inferior
            16, 17, 18, 16, 18, 19,
            20, 21, 22, 20, 22, 23,

            // Lados de la base inferior
            16, 17, 21, 16, 21, 20,
            17, 18, 22, 17, 22, 21,
            18, 19, 23, 18, 23, 22,
            19, 16, 20, 19, 20, 23
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.DarkOrange);
            GL.Enable(EnableCap.DepthTest);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            UpdateVertexBuffer();

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            _shader = Shader.LoadShader("shader.vert", "shader.frag");
            GL.UseProgram(_shader);

            int vertexLocation = GL.GetAttribLocation(_shader, "aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);
        }

        private float[] GenerateVertices()
        {
            return new float[]
            {
                // Front face
                -0.6f,  0.6f,  0.1f,  // 0
                -0.6f, -0.6f,  0.1f,  // 1
                -0.4f, -0.6f,  0.1f,  // 2
                -0.4f,  0.6f,  0.1f,  // 3
                 0.4f,  0.6f,  0.1f,  // 4
                 0.4f, -0.6f,  0.1f,  // 5
                 0.6f, -0.6f,  0.1f,  // 6
                 0.6f,  0.6f,  0.1f,  // 7

                // Back face
                -0.6f,  0.6f, -0.1f,  // 8
                -0.6f, -0.6f, -0.1f,  // 9
                -0.4f, -0.6f, -0.1f,  // 10
                -0.4f,  0.6f, -0.1f,  // 11
                 0.4f,  0.6f, -0.1f,  // 12
                 0.4f, -0.6f, -0.1f,  // 13
                 0.6f, -0.6f, -0.1f,  // 14
                 0.6f,  0.6f, -0.1f,  // 15

                // Base inferior de la "U" con grosor
                -0.6f, -0.6f,  0.1f,  // 16
                -0.6f, -0.6f, -0.1f,  // 17
                 0.6f, -0.6f, -0.1f,  // 18
                 0.6f, -0.6f,  0.1f,  // 19
                -0.6f, -0.8f,  0.1f,  // 20
                -0.6f, -0.8f, -0.1f,  // 21
                 0.6f, -0.8f, -0.1f,  // 22
                 0.6f, -0.8f,  0.1f   // 23
            };
        }

        private void UpdateVertexBuffer()
        {
            float[] vertices = GenerateVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _rotation += 0.0002f;
            Matrix4 model = Matrix4.CreateTranslation(0f, 0f, 0f) *    //NUEVO EJE CENTRAL para modificar
               Matrix4.CreateRotationY(_rotation);
            Matrix4 view = Matrix4.LookAt(new Vector3(0, 0, 4), Vector3.Zero, Vector3.UnitY);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), 800f / 600f, 0.1f, 100f);
            _mvp = model * view * projection;

            int mvpLocation = GL.GetUniformLocation(_shader, "mvp");
            GL.UseProgram(_shader);
            GL.UniformMatrix4(mvpLocation, false, ref _mvp);

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
        }
    }
}