using System.Collections.Generic;
using OpenTK.Mathematics;

namespace AnimacionAuto
{
    public class Escena
    {
        private Libreto libreto;
        private Ejecutor ejecutor;
        private Auto auto;

        public void CargarContenido()
        {
            auto = new Auto();

            libreto = new Libreto();
            libreto.AgregarAccion(new Accion(auto, TipoAccion.Mover, 0f, 1f, new Vector3(0, 0, 0), new Vector3(5, 0, 0)));
            libreto.AgregarAccion(new Accion(auto, TipoAccion.Rotar, 1f, 2f, new Vector3(0, 0, 0), new Vector3(0, 90, 0)));
            libreto.AgregarAccion(new Accion(auto, TipoAccion.Escalar, 2f, 3f, new Vector3(1, 1, 1), new Vector3(2, 2, 2)));

            libreto.AgregarAccion(new Accion(auto, TipoAccion.Mover, 3f, 4f, new Vector3(5, 0, 0), new Vector3(10, 0, 0)));
            libreto.AgregarAccion(new Accion(auto, TipoAccion.Rotar, 4f, 5f, new Vector3(0, 90, 0), new Vector3(0, 180, 0)));
            libreto.AgregarAccion(new Accion(auto, TipoAccion.Escalar, 5f, 6f, new Vector3(2, 2, 2), new Vector3(1, 1, 1)));


            ejecutor = new Ejecutor(libreto);
        }

        public void Dibujar()
        {
            auto.Dibujar();
        }

        public void Actualizar(float tiempo)
        {
            ejecutor.Ejecutar(tiempo);
        }
    }
}
