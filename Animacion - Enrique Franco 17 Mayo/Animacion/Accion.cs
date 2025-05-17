using OpenTK.Mathematics; // NECESARIO para Vector3

namespace AnimacionAuto
{
    public enum TipoAccion { Mover, Rotar, Escalar }

    public class Accion
    {
        public Objeto3D Objeto { get; }
        public TipoAccion Tipo { get; }
        public float TiempoInicio { get; }
        public float TiempoFin { get; }

        public Vector3 Inicio { get; }
        public Vector3 Fin { get; }

        public Accion(Objeto3D objeto, TipoAccion tipo, float inicio, float fin, Vector3 posInicio, Vector3 posFin)
        {
            Objeto = objeto;
            Tipo = tipo;
            TiempoInicio = inicio;
            TiempoFin = fin;
            Inicio = posInicio;
            Fin = posFin;
        }

        public void Ejecutar(float tiempoActual)
        {
            float t = MathHelper.Clamp((tiempoActual - TiempoInicio) / (TiempoFin - TiempoInicio), 0f, 1f);

            switch (Tipo)
            {
                case TipoAccion.Mover:
                    Objeto.Posicion = Vector3.Lerp(Inicio, Fin, t);
                    break;
                case TipoAccion.Rotar:
                    Objeto.Rotacion = Vector3.Lerp(Inicio, Fin, t);
                    break;
                case TipoAccion.Escalar:
                    Objeto.Escala = Vector3.Lerp(Inicio, Fin, t);
                    break;
            }
        }
    }
}
