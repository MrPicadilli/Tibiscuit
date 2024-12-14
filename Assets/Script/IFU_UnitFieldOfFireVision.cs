using UnityEngine;

public class IFU_UnitFieldOfFireVision : MonoBehaviour // this is attached to a child GO
{
    [Header("Field of View Generator")]
    public float AngleOfView;
    public int rayCount;
    public float ViewDistance;
    public LayerMask GroundLayerMask;

    public GameObject MainBodyGO;

    public float StartingAngle;
    Mesh mesh;
    Vector3 MainBodyPos;

    // Start is called before the first frame update
    void Start()
    {
        SetOrigin(MainBodyGO.transform.position);



        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }


    // Update is called once per frame
    private void Update()
    {
        SetOrigin(MainBodyGO.transform.position);
        ReturnStartingAngle();
        MeshGenQuarternionCalcs();
    }



    // ========================================================================================================

    public void SetOrigin(Vector3 NewOrigin)
    {
        MainBodyPos = NewOrigin;
    }

    public void ReturnStartingAngle()
    {
        StartingAngle = MainBodyGO.transform.localRotation.eulerAngles.y;
    }

    public void MeshGenQuarternionCalcs()
    {
        float fov = AngleOfView;

        int raycount = rayCount;
        float meshAngle = StartingAngle + fov/2;
        float angleIncrease = fov / raycount;
        float viewDistance = ViewDistance;

        Vector3[] Vertices = new Vector3[raycount + 1 + 1]; // positioning of points
        Vector2[] uv = new Vector2[Vertices.Length]; // texture rendered - vector 2 as the image it references is flat 2d so it uses vector 2 only
        int[] triangles = new int[raycount * 3]; // actual points of the mesh

        Vertices[0] = Vector3.zero; // same as above, mesh origin is at this transform's position

        int vertexIndex = 1; // 0 is the origin
        int triangleIndex = 0;
        for (int i = 0; i <= raycount; i++)
        {

            Vector3 Vertex = Vector3.zero;

            RaycastHit raycastHit;

            if (Physics.Raycast(MainBodyPos, PointCalcWorldSpace(meshAngle), out raycastHit, viewDistance, GroundLayerMask))
            {
                Transform MainBodyTransform = MainBodyGO.transform;
                Vertex = MainBodyTransform.InverseTransformPoint(raycastHit.point);
            }
            else
            {
                Vertex = Vector3.zero + PointCalc(meshAngle);
            }

            Vertices[vertexIndex] = Vertex;


            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            meshAngle -= angleIncrease; // goes counter clockwise if +, - for anti clockwise
        }

        mesh.vertices = Vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    Vector3 PointCalc(float angle)
    {
        Quaternion pointRot = MainBodyGO.transform.rotation * Quaternion.AngleAxis(angle, -Vector3.up);
        Vector3 Point = pointRot * Vector3.forward * ViewDistance;

        return Point;
    }

    Vector3 PointCalcWorldSpace(float angle)
    {
        Quaternion pointRot = MainBodyGO.transform.rotation * Quaternion.AngleAxis(angle, -Vector3.up);
        Vector3 Point = pointRot * Vector3.forward * ViewDistance;
        Vector3 VectorInWorldSpace = transform.TransformDirection(Point);
        return VectorInWorldSpace;
    }
}