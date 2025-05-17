using OpenTK.Graphics.OpenGL4;
using System;

namespace AnimacionAuto
{
    public class Shader
    {
        public int Handle;

        public Shader(string vertPath, string fragPath)
        {
            string vertexShaderSource = File.ReadAllText(vertPath);
            string fragmentShaderSource = File.ReadAllText(fragPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Usar()
        {
            GL.UseProgram(Handle);
        }
    }
}