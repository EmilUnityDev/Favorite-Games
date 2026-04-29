using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public AudioClip StoneEffect;
    public AudioClip MagicEffect;
    public AudioClip Money;
    public AudioSource CellSound;
    public void StartClickEffect()
    {
        CellSound.PlayOneShot(StoneEffect);
        CellSound.PlayOneShot(MagicEffect);
    }
    public void STartEffectMoney()
    {
        CellSound.PlayOneShot(Money);
    }
}
