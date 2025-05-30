using UnityEngine;

public interface ICard
{
    Material Material { get; set; }
    void ChangeMaterial(int id, Texture2D texture);
}
