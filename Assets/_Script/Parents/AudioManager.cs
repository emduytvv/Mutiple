using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("-----------------Audio Source------------------")]
    [SerializeField] protected AudioSource musicSource;
    [SerializeField] protected AudioSource SFXSource;
    [SerializeField] protected AudioSource UISource;

    [Header("-----------------Audio Clip------------------")]
    public AudioClip BattleBackGround;
    public AudioClip LobbyBackGround;
    public AudioClip MenuBackGround;
    [SerializeField] private AudioClip _uIClick;
    public AudioClip UIClick => _uIClick;
    [SerializeField] private AudioClip _shootSFX;
    public AudioClip ShootSFX => _shootSFX;
    [SerializeField] private AudioClip _dashSFX;
    public AudioClip DashSFX => _dashSFX;
    [SerializeField] private AudioClip _jumpSFX;
    public AudioClip JumpSFX => _jumpSFX;
    [SerializeField] private AudioClip _enemyHitSFX;
    public AudioClip EnemyHitSFX => _enemyHitSFX;
    [SerializeField] private AudioClip _aim;
    public AudioClip Aim => _aim;
    [SerializeField] private AudioClip _explosionSFX;
    public AudioClip ExplosionSFX => _explosionSFX;
    [SerializeField] private AudioClip _goldPickupSFX;
    public AudioClip GoldPickupSFX => _goldPickupSFX;
    public void PlayMenuMusic() => PlayMusic(MenuBackGround);
    public void PlayLobbyMusic() => PlayMusic(LobbyBackGround);
    public void PlayBattleMusic() => PlayMusic(BattleBackGround);

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) => SFXSource.PlayOneShot(clip);
    public void PlayUI(AudioClip clip) => UISource.PlayOneShot(clip);
}
