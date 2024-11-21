using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PerspectivePlane : MonoBehaviour
{
    void Start()
    {
        // Mesh 가져오기
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // 원근감을 줄 수 있도록 Z축 기준으로 변형
        for (int i = 0; i < vertices.Length; i++)
        {
            float distanceFactor = Mathf.Abs(vertices[i].z); // Z축에 따라 변형
            vertices[i].x *= 2.0f + distanceFactor * 0.1f; // X축 확장
        }

        // 변경된 Vertex 적용
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}