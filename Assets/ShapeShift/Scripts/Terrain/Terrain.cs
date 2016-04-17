using UnityEngine;

namespace ShapeShift.Terrain
{
    public class Terrain
    {
        public int Seed;
        public int Width;
        public float Lacunarity;
        public float Persistence;

        private float[] _points;

        public Terrain(int seed, int width, float lacunarity = 2f, float persistence = 0.5f)
        {
            this.Seed = seed;
            this.Width = width;
            this.Lacunarity = lacunarity;
            this.Persistence = persistence;
        }

        public float[] GeneratePoints(int octaves = 1)
        {
            this._points = new float[this.Width];
            for (int i = 0; i < octaves; i++)
            {
                var frequency = Mathf.Pow(this.Lacunarity, i + 1);
                var amplitude = Mathf.Pow(this.Persistence, i + 1);
                UnityEngine.Debug.Log(string.Format("F: {0} - A: {1}", frequency, amplitude));

                for (int x = 0; x < this.Width; x++)
                {
                    var xCoord = (float) (x + this.Seed) / 20f * frequency;
                    this._points[x] += (Mathf.PerlinNoise(xCoord, 0f) - 0.5f) * 2f * amplitude;
                }
            }

            return this._points;
        }

        public void Debug()
        {
            for (int x = 0; x < this._points.Length; x++)
            {
                var o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                o.transform.localPosition = new Vector3(x/3f, this._points[x], 0f);
            }
        }
    }
}
