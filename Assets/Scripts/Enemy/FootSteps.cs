using UnityEngine;
using UnityEngine.AI;

public class FootSteps : MonoBehaviour
{
    [SerializeField] AudioClip[] _footstepClips;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _walkVolume = 0.75f;
    [SerializeField] float _runVolume = 1f;
    
    void Step()
    {
        if(_agent.speed <= 2)
        {
            AudioClip clip = GetRandomClip();
            _audioSource.PlayOneShot(clip, _walkVolume);
        }
    }

    void RunStep()
    {
        if(_agent.speed > 2)
        {
            AudioClip clip = GetRandomClip();
            _audioSource.PlayOneShot(clip, _runVolume);
        }
    }

    AudioClip GetRandomClip()
    {
        return _footstepClips[Random.Range(0, _footstepClips.Length)];
    }
}
