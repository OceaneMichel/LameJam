using UnityEngine;

public class PlaySound : MonoBehaviour
{
   [SerializeField] private AudioClip m_sound;

   public void Play()
   {
      AudioSource.PlayClipAtPoint(m_sound, transform.position);
   }
}
