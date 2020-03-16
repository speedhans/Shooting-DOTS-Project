using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

[UpdateAfter(typeof(TrailCalculateSystem))]
public static class TrailMeshGenerator
{
    static float MeshHeightScale = 0.5f;
    static Vector3 CalcPosition(int i, int j) => new Vector3(i, Mathf.Sin(j * Mathf.PI * 2 / 8.0f) * MeshHeightScale, Mathf.Cos(j * Mathf.PI * 2 / 8.0f) * MeshHeightScale);

    public static Mesh CreateMesh()
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        for (var i = 0; i < 20 - 1; ++i)
        {
            for (var j = 0; j < 8; ++j)
            {
                triangles.AddRange(new[] {
                    vertices.Count + 0, vertices.Count + 1, vertices.Count + 2,vertices.Count + 2, vertices.Count + 1, vertices.Count + 3,
                });
                vertices.AddRange(
                    new[]  {
                        CalcPosition(i, j),  CalcPosition(i+1, j),CalcPosition(i, j+1), CalcPosition(i+1, j+1),
                    }
                );
            }
        }

        var mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        Vector2[] uv = new Vector2[vertices.Count];

        for (int i = 0; i < vertices.Count; ++i)
        {
            uv[i] = new Vector2(vertices[i].x / mesh.bounds.size.x, vertices[i].z / mesh.bounds.size.z);
        }
        mesh.SetUVs(0, uv);
        return mesh;
    }
}

//[UpdateAfter(typeof(TrailCalculateSystem))]
public class TrailRendererSystem : ComponentSystem
{
    public static Material m_Material;

    MeshInstancedArgs m_MeshInstancedArgs;
    Mesh m_TrailMesh;
    MaterialPropertyBlock m_MaterialPropertyBlock;

    ComputeBuffer m_TrailElementBufferInShader;
    ComputeBuffer m_SegmentBufferInShader;

    NativeArray<float3> m_TrailElements;
    NativeArray<int3> m_Segments;

    EntityQuery m_TrailBuffer;

    protected override void OnCreate()
    {
        m_MeshInstancedArgs = new MeshInstancedArgs();
        m_TrailMesh = TrailMeshGenerator.CreateMesh();
        m_MaterialPropertyBlock = new MaterialPropertyBlock();
    }

    protected override void OnDestroy()
    {
        m_MeshInstancedArgs.Dispose();
        if (m_TrailElementBufferInShader != null)
            m_TrailElementBufferInShader.Dispose();
        if (m_SegmentBufferInShader != null)
            m_SegmentBufferInShader.Dispose();

        if (m_TrailElements.IsCreated) m_TrailElements.Dispose();
        if (m_Segments.IsCreated) m_Segments.Dispose();
    }

    static int CalcWrappingArraySize(int length) =>
    Mathf.Max(1 << Mathf.CeilToInt(Mathf.Log(length, 2)), 1048);

    protected override unsafe void OnUpdate()
    {
        EntityQuery query = GetEntityQuery(ComponentType.ReadWrite<TrailBufferElement>());
        NativeArray<Entity> entitis = query.ToEntityArray(Allocator.Persistent);
        int segmentCount = entitis.Length;
        int trailElementCount = 0;
        BufferFromEntity<TrailBufferElement> buffers = GetBufferFromEntity<TrailBufferElement>();
        for (int i = 0; i < entitis.Length; ++i)
        {
            trailElementCount += buffers[entitis[i]].Length;
        }

        if (!m_TrailElements.IsCreated || m_TrailElements.Length < trailElementCount)
        {
            if (m_TrailElements.IsCreated) m_TrailElements.Dispose();
            m_TrailElements = new NativeArray<float3>(CalcWrappingArraySize(trailElementCount), Allocator.Persistent);

            m_TrailElementBufferInShader?.Dispose();
            m_TrailElementBufferInShader = new ComputeBuffer(m_TrailElements.Length, sizeof(TrailBufferElement));
        }

        if (!m_Segments.IsCreated || m_Segments.Length < segmentCount)
        {
            if (m_Segments.IsCreated) m_Segments.Dispose();
            m_Segments = new NativeArray<int3>(CalcWrappingArraySize(segmentCount), Allocator.Persistent);

            m_SegmentBufferInShader?.Dispose();
            m_SegmentBufferInShader = new ComputeBuffer(m_Segments.Length, sizeof(TrailBufferElement));
        }

        int offset = 0;
        float3* trailElementsPtr = (float3*)m_TrailElements.GetUnsafePtr();
        int3* segmentsPtr = (int3*)m_Segments.GetUnsafePtr();

        for (int i = 0; i < segmentCount; ++i)
        {
            DynamicBuffer<float3> trailbuffer = buffers[entitis[i]].Reinterpret<float3>();
            Entity entity = entitis[i];

            int bufferlength = trailbuffer.Length;

            UnsafeUtility.MemCpy(trailElementsPtr, trailbuffer.GetUnsafePtr(), sizeof(float3) * bufferlength);
            *segmentsPtr = new int3(offset, bufferlength, entity.Index);

            offset += bufferlength;

            segmentsPtr++;
            trailElementsPtr += bufferlength;

            for (int j = 0; j < trailbuffer.Length; ++j)
            {
                if (trailbuffer[j].x == 0.0f && trailbuffer[j].y == 0.0f && trailbuffer[j].z == 0.0f)
                {
                    Debug.Log(entity.Index);
                }
            }
        }

        m_TrailElementBufferInShader.SetData(m_TrailElements);
        m_SegmentBufferInShader.SetData(m_Segments);

        m_MaterialPropertyBlock.SetBuffer("_Positions", m_TrailElementBufferInShader);
        m_MaterialPropertyBlock.SetBuffer("_Segments", m_SegmentBufferInShader);

        m_MeshInstancedArgs.SetData(m_TrailMesh, (uint)segmentCount);

        Graphics.DrawMeshInstancedIndirect(
            m_TrailMesh, 0, m_Material,
            new Bounds(Vector3.zero, Vector3.one * 1000),
            m_MeshInstancedArgs.m_Buffer, 0, m_MaterialPropertyBlock
        );

        entitis.Dispose();
    }
}
