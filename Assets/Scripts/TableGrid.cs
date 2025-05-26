using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TableGrid : MonoBehaviour
{
    [SerializeField] static CardScript cardScript;

    private static Rect safeArea;


    private static Vector3[] vectorArray;

    public static Vector3[] SpiralMatrixCards(int sizeX, int sizeY)
    {
        vectorArray = new Vector3[sizeX * sizeY];
        //vectorArray = new Vector3[sizeX * sizeY];
        float spacing = 1.5f; 
        float verticalScale = 1.2f;

        Vector3 gridCenter = new Vector3(
            safeArea.position.normalized.x,
            0.1f,
            safeArea.position.normalized.y
        );

        Vector3 newStartPos = gridCenter - new Vector3(
            ((sizeX - 1) * 0.5f) * spacing,
            0f,
            ((sizeY - 1) * 0.5f) * spacing * verticalScale
        );
       
        int total = sizeX * sizeY;


        int count = 1;

        int x = sizeX / 2;
        int y = sizeY / 2;

        

        if (sizeX % 2 == 0) x--;
        if (sizeY % 2 == 0) y--;
        
        vectorArray[y * sizeX + x] = newStartPos + new Vector3(x * spacing, 0, y * spacing * verticalScale);

        int[][] directions = new int[][]
        {
     new int[]{1, 0},
     new int[]{0, 1},
     new int[]{-1, 0},
     new int[]{0, -1}
        };

       count++;
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
                        count++;
                        vectorArray[y*sizeX+x]= newStartPos + new Vector3(x * spacing, 0, y * spacing * verticalScale);


                        if (count > total) break;
                    }
                }
                if (d % 2 == 1) steps++;
            }
        }


        return vectorArray;

    }



   /* private void OnDrawGizmos()
    {
        int sizeY = 4;
        int sizeX = 3;
        Rect safeArea = Screen.safeArea;

        float spacing = 1.5f;
        float verticalScale = 1.2f;

        Vector3 gridCenter = new Vector3(
            safeArea.position.normalized.x,
            0.1f,
            safeArea.position.normalized.y
        );

        Vector3 newStartPos = gridCenter - new Vector3(
            ((sizeX - 1) * 0.5f) * spacing,
            0f,
            ((sizeY - 1) * 0.5f) * spacing * verticalScale
        );

        int total = sizeX * sizeY;
        int count = 1;

        int x = sizeX / 2;
        int y = sizeY / 2;
        if (sizeX % 2 == 0) x--;
        if (sizeY % 2 == 0) y--;

        count++;

        int[][] directions = new int[][]
        {
            new int[]{1, 0},
            new int[]{0, 1},
            new int[]{-1, 0},
            new int[]{0, -1}
        };
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(
                            newStartPos + new Vector3(x * spacing, 0, y * spacing * verticalScale),
                            new Vector3(1, 0.1f, 1.5f)
                        );
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
                        count++;

                        Gizmos.DrawCube(
                           newStartPos + new Vector3(x * spacing, 0, y * spacing * verticalScale),
                           new Vector3(1, 0.1f, 1.5f)
                       );


                        Gizmos.color = ((x + y) % 2 == 0) ? Color.red : Color.green;

                        if (count > total) break;
                    }
                }
                if (d % 2 == 1) steps++;
            }
        }


        Gizmos.DrawCube(newStartPos, new Vector3(1, 0.1f, 1.5f));
    }
*/


    void Awake()
    {
        safeArea = Screen.safeArea; 
        //SpiralMatrixCards(5,4);
    }


}
