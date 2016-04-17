using UnityEngine;
using System.Collections;

namespace ShapeShift.Terrain
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(EdgeCollider2D))]
    public class TerrainRenderer : MonoBehaviour
    {
        public Terrain Terrain;
        public Vector2 Size = new Vector2(50, 2);
        public int Seed = 10;
        public float Lacunarity = 2f;
        public float Persistence = 0.5f;
        public float StepWidth = 0.5f;

        public bool GenerateCollider = true;

        private MeshFilter _meshFilter;
        private EdgeCollider2D _edgeCollider;

        public void OnEnable()
        {
            this.Setup();
            this.Generate();
        }

        public void OnAwake()
        {
            this.Setup();
            this.Generate();
        }

        private void Setup()
        {
            this._meshFilter = this.GetComponent<MeshFilter>();
            this._edgeCollider = this.GetComponent<EdgeCollider2D>();
        }

        private void Generate()
        {
            if (this.Terrain == null)
            {
                this.Terrain = new Terrain(
                    GameController.Seed,
                    (int)Mathf.Ceil(this.Size.x),
                    this.Lacunarity,
                    this.Persistence);
            }

            var mesh = this.GenerateMesh(this.Size.y, this.StepWidth);
            this._meshFilter.sharedMesh = mesh;

            if (this.GenerateCollider)
                this.GenerateColliderMesh(mesh);
        }

        public Mesh GenerateMesh(float height, float stepWidth = 0.1f)
        {
            var points = this.Terrain.GeneratePoints(5);
            var mesh = new Mesh();

            var vertices = new Vector3[points.Length * 2 + 2];
            var triangles = new int[points.Length * 2 * 3];
            var uvs = new Vector2[vertices.Length];

            Debug.Log("Points: " + points.Length);
            Debug.Log("Vertices: " + vertices.Length);
            Debug.Log("Triangles: " + triangles.Length);
            Debug.Log("UVs: " + uvs.Length);

            for (int i = 0; i < points.Length; i++)
            {
                int vi = i * 2 + 2;
                float x = (float)i;
                float y = points[i];

                if (i == 0)
                {
                    vertices[vi - 2] = new Vector3((x - 1) * stepWidth, y, 0f);
                    vertices[vi - 1] = new Vector3(x * stepWidth, y - height, 0f);
                }

                vertices[vi + 0] = new Vector3(x * stepWidth, y, 0f); 
                vertices[vi + 1] = new Vector3((x + 1) * stepWidth, y - height, 0f);

                // 0--2
                //  \ |\
                //   \| \
                //    1--3
                // Order:
                // 1 -> 0 -> 2
                // 1 -> 2 -> 3

            }

            for (int i = 0; i < triangles.Length; i += 6)
            {
                // a--c
                //  \ |\
                //   \| \
                //    b--d
                // a -> c -> b
                // c -> d -> b
                int a, b, c, d;
                if (i > 0)
                    a = i / 3;
                else
                    a = 0;

                b = a + 1;
                c = b + 1;
                d = c + 1;

                triangles[i + 0] = a;
                triangles[i + 1] = c;
                triangles[i + 2] = b;
                triangles[i + 3] = c;
                triangles[i + 4] = d;
                triangles[i + 5] = b;
            }

            for (int i = 0; i < vertices.Length; i++)
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.Optimize();
            return mesh;
        }

        public void GenerateColliderMesh(Mesh m)
        {
            var points = new Vector2[m.vertices.Length / 2 + 2];

            points[0] = new Vector2(0f, 100f);

            for (int i = 0; i < m.vertices.Length; i += 2)
                points[1 + i / 2] = m.vertices[i];

            points[points.Length - 1] = new Vector2(points[points.Length - 2].x, 100f);

            this._edgeCollider.points = points;
        }

        public void OnCollisionEnter2D(Collision2D c)
        {
        }
    }
}