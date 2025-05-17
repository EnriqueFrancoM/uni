using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace AnimacionAuto
{
    public class Auto : Objeto3D
    {
        private float[] vertices =
        {
            // X, Y, Z
            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
        };

        private uint[] indices =
        {
            0, 1, 2, 2, 3, 0,
            1, 5, 6, 6, 2, 1,
            5, 4, 7, 7, 6, 5,
            4, 0, 3, 3, 7, 4,
            3, 2, 6, 6, 7, 3,
            4, 5, 1, 1, 0, 4
        };

        private int vao, vbo, ebo;
        private Shader shader;

        public Auto()
        {
            // Cargar modelo simple (un cubo)
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            shader = new Shader("shader.vert", "shader.frag");
        }

        public override void Dibujar()
        {
            shader.Usar();

            Matrix4 modelo =
                            Matrix4.CreateScale(Escala) *
                            Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
                            Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
                            Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
                            Matrix4.CreateTranslation(Posicion);

            Matrix4 vista = Matrix4.LookAt(new Vector3(15, 6, 15), new Vector3(5, 0, 0), Vector3.UnitY);  // CAMARA

            Matrix4 proyeccion = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), 800f / 600f, 0.1f, 100f);

            int modelLoc = GL.GetUniformLocation(shader.Handle, "model");
            int viewLoc = GL.GetUniformLocation(shader.Handle, "view");
            int projLoc = GL.GetUniformLocation(shader.Handle, "projection");

            GL.UniformMatrix4(modelLoc, false, ref modelo);
            GL.UniformMatrix4(viewLoc, false, ref vista);
            GL.UniformMatrix4(projLoc, false, ref proyeccion);

            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
