using System;
using Unity.Collections;
using UnityEngine;

public class MeshInstancedArgs : IDisposable
{
    public ComputeBuffer m_Buffer { get; }
    NativeArray<uint> m_Args;

    public MeshInstancedArgs()
    {
        m_Buffer = new ComputeBuffer(1, sizeof(uint) * 5, ComputeBufferType.IndirectArguments);
        m_Args = new NativeArray<uint>(5, Allocator.Persistent);
    }

    public void SetData(Mesh mesh, uint instanceCount, int subMeshIndex = 0)
    {
        m_Args[0] = mesh.GetIndexCount(subMeshIndex);
        m_Args[1] = instanceCount;
        m_Args[2] = mesh.GetIndexStart(subMeshIndex);
        m_Args[3] = mesh.GetBaseVertex(subMeshIndex);

        m_Buffer.SetData(m_Args);
    }

    public void Dispose()
    {
        m_Args.Dispose();
        m_Buffer?.Dispose();
    }
}