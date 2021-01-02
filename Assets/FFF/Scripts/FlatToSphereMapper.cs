using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatToSphereMapper : MonoBehaviour
{
    public GameObject target;
    public GameObject source;
    public float radius = 100.0f;

    void Start()
    {
        foreach (var mesh in source.GetComponentsInChildren<MeshFilter>())
        {
            GameObject copy = GameObject.Instantiate(mesh.gameObject, target.transform);
            MeshFilter copyMeshFilter = copy.GetComponent<MeshFilter>();
            Mesh sphereMappedMesh = new Mesh();
            sphereMappedMesh.name = mesh.name+"_spherical";
            var vertices = new Vector3[mesh.mesh.vertices.Length];
            for (int i = 0; i < mesh.mesh.vertices.Length; i++)
                vertices[i] = MapToSphere(mesh.mesh.vertices[i]);
            sphereMappedMesh.vertices = vertices;
            sphereMappedMesh.triangles = mesh.mesh.triangles;
            //sphereMappedMesh.uv = mesh.mesh.uv;
            sphereMappedMesh.RecalculateBounds();
            sphereMappedMesh.RecalculateNormals();
            copyMeshFilter.mesh = sphereMappedMesh;
        }
    }

    private Vector3 MapToSphere(Vector3 source)
    {
        //source.x = Mathf.Sin(source.x);
        source.y = Mathf.Sin(source.x/radius) * radius;
        return source;
    }
}
