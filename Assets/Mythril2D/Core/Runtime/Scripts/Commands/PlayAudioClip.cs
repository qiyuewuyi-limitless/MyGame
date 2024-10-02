using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class PlayAudioClip : ICommand
    {
        [SerializeField] private AudioClipResolver m_audioClip = null;

        public void Execute()
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_audioClip);
        }

        public static void Play(AudioClipResolver clip)
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(clip);

        }
    }
}
