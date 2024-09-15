using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private List<ResourceNode> resourceNodeList = new List<ResourceNode>();
    [SerializeField] private List<AudioClip> wooshSounds = new List<AudioClip>();

    private AudioSource audioSource;

    private bool isInputActive = true;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        ChangeIsInputActive(false);

        GameObject[] resourceNodeGOArray = GameObject.FindGameObjectsWithTag("ResourceNode");
        
        foreach(GameObject resourceNodeGO in resourceNodeGOArray)
        {
            resourceNodeList.Add(resourceNodeGO.GetComponent<ResourceNode>());
        }
    }

    public bool GetIsInputActive() => isInputActive;
    public void ChangeIsInputActive(bool cond) => isInputActive = cond;
    public List<ResourceNode> GetResourceNodeList() => resourceNodeList;
    public void PlaySoundRandomPitch()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.clip = wooshSounds[UnityEngine.Random.Range(0, wooshSounds.Count - 1)];
        audioSource.Play();
    }
}
