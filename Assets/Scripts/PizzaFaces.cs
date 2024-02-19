using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PizzaFaces : MonoBehaviour
{
    [SerializeField]
    Sprite[] _faces;
    SpriteRenderer _renderer;
    [SerializeField]
    AudioSource _audioSource;
    private void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void GenerateFace()
    {
        _renderer.sprite = _faces[Random.Range(0, _faces.Count())];
    }

    public void KillFace()
    {
        if(_renderer.sprite != null) 
            _audioSource.Play();
        _renderer.sprite = null;
    }
}
