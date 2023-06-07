using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] Material[] _mats;
    [SerializeField] Material[] _defaultMaterials;
    [SerializeField] Material[] _squishedMaterials;
    [SerializeField] SkinnedMeshRenderer _renderer;
    
    [SerializeField] bool _skinless;
    [SerializeField] MeshRenderer _altRenderer;

    void Awake()
    {
        if(!_skinless)
        {
            _mats = _renderer.materials;
        }
    }

    public void SwapMaterials()
    {
        if(_skinless)
        {
            _altRenderer.material = _squishedMaterials[0];
        }
        else
        {
            for(int i = 0; i < _renderer.materials.Length; i++)
            {
                _mats[i] = _squishedMaterials[i];
            }
            _renderer.materials = _mats;
        }
    }

    public void UnswapMaterials()
    {
        if(_skinless)
        {
            _altRenderer.material = _defaultMaterials[0];
        }
        else
        {
            for(int i = 0; i < _renderer.materials.Length; i++)
            {
                _mats[i] = _defaultMaterials[i];
            }
            _renderer.materials = _mats;
        }
    }
}
