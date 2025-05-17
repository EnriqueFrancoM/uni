using System.Collections.Generic;

namespace AnimacionAuto
{
    public class Libreto
    {
        public List<Accion> Acciones { get; } = new List<Accion>();

        public void AgregarAccion(Accion accion)
        {
            Acciones.Add(accion);
        }
    }
}