using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int terrainIndex;
    private TerrainDetector _terrainDetector;
    private SoundManager _soundManager;
    
    private void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _soundManager.SetFootstepsTarget(gameObject.transform.Find("Footsteps").GetComponent<AudioSource>());
        _terrainDetector = new TerrainDetector();
    }
    
    public int GetTerrainIndex()
    {
        terrainIndex = _terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        Debug.Log(terrainIndex);
        return terrainIndex;
    }

    private void Step()
    {
        _soundManager.PlayFootstepsSound();
    }
}
