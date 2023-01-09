using UnityEngine;

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
