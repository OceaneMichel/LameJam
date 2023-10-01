using UnityEngine;

public class PlaySound : MonoBehaviour
{
   [SerializeField] private AudioSource m_source;
   [SerializeField] private AudioClip m_sound;
   
   public void Play()
   {
      AudioSource.PlayClipAtPoint(m_sound, transform.position);
   }

   public void PlayInSource()
   {
      if (m_source != null)
      {
         m_source.clip = m_sound;
         m_source.Play();
      }
      else
      {
         Play();
      }
   }
}
