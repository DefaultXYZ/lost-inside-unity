using UnityEngine;

public class StoryNote : MonoBehaviour, IInteractable, IObservable
{
    [SerializeField]
    private Material outlineMaterial;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private string text;

    private Material[] normalMaterials;
    private readonly Material[] outlineMaterials = new Material[2];

    private void Awake()
    {
        normalMaterials = meshRenderer.materials;
        outlineMaterials[0] = normalMaterials[0];
        outlineMaterials[1] = outlineMaterial;
    }

    public void OnInteract()
    {
    }

    public void OnLookReceived()
    {
        meshRenderer.materials = outlineMaterials;
    }

    public void OnLookLost()
    {
        meshRenderer.materials = normalMaterials;
    }
}