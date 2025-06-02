using UnityEngine;

public interface ICard
{
    Material Material { get; set; }
    void ChangeMaterial(int id,in Texture2D texture);
}
