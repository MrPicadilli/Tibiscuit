using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer;//layer with objects that obstruct the enemy view, like walls, for example
    public int VisionConeResolution = 120;//the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;
    //Create all of these variables, most of them are self explanatory, but for the ones that aren't i've added a comment to clue you in on what they do
    //for the ones that you dont understand dont worry, just follow along
    void Start()
    {
        transform.GetComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.GetComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
        VisionConeMesh.bounds = new Bounds(Vector3.zero, new Vector3(VisionRange, VisionRange, VisionRange));

    }


    void Update()
    {
        DrawVisionCone();//calling the vision cone function everyframe just so the cone is updated every frame
    }

    void DrawVisionCone()//this method creates the vision cone mesh
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    Debug.Log("found it ! ");
            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }



    void DrawVisionCone1()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];

        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Instantiate(sphere,Vector3.zero,Quaternion.identity)
        VisionConeMesh.bounds = new Bounds(Vector3.zero, new Vector3(VisionRange, VisionRange, VisionRange));
        // Set the initial vertex at the local origin
        Vertices[0] = Vector3.zero;

        float CurrentAngle = -VisionAngle / 2;
        float AngleIncrement = VisionAngle / (VisionConeResolution - 1);
        float Sine, Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(CurrentAngle);
            Cosine = Mathf.Cos(CurrentAngle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);

            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);

            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                // Convert hit point to local space
                Vertices[i + 1] = transform.InverseTransformPoint(transform.position + (VertForward * hit.distance)) - transform.localPosition;
            }
            else
            {
                // Use the maximum vision range in local space
                Vertices[i + 1] = transform.InverseTransformPoint(transform.position + (VertForward * VisionRange)) - transform.localPosition   ;
            }

            CurrentAngle += AngleIncrement;
            //Debug.Log(transform.localPosition + " and " +  transform.parent.localPosition);
        }

        // Build triangles
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }

        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;

        // Recalculate bounds to prevent origin shift issues
        //VisionConeMesh.RecalculateBounds();
        //VisionConeMesh.RecalculateNormals();

        MeshFilter_.mesh = VisionConeMesh;
    }


}