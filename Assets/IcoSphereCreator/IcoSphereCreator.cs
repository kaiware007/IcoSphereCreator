using UnityEngine;
using System.Collections;

public static class IcoSphereCreator {


    public static Mesh Create(int n, float radius)
    {
        int nn = n * 4;
        int vertexNum = (nn * nn / 16) * 24;
        Vector3[] vertices = new Vector3[vertexNum];
        int[] triangles = new int[vertexNum];
        Vector2[] uv = new Vector2[vertexNum];

        Quaternion[] init_vectors = new Quaternion[24];
        // 0
        init_vectors[0] = new Quaternion(0, 1, 0, 0);   //the triangle vertical to (1,1,1)
        init_vectors[1] = new Quaternion(0, 0, 1, 0);
        init_vectors[2] = new Quaternion(1, 0, 0, 0);
        // 1
        init_vectors[3] = new Quaternion(0, -1, 0, 0);  //to (1,-1,1)
        init_vectors[4] = new Quaternion(1, 0, 0, 0);
        init_vectors[5] = new Quaternion(0, 0, 1, 0);
        // 2
        init_vectors[6] = new Quaternion(0, 1, 0, 0);   //to (-1,1,1)
        init_vectors[7] = new Quaternion(-1, 0, 0, 0);
        init_vectors[8] = new Quaternion(0, 0, 1, 0);
        // 3
        init_vectors[9] = new Quaternion(0, -1, 0, 0);  //to (-1,-1,1)
        init_vectors[10] = new Quaternion(0, 0, 1, 0);
        init_vectors[11] = new Quaternion(-1, 0, 0, 0);
        // 4
        init_vectors[12] = new Quaternion(0, 1, 0, 0);  //to (1,1,-1)
        init_vectors[13] = new Quaternion(1, 0, 0, 0);
        init_vectors[14] = new Quaternion(0, 0, -1, 0);
        // 5
        init_vectors[15] = new Quaternion(0, 1, 0, 0); //to (-1,1,-1)
        init_vectors[16] = new Quaternion(0, 0, -1, 0);
        init_vectors[17] = new Quaternion(-1, 0, 0, 0);
        // 6
        init_vectors[18] = new Quaternion(0, -1, 0, 0); //to (-1,-1,-1)
        init_vectors[19] = new Quaternion(-1, 0, 0, 0);
        init_vectors[20] = new Quaternion(0, 0, -1, 0);
        // 7
        init_vectors[21] = new Quaternion(0, -1, 0, 0);  //to (1,-1,-1)
        init_vectors[22] = new Quaternion(0, 0, -1, 0);
        init_vectors[23] = new Quaternion(1, 0, 0, 0);
        
        int j = 0;  //index on vectors[]

        for (int i = 0; i < 24; i += 3)
        {
            /*
			 *                   c _________d
			 *    ^ /\           /\        /
			 *   / /  \         /  \      /
			 *  p /    \       /    \    /
			 *   /      \     /      \  /
			 *  /________\   /________\/
			 *     q->       a         b
			 */
            for (int p = 0; p < n; p++)
            {   
                //edge index 1
                Quaternion edge_p1 = Quaternion.Lerp(init_vectors[i], init_vectors[i + 2], (float)p / n);
                Quaternion edge_p2 = Quaternion.Lerp(init_vectors[i + 1], init_vectors[i + 2], (float)p / n);
                Quaternion edge_p3 = Quaternion.Lerp(init_vectors[i], init_vectors[i + 2], (float)(p + 1) / n);
                Quaternion edge_p4 = Quaternion.Lerp(init_vectors[i + 1], init_vectors[i + 2], (float)(p + 1) / n);

                for (int q = 0; q < (n - p); q++)
                {   
                    //edge index 2
                    Quaternion a = Quaternion.Lerp(edge_p1, edge_p2, (float)q / (n - p));
                    Quaternion b = Quaternion.Lerp(edge_p1, edge_p2, (float)(q + 1) / (n - p));
                    Quaternion c, d;
                    if(edge_p3 == edge_p4)
                    {
                        c = edge_p3;
                        d = edge_p3;
                    }else
                    {
                        c = Quaternion.Lerp(edge_p3, edge_p4, (float)q / (n - p - 1));
                        d = Quaternion.Lerp(edge_p3, edge_p4, (float)(q + 1) / (n - p - 1));
                    }

                    triangles[j] = j;
                    vertices[j++] = new Vector3(a.x, a.y, a.z);
                    triangles[j] = j;
                    vertices[j++] = new Vector3(b.x, b.y, b.z);
                    triangles[j] = j;
                    vertices[j++] = new Vector3(c.x, c.y, c.z);
                    if (q < n - p - 1)
                    {
                        triangles[j] = j;
                        vertices[j++] = new Vector3(c.x, c.y, c.z);
                        triangles[j] = j;
                        vertices[j++] = new Vector3(b.x, b.y, b.z);
                        triangles[j] = j;
                        vertices[j++] = new Vector3(d.x, d.y, d.z);
                    }
                }
            }
        }
        Mesh mesh = new Mesh();
        mesh.name = "IcoSphere";

        CreateUV(n, vertices, uv);
        for (int i = 0; i < vertexNum; i++)
        {
            vertices[i] *= radius;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        return mesh;
    }

    static void CreateUV(int n, Vector3[] vertices, Vector2[] uv)
    {
        int tri = n * n;        // devided triangle count (1,4,9...)
        int uvLimit = tri * 6;  // range of wrap UV.x 
        Debug.Log("tri " + tri + " uvLimit " + uvLimit);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];

            Vector2 textureCoordinates;
            if((v.x == 0f)&&(i < uvLimit))
            {
                textureCoordinates.x = 1f;
            }
            else
            {
                textureCoordinates.x = Mathf.Atan2(v.x, v.z) / (-2f * Mathf.PI);
            }

            if (textureCoordinates.x < 0f)
            {
                textureCoordinates.x += 1f;
            }

            textureCoordinates.y = Mathf.Asin(v.y) / Mathf.PI + 0.5f;
            uv[i] = textureCoordinates;
        }

        int tt = tri * 3;
        uv[0 * tt + 0].x = 0.875f;
        uv[1 * tt + 0].x = 0.875f;
        uv[2 * tt + 0].x = 0.125f;
        uv[3 * tt + 0].x = 0.125f;
        uv[4 * tt + 0].x = 0.625f;
        uv[5 * tt + 0].x = 0.375f;
        uv[6 * tt + 0].x = 0.375f;
        uv[7 * tt + 0].x = 0.625f;

    }
}
