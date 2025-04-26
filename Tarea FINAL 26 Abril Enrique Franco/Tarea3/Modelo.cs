public class Modelo
{
    public float[] vertices { get; set; } = Array.Empty<float>();       // Array que contiene las coordenadas de los vértices (X, Y, Z)
    public int[] indices { get; set; } = Array.Empty<int>();        // Array que contiene los índices que definen las caras del modelo (referencia a los vértices)
}