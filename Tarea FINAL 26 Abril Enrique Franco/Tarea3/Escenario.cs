using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OpenTK_U
{
    public class Escenario
    {
        public float[] Vertices { get; private set; } = Array.Empty<float>();   // Vértices del modelo 3D
        public uint[] Indices { get; private set; } = Array.Empty<uint>();      // Índices que definen las caras del modelo 3D
        public Vector3 CentroDeMasa { get; private set; }       // El centro de masa del modelo
        public List<Vector3> Instancias { get; private set; } = new();      // Lista de posiciones de las instancias del modelo en el escenario

        public Escenario(string path)
        {
            string json = File.ReadAllText(path);                    // Lee el archivo JSON que contiene la información del modelo
            var modelo = JsonSerializer.Deserialize<Modelo>(json);      // Deserializa el contenido JSON en un objeto Modelo

            Vertices = modelo?.vertices ?? Array.Empty<float>();        // Asigna los vértices del modelo, si existen
            Indices = (modelo?.indices ?? Array.Empty<int>()).Select(i => (uint)i).ToArray();   // Asigna los índices del modelo, convirtiendo a uint

            CalcularCentroDeMasa(Vertices);     // Calcula el centro de masa del modelo usando los vértices
        }

        private void CalcularCentroDeMasa(float[] vertices)
        {
            if (vertices == null || vertices.Length == 0)       // Verifica si los vértices están vacíos
            {
                CentroDeMasa = Vector3.Zero;            // Si no hay vértices, el centro de masa es el origen
                return;
            }

            Vector3 suma = Vector3.Zero;
            int total = vertices.Length / 3;            // Cada vértice tiene 3 componentes (X, Y, Z)

            for (int i = 0; i < vertices.Length; i += 3)    // Suma todas las posiciones de los vértices
            {
                suma += new Vector3(vertices[i], vertices[i + 1], vertices[i + 2]);     // Acumula las coordenadas (X, Y, Z) de cada vértice
            }

            CentroDeMasa = suma / total;         // Calcula el centro de masa promediando las coordenadas
        }

        public void AgregarInstancia(Vector3 posicion)      // Añade una nueva instancia del modelo en la posición especificada
        {
            Instancias.Add(posicion);
        }

        private class Modelo
        {
            public float[]? vertices { get; set; }      // Propiedad para los vértices del modelo
            public int[]? indices { get; set; }         // Propiedad para los índices del modelo
        }
    }
}