namespace AnimacionAuto
{
    public class Ejecutor
    {
        private Libreto libreto;
        private float tiempoActual;

        public Ejecutor(Libreto libreto)
        {
            this.libreto = libreto;
        }

        public void Ejecutar(float deltaTiempo)
        {
            tiempoActual += deltaTiempo;
            foreach (var accion in libreto.Acciones)
            {
                if (tiempoActual >= accion.TiempoInicio && tiempoActual <= accion.TiempoFin)
                {
                    accion.Ejecutar(tiempoActual);
                }
            }
            if (tiempoActual > 6f) 
                tiempoActual = 0f;

        }
    }
}
