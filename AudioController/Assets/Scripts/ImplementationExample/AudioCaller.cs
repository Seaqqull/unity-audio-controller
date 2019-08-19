using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    [SerializeField] private Audio.AudioController _controller;

    private string _ambientKey;


    public void PlaySFX(string audioName)
    {
        _controller.Play(audioName);
    }

    public void PlayAmbient(string audioName)
    {
        if (_ambientKey != null) return;

        _ambientKey  = _controller.Play(audioName);
    }

    public void StopAmbient(string audioName)
    {
        if (_ambientKey == null) return;

        if (_controller.StopInstant(audioName, _ambientKey))
            _ambientKey = null;
    }

}
