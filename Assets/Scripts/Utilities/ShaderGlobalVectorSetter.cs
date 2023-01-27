using UnityEngine;

/// <summary>
/// Utility class used to align the shader vector _GlobalNormals with the sun direction.
/// To be used on the main directional light in each scene that has nature.
/// </summary>
public class ShaderGlobalVectorSetter : MonoBehaviour
{
    private void OnValidate() => UpdateGlobalShaderVector();
    private void OnDrawGizmosSelected() => UpdateGlobalShaderVector();
    private void Start() => UpdateGlobalShaderVector();

    private void UpdateGlobalShaderVector()
    {
        Shader.SetGlobalVector("_GlobalNormals", transform.forward);
    }
}
