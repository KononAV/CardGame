using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TableGrid : MonoBehaviour
{
    [SerializeField]CardScript cardScript;
    public Vector2 Size = new Vector2(1.5f,1);

    [SerializeField] private GameObject Table;
    [SerializeField] private CardScript cardExamp;


    private List<Vector3> vectorList = new();


    enum MatrixSide
    {
        LeftUp,
        RightUp,
        LeftDown,
        RightDown,
    }

    private void CardMatrix(MatrixSide side)
    {
        Rect safeArea = Screen.safeArea;
        Vector3 StartPos = new Vector3(safeArea.position.normalized.x, .1f, safeArea.position.normalized.y);

        (int x, int y) vectors = side switch
        {
            MatrixSide.LeftUp => (-1, 1),
            MatrixSide.RightUp => (1, 1),
            MatrixSide.RightDown => (1, -1),
            MatrixSide.LeftDown => (-1, -1),
        };

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                float spacing = 1.5f; 
                Vector3 offsetup = new Vector3(x*vectors.x * spacing, 0, y*vectors.y * spacing * 1.2f);

               

                Instantiate(cardExamp, StartPos + offsetup, cardExamp.transform.rotation);
            }
        }
    }



    public int[][] GetMatrix(int sizeX, int sizeY)
    {

        Rect safeArea = Screen.safeArea;
        Vector3 StartPos = new Vector3(safeArea.position.normalized.x, .1f, safeArea.position.normalized.y);

        float spacing = 1.5f;

        int[][] matrix = new int[sizeY][];
        for (int i = 0; i < sizeY; i++)
        {
            matrix[i] = new int[sizeX];
        }

        int total = sizeX * sizeY;
        int count = 1;


        int x = sizeX / 2;
        int y = sizeY / 2;

        if (sizeX % 2 == 0) x--;
        if (sizeY % 2 == 0) y--;

        matrix[y][x] = count++;


        int[][] directions = new int[][]
        {
            new int[]{1, 0},
            new int[]{0, 1},
            new int[]{-1, 0},
            new int[]{0, -1}
        };

        int steps = 1;
        while (count <= total)
        {
            for (int d = 0; d < 4; d++)
            {
                for (int s = 0; s < steps; s++)
                {
                    x += directions[d][0];
                    y += directions[d][1];

                    if (x >= 0 && x < sizeX && y >= 0 && y < sizeY)
                    {
                        matrix[y][x] = count++;
                        if (count > total) break;
                    }
                }
                if (d % 2 == 1) steps++;
            }
        }

        return matrix;
    }


        private void OnDrawGizmos()
        {
        int sizeY = 4;
        int sizeX = 5;
        float centerDel = 0;
float spacing = 1.5f;
        Rect safeArea = Screen.safeArea;
        Vector3 StartPos = new Vector3(safeArea.position.normalized.x, .1f, safeArea.position.normalized.y);

        if(sizeX%2==0)centerDel = .5f;
        //else centerDel = 1/spacing;

        Vector3 newStartPos = new Vector3(
            safeArea.position.normalized.x-spacing*((float)Math.Round((double)sizeX/2-1) )- spacing* centerDel, 
            .1f, 
            safeArea.position.normalized.y - (spacing *1.2f)* (float)Math.Round((double)sizeY / 2-1)-(spacing*1.2f/ 2) );

        //Debug.Log(StartPos);



        int[][] matrix = new int[sizeY][];
        for (int i = 0; i < sizeY; i++)
        {
            matrix[i] = new int[sizeX];
        }

        int total = sizeX * sizeY;
        int count = 1;

        int x = sizeX / 2;
        int y = sizeY / 2;

        if (sizeX % 2 == 0) x--;
        if (sizeY % 2 == 0) y--;

        matrix[y][x] = count++;


        int[][] directions = new int[][]
        {
            new int[]{1, 0},
            new int[]{0, 1},
            new int[]{-1, 0},
            new int[]{0, -1}
        };

        int steps = 1;
        while (count <= total)
        {
            for (int d = 0; d < 4; d++)
            {
                for (int s = 0; s < steps; s++)
                {
                    x += directions[d][0];
                    y += directions[d][1];

                    if (x >= 0 && x < sizeX && y >= 0 && y < sizeY)
                    {
                        matrix[y][x] = count++;
                        if((x+y)%2==0)Gizmos.color = Color.red;
                        else Gizmos.color = Color.green;
                        Gizmos.DrawCube(newStartPos + new Vector3(x * spacing, 0, y * spacing * 1.2f), new Vector3(1, 0.1f, 1.5f));
                        //Instantiate(cardExamp, newStartPos + new Vector3(x * spacing, 0, y * spacing * 1.2f), cardExamp.transform.rotation);
                        if (count > total) break;
                    }
                }
                if (d % 2 == 1) steps++;
            }
        }

    

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(StartPos, new Vector3(1, .1f, 1.5f));
        /*
            Rect safeArea = Screen.safeArea;
            Vector3 StartPos = new Vector3(safeArea.position.normalized.x, .1f, safeArea.position.normalized.y);

            Gizmos.color = Color.blue;
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 2; y++)
                {


                    float spacing = 1.5f; // 1 (ширина) + 1 (зазор)
                    Vector3 offsetup = new Vector3(x * spacing, 0, y * spacing*1.5f);
                    Vector3 offsetdown = new Vector3(-x * spacing, 0, y * spacing * 1.5f);

                    Gizmos.DrawCube(StartPos + offsetup, new Vector3(1, 0.1f, 1.5f));

                    Gizmos.DrawCube(StartPos + offsetdown, new Vector3(1, 0.1f, 1.5f));


                   // Gizmos.DrawCube(StartPos - offset, new Vector3(1, 0.1f, 1.5f));

                }
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 3; y++)
                {

                    float spacing = 1.5f; // 1 (ширина) + 1 (зазор)
                    Vector3 offsetup = new Vector3(-x * spacing, 0, -y * spacing * 1.5f);
                    Vector3 offsetdown = new Vector3(x * spacing, 0, -y * spacing * 1.5f);

                    Gizmos.DrawCube(StartPos + offsetup, new Vector3(1, 0.1f, 1.5f));

                    Gizmos.DrawCube(StartPos + offsetdown, new Vector3(1, 0.1f, 1.5f));


                    // Gizmos.DrawCube(StartPos - offset, new Vector3(1, 0.1f, 1.5f));

                }
            }


            Gizmos.color = new Color(0.88f, 0f, 0.3f);
            Gizmos.DrawCube(StartPos, new Vector3(1, .1f, 1.5f));*/
    }
    void Start()
    {
        CardMatrix(MatrixSide.LeftDown);
    }

    
}
