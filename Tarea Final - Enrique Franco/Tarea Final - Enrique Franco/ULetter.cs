using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_U
{
    public class ULetter
    {
        public List<Pilar> Pilares { get; private set; } = new List<Pilar>();       // Lista de pilares que forman la letra U
        public Vector3 Position { get; set; }           // Posición de la letra U en el espacio 3D

        public ULetter(Vector3 position, float[] vertices, uint[] indices)
        {
            Position = position;

            // Divide los índices en 3 grupos (izquierda, derecha, base)
            var indicesIzquierda = new List<uint>();
            var indicesDerecha = new List<uint>();
            var indicesBase = new List<uint>();

            for (int i = 0; i < indices.Length; i += 3)     // Recorre los índices para dividir los triángulos
            {
                var v0 = vertices[(indices[i] * 3)];
                var v1 = vertices[(indices[i + 1] * 3)];
                var v2 = vertices[(indices[i + 2] * 3)];

                float avgX = (v0 + v1 + v2) / 3f;           // Calcula el promedio en el eje X de los tres vértices del triángulo

                if (avgX < -0.2f)   // Si el promedio está a la izquierda, se agrega a la parte izquierda
                    indicesIzquierda.AddRange(new[] { indices[i], indices[i + 1], indices[i + 2] });
                else if (avgX > 0.2f)   // Si está a la derecha, se agrega a la parte derecha
                    indicesDerecha.AddRange(new[] { indices[i], indices[i + 1], indices[i + 2] });
                else                    // Si está en el centro, se agrega a la base
                    indicesBase.AddRange(new[] { indices[i], indices[i + 1], indices[i + 2] });
            }

            Pilares.Add(new Pilar(vertices, indicesIzquierda.ToArray()));        // Crea el pilar izquierdo
            Pilares.Add(new Pilar(vertices, indicesDerecha.ToArray()));         // el derecho
            Pilares.Add(new Pilar(vertices, indicesBase.ToArray()));            // el pilar de abajo
        }
    }

    public class Pilar
    {
        private readonly int _vao;              // ID del Vertex Array Object
        private readonly int _vbo;              // ID del Vertex Buffer Object
        private readonly int _ebo;              // ID del Element Buffer Object
        private readonly int _indexCount;       // Número de índices para el pilar

        public Vector3 Position { get; set; } = Vector3.Zero;       // Posición del pilar
        public Vector3 Rotation { get; set; } = Vector3.Zero;       // Rotación del pilar
        public Vector3 Scale { get; set; } = Vector3.One;           // Escala del pilar

        public Pilar(float[] vertices, uint[] indices)              // Guarda el número de índices
        {
            _indexCount = indices.Length;

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}