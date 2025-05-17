using OpenTK.Mathematics;

namespace AnimacionAuto
{
    public abstract class Objeto3D
    {
        public Vector3 Posicion { get; set; } = Vector3.Zero;
        public Vector3 Rotacion { get; set; } = Vector3.Zero;
        public Vector3 Escala { get; set; } = Vector3.One;


        public abstract void Dibujar();
        public virtual void Mover() { Posicion += new Vector3(0.01f, 0, 0); }
        public virtual void Rotar() { }
        public virtual void Escalar() { }
    }
}