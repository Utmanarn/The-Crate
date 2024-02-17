using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Box : MonoBehaviour
{
    private Mesh mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh {name = "Box"};
        meshFilter.sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        MeshBuilder builder = new MeshBuilder();

        for (int i = 0; i <= 1; i++)
        {
            // Set the bottom of the image to dirt (and stone).
            builder.TextureMatrix = Matrix4x4.Translate(new Vector3(0.5f * i, 0.0f, 0.0f)) *
                                    Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));
            
            Matrix4x4 mat = Matrix4x4.identity *
                            Matrix4x4.Translate(new Vector3(0, 0, i)) *
                            Matrix4x4.Rotate(Quaternion.AngleAxis(180 * i, Vector3.left));
            builder.VertexMatrix = mat;
            
            int e = builder.AddVertex(
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector2(0, 0));
            int f = builder.AddVertex(
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 0),
                new Vector2(0, 1));
            int g = builder.AddVertex(
                new Vector3(1, 0, 1),
                new Vector3(0, 1, 0),
                new Vector2(1, 1));
            int h = builder.AddVertex(
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector2(1, 0));
            builder.AddQuad(e, f, g, h);
        }
        
        // Set the texture of the lid to grass.
        builder.TextureMatrix = Matrix4x4.Translate(new Vector3(0.5f, 0.5f, 0.0f)) *
                                Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));
        
        float t = Time.time; // In seconds
        t -= Mathf.Floor(t); // 0.0 to 1.0
        t -= 0.5f;           // -0.5 to 0.5
        t *= 2.0f;           // -1.0 to 1.0
        t = Mathf.Abs(t);    // 0.0 to 1.0
        t = Mathf.SmoothStep(0.0f, 1.0f, t);

        for (int i = 0; i <= 1; i++)
        {
            builder.VertexMatrix =
                Matrix4x4.Translate(new Vector3(0, 1, 0)) *
                Matrix4x4.Rotate(Quaternion.AngleAxis(t*-90, Vector3.right)) *
                Matrix4x4.Translate(new Vector3(0, 0, i)) *
                Matrix4x4.Rotate(Quaternion.AngleAxis(i * 180, Vector3.left));
        
            int a = builder.AddVertex(
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector2(0, 0));
            int b = builder.AddVertex(
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 0),
                new Vector2(0, 1));
            int c = builder.AddVertex(
                new Vector3(1, 0, 1),
                new Vector3(0, 1, 0),
                new Vector2(1, 1));
            int d = builder.AddVertex(
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector2(1, 0));
            builder.AddQuad(a, b, c, d);
        }
        
        // Set the texture of the walls to dirt with grass top.
        builder.TextureMatrix = Matrix4x4.Translate(new Vector3(0.0f, 0.5f, 0.0f)) *
                                Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));
        for (int i = 0; i < 4; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                Matrix4x4 mat = Matrix4x4.identity *
                                Matrix4x4.Translate(new Vector3(0.5f, 0, 0.5f)) *
                                Matrix4x4.Rotate(Quaternion.AngleAxis(90*i, Vector3.up)) *
                                Matrix4x4.Translate(new Vector3(-0.5f, 0, j * 0.5f)) *
                                Matrix4x4.Rotate(Quaternion.AngleAxis(-90, Vector3.right));
                builder.VertexMatrix = mat;
                
                int w0 = builder.AddVertex(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector2(0, 0));
                int w1 = builder.AddVertex(new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector2(0, 1));
                int w2 = builder.AddVertex(new Vector3(1, 0, 1), new Vector3(0, 1, 0), new Vector2(1, 1));
                int w3 = builder.AddVertex(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector2(1, 0));
                builder.AddQuad(w0, w1, w2, w3);
            }
        }

        builder.Build(mesh);
    }
}
