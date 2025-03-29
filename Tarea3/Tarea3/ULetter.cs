using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_U
{
    public class ULetter
    {
        private int _vao, _vbo, _ebo;
        private static readonly uint[] Indices = {
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

        private float[] _vertices;

        public Vector3 Position { get; private set; }

        public ULetter(Vector3 position)
        {
            Position = position;
            GenerateVertices();

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            int vertexLocation = 0; // La ubicación del atributo en el shader
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);
        }

        private void GenerateVertices()
        {
            _vertices = new float[]
            {
                // Front face
                -0.6f,  0.6f,  0.1f,  
                -0.6f, -0.6f,  0.1f,  
                -0.4f, -0.6f,  0.1f,  
                -0.4f,  0.6f,  0.1f,  
                 0.4f,  0.6f,  0.1f,  
                 0.4f, -0.6f,  0.1f,  
                 0.6f, -0.6f,  0.1f,  
                 0.6f,  0.6f,  0.1f,  

                // Back face
                -0.6f,  0.6f, -0.1f,  
                -0.6f, -0.6f, -0.1f,  
                -0.4f, -0.6f, -0.1f,  
                -0.4f,  0.6f, -0.1f,  
                 0.4f,  0.6f, -0.1f,  
                 0.4f, -0.6f, -0.1f,  
                 0.6f, -0.6f, -0.1f,  
                 0.6f,  0.6f, -0.1f,  

                // Base inferior con grosor
                -0.6f, -0.6f,  0.1f,  
                -0.6f, -0.6f, -0.1f,  
                 0.6f, -0.6f, -0.1f,  
                 0.6f, -0.6f,  0.1f,  
                -0.6f, -0.8f,  0.1f,  
                -0.6f, -0.8f, -0.1f,  
                 0.6f, -0.8f, -0.1f,  
                 0.6f, -0.8f,  0.1f   
            };

            // Aplicar la posición
            for (int i = 0; i < _vertices.Length; i += 3)
            {
                _vertices[i] += Position.X;
                _vertices[i + 1] += Position.Y;
                _vertices[i + 2] += Position.Z;
            }
        }

        public void Draw(int shader)
        {
            GL.UseProgram(shader);
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
