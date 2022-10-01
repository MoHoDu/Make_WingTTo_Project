using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{
    public GameObject createPrefab;
    MeshFilter meshFilter;

    public List<Vector3> vertices = new List<Vector3>();
    int[] triangels;

    Ray ray;
    GameObject quad;
    MeshCollider quadMeshcollider;
    Vector3 worldPos;

    void Awake()
    {
        triangels = new int[] { 0, 1, 2,
                                0, 2, 3 };
    }

    private void OnMouseUp()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
        RaycastHit hit;

        quad = GameObject.Find("Wall");
        quadMeshcollider = quad.GetComponent<MeshCollider>();

        // if (quadMeshcollider.Raycast(ray, out hit, 10000))
        // {
        //     worldPos = hit.point;
        //     vertices.Add(hit.point);
        // }
        int layerMask = 1 << LayerMask.NameToLayer("wall");
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            worldPos = hit.point;
            vertices.Add(hit.point);
        }

        // GameObject point = Instantiate(new GameObject("point"));
        // point.transform.position = worldPos;
    }

    void CreateObj()
    {
        GameObject createdObj = Instantiate(createPrefab);
        meshFilter = createdObj.GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangels;
        meshFilter.mesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        createdObj.GetComponent<MeshRenderer>().material = material;

        vertices.Clear();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseUp();
        }

        if (vertices.Count == 4)
        {
            CreateObj();
        }
    }
}
