using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI; 



public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 400;
    public int zSize = 400;
    public int NumOfIt = 0;
    public Text numOfIter;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CreateShape());
        
    }
    void Update()
    {
        numOfIter.text = NumOfIt.ToString();
        UpdateMesh();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mesh.Clear();
            StartCoroutine(CreateShape());
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(iterate(100));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(iterate(200));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(iterate(400));
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(Vector3.up * 30 * Time.deltaTime);
        };
        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(Vector3.up * -30 * Time.deltaTime);
        };
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 position = transform.position;
            position.x--;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 position = transform.position;
            position.x++;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 position = transform.position;
            position.z++;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 position = transform.position;
            position.z--;
            transform.position = position;
        }

    }

    IEnumerator iterate(int num)
    {
        for (int j = 0; j < num; j++)
        {
            for (int i = 0; i < 100; i++)
            {
                runDroplet();
            };
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator CreateShape()
    {
        NumOfIt = 0;
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float randomNum1 = Random.Range(0.3f, 2.4f);
        float randomNum2 = Random.Range(0.3f, 2.4f);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x<= xSize; x++)
            {
                float y = (Mathf.PerlinNoise(x * .01f * randomNum2, z * .01f * randomNum1) * 40f) + (Mathf.PerlinNoise(x * .01f* randomNum1, z * .01f * randomNum2) * 60f) + (Mathf.PerlinNoise(x * .06f, z * .1f) * 5f);
        vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6]; 
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++){
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

            };
            vert++;
            yield return new WaitForSeconds(.01f);

        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        for (int i=0; i<vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

    int getMinIndex(int index)
    {
        int xSizeV = xSize + 1;
        float localHeight = vertices[index].y;
        float LTheight = vertices[index - xSizeV - 1].y;
        float CTheight = vertices[index -1].y;
        float RTheight = vertices[index + xSizeV - 1].y;
        float LMheight = vertices[index - xSizeV].y;
        float RMheight = vertices[index + xSizeV].y;
        float LBheight = vertices[index - xSizeV + 1].y;
        float CBheight = vertices[index +1].y;
        float RBheight = vertices[index + xSizeV + 1].y;
        List<float> list = new List<float>();
        list.Add(localHeight);
        list.Add(LTheight);
        list.Add(CTheight);
        list.Add(RTheight);
        list.Add(LMheight);
        list.Add(RMheight);
        list.Add(LBheight);
        list.Add(CBheight);
        list.Add(RBheight);

        float minVal = list.Min();
        int minIndex =  list.IndexOf(minVal);
        
        if (minIndex == 1)
        {
            return index - xSizeV - 1;
        }; 
        if (minIndex == 2)
        {
            return index - 1;
        };
        if (minIndex == 3)
        {
            return index + xSizeV - 1;
        };
        if (minIndex == 4)
        {
            return index - xSizeV;
        };
        if (minIndex == 5)
        {
            return index + xSizeV;
        };
        if (minIndex == 6)
        {
            return index - xSizeV + 1;
        };
        if (minIndex == 7)
        {
            return index + 1;
        };
        if (minIndex == 8)
        {
            return index + xSizeV + 1;
        };

        return index;
    }

    float getSlope(int indexFirst, int indexSecond)
    {
        float horiDist = Mathf.Pow((Mathf.Pow((vertices[indexFirst].x - vertices[indexSecond].x),2f) + Mathf.Pow((vertices[indexFirst].z - vertices[indexSecond].z),2f)),0.5f);
        float vertDist = (vertices[indexFirst].y - vertices[indexSecond].y);
        float angle = Vector2.Angle(new Vector2(100, 0), new Vector2( horiDist, vertDist));
        return angle;
    }

    void runDroplet()
    {
        NumOfIt++;
        int locX = Random.Range(1, xSize);
        int locZ = Random.Range(1, zSize);
        int firstIndex = ((xSize + 1) * (locZ - 1)) + locX;
        int cyclesLeft = 20;
        float capacity = 1f;
        int ownIndex = firstIndex;
        while ( cyclesLeft > 0)
        {
            if (vertices[ownIndex].x > 2 && vertices[ownIndex].x < (xSize -2) && vertices[ownIndex].z > 2 && vertices[ownIndex].z < (zSize -2))
            {
                int minHeightIndex = getMinIndex(ownIndex);
                Vector3 currentVector = vertices[ownIndex];
                Vector3 nextVector = vertices[minHeightIndex];
                if (ownIndex != minHeightIndex)
                {
                    float slope = getSlope(ownIndex, minHeightIndex);
                    float minSlope = capacity*30;
                    float heightDiff = currentVector.y - vertices[minHeightIndex].y;
                    if (slope > minSlope || capacity < 0.3f)
                    {
                        changeAround(ownIndex, -(capacity * slope / 600), capacity);
                        capacity -= (capacity * slope / 600);
                    }
                    else
                    {
                        if (slope < minSlope || capacity > 3f)
                        {
                            float changeValue = 0f;
                            changeValue = (capacity * slope / 600);
                            changeAround(minHeightIndex, changeValue, capacity);
                            capacity += changeValue;
                        }
                    }

                    ownIndex = minHeightIndex;
                }
                else
                {
                    changeAround(ownIndex, (capacity * 0.1f), capacity);
                    capacity += capacity * 0.1f;
                }

            }
            else { cyclesLeft--; 
                continue; }
            cyclesLeft--;
        };
        
    }

    void changeAround(int index, float amount, float capacity)
    {
        float amountSecond = amount * (Mathf.Pow(capacity,2f)/10);
        int xSizeV = xSize + 1;
        Vector3 local = vertices[index];
        Vector3 LT = vertices[index - xSizeV - 1];
        Vector3 CT = vertices[index - 1];
        Vector3 RT = vertices[index + xSizeV - 1];
        Vector3 LM = vertices[index - xSizeV];
        Vector3 RM = vertices[index + xSizeV];
        Vector3 LB = vertices[index - xSizeV + 1];
        Vector3 CB = vertices[index + 1];
        Vector3 RB = vertices[index + xSizeV + 1];

        vertices[index] = new Vector3(local.x, local.y + (amount - amountSecond), local.z);
        vertices[index - xSizeV - 1] = new Vector3(LT.x, LT.y + amountSecond/2, LT.z);
        vertices[index - 1] = new Vector3(CT.x, CT.y + amountSecond, CT.z);
        vertices[index + xSizeV - 1] = new Vector3(RT.x, RT.y + amountSecond/2, RT.z);
        vertices[index - xSizeV] = new Vector3(LM.x, LM.y + amountSecond, LM.z);
        vertices[index + xSizeV] = new Vector3(RM.x, RM.y + amountSecond, RM.z);
        vertices[index - xSizeV + 1] = new Vector3(LB.x, LB.y + amountSecond/2, LB.z);
        vertices[index + 1] = new Vector3(CB.x, CB.y + amountSecond, CB.z);
        vertices[index + xSizeV + 1] = new Vector3(RB.x, RB.y + amountSecond/2, RB.z);
    }
}
