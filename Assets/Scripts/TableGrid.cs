using UnityEngine;

public class TableGrid : MonoBehaviour
{
    
    public Vector2Int Size = Vector2Int.one;

    private void OnDrawGizmos()
    {
         Debug.Log("Gizmos");
        for (int x = 0; x < Size.x; x++)
        {
            for(int y = 0; y < Size.y; y++)
            {
                Gizmos.color = new Color(0.88f, 0f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
    void Start()
    {

        //OnDrawGizmos();
    }

    
}
