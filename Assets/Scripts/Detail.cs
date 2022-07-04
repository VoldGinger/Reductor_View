using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class Detail : MonoBehaviour
{
    public string Name;
    [Tooltip("It,s matters when mechanism is exploding")]
    public int DetailOrder;
    
    private Material _basicMaterial;
    private Material _transparentMaterial;
    private MeshRenderer _meshRenderer;


    public void Init(Material basicMaterial, Material transparentMaterial)
    {
        _basicMaterial = basicMaterial;
        _transparentMaterial = transparentMaterial;
    }
        
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }


    public void ShowDetail()
    {
        _meshRenderer.material = _basicMaterial;
    }
    public void HideDetail()
    {
        _meshRenderer.material = _transparentMaterial;

    }

}
