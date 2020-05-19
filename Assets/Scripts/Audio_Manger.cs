using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manger : MonoBehaviour
{
    public AudioSource music_ad;
    public AudioSource sound_ad;

    public AudioClip[] music;
    public AudioClip coin_clip;
    public AudioClip enemy_clip;
    public AudioClip win_clip;
    public AudioClip gameover_clip;
    public AudioClip doublleCoin_clip;
    public AudioClip clik_sound;
    public AudioClip wrong_option_clip;
    public AudioClip jump_clip;

    public int isMute = 0;//0=not mute 1= mute

    private void Awake()
    {
        mute_unmute( PlayerPrefs.GetInt("Sound", 0));
      
    }
    public void mute_unmute(int tmp)
    {
        isMute = tmp;
        PlayerPrefs.SetInt("Sound", isMute);
        if (isMute == 1)
        {
            //mute audio
            music_ad.mute = true;
            sound_ad.mute = true;
        }else if (isMute == 0)
        {
            //unmute audio
            music_ad.mute = false;
            sound_ad.mute = false;
        }
    }
    public void playSound(AudioClip ad_clip)
    {
        sound_ad.Stop();
        sound_ad.PlayOneShot(ad_clip, 1);
    }

    //o==main screen 1= play
    public void back_music(bool main_screen)
    {
        music_ad.Stop();
        if(main_screen)
        {
            music_ad.clip=music[0];
        }else
        {
            music_ad.clip = music[1];
        }
        music_ad.Play();
    }
}
