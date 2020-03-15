using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public class ECS_SpriteRenderer : MonoBehaviour
{
    [SerializeField]
    Mesh m_Mesh;
    [SerializeField]
    Material m_Mat;
    [SerializeField]
    int m_CreateCount;
    static public Mesh m_sMesh;
    static public Material m_sMat;

    [SerializeField]
    GameObject m_Prefab;
    [SerializeField]
    bool m_Legacy;
    private void Start()
    {
        if (m_Legacy)
        {
            for (int i = 0; i < m_CreateCount; ++i)
            {
                Instantiate(m_Prefab, new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), UnityEngine.Random.Range(-5.0f, 5.0f), UnityEngine.Random.Range(-5.0f, 5.0f)), Quaternion.identity);
            }
            return;
        }

        m_sMat = m_Mat;
        m_sMesh = m_Mesh;

        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Unity.Mathematics.Random random = new Unity.Mathematics.Random(51);


        EntityArchetype archetype = manager.CreateArchetype(typeof(LocalToWorld), typeof(Translation), typeof(Rotation), typeof(NonUniformScale), typeof(TestComponent), typeof(MovementData));

        for (int i = 0; i < m_CreateCount; ++i)
        {
            Entity entiti = manager.CreateEntity(archetype);
            manager.SetComponentData(entiti, new Translation { Value = new float3(random.NextFloat(-5.0f, 5.0f), random.NextFloat(-5.0f, 5.0f), 0.0f) });
            manager.SetComponentData(entiti, new NonUniformScale { Value = new float3(1, 1, 1) });
            manager.SetComponentData(entiti, new TestComponent { Value = 1 });
            manager.SetComponentData(entiti, new MovementData { PerSpeed = 10.0f });
            manager.SetName(entiti, "sprite test");
        }
    }

    //private void Update()
    //{
    //    Graphics.DrawMesh(m_Mesh, Matrix4x4.identity, m_Mat, 0);
    //}

    Mesh CreateQuadMesh(float _Width, float _Height)
    {
        Vector3[] vertices = new Vector3[4];
        int[] indices = new int[6];
        Vector2[] uv = new Vector2[4];

        float halfwidth = _Width * 0.5f;
        float halfhegith = _Height * 0.5f;

        vertices[0] = new Vector3(-halfwidth, -halfhegith);
        vertices[1] = new Vector3(-halfwidth, halfhegith);
        vertices[2] = new Vector3(halfwidth, halfhegith);
        vertices[3] = new Vector3(halfwidth, -halfhegith);

        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 3;

        indices[3] = 1;
        indices[4] = 2;
        indices[5] = 3;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.uv = uv;

        return mesh;
    }
}
