using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPointsManager : SaiMonoBehaviour
{
    [SerializeField] protected List<Transform> points_Bat;
    public List<Transform> Points_Bat => points_Bat;
    [SerializeField] protected List<Transform> points_MagicMini;
    public List<Transform> Points_MagicMini => points_MagicMini;
    [SerializeField] protected List<Transform> points_ShooterOnPlatform;
    public List<Transform> Points_ShooterOnPlatform => points_ShooterOnPlatform;
    [SerializeField] protected List<Transform> points_Airm;
    public List<Transform> Points_Airm => points_Airm;
    [SerializeField] protected List<Transform> points_Melee;
    public List<Transform> Points_Melee => points_Melee;
    [SerializeField] protected List<Transform> points_Explosion;
    public List<Transform> Points_Explosion => points_Explosion;

    [SerializeField] protected List<Transform> points_Slime;
    public List<Transform> Points_Slime => points_Slime;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPointsBat();
        LoadPointsMagicMini();
        LoadPointsShooterOnPlatform();
        LoadPointsAirm();
        LoadPointsMelee();
        LoadPointsSlime();
        LoadPointsExplosion();
    }

    private void LoadPointsBat()
    {
        if (points_Bat.Count > 0) return;
        Transform points = transform.Find("Points_Bat");
        foreach (Transform point in points)
        {
            points_Bat.Add(point);
        }
        Debug.Log(transform.name + ": Load Points_Bat", gameObject);
    }
    private void LoadPointsMagicMini()
    {
        if (points_MagicMini.Count > 0) return;
        Transform points = transform.Find("Points_MagicMini");
        foreach (Transform point in points)
        {
            points_MagicMini.Add(point);
        }
        Debug.Log(transform.name + ": LoadPointsMagicMini", gameObject);
    }
    private void LoadPointsShooterOnPlatform()
    {
        if (points_ShooterOnPlatform.Count > 0) return;
        Transform points = transform.Find("Points_ShooterOnPlatform");
        foreach (Transform point in points)
        {
            points_ShooterOnPlatform.Add(point);
        }
        Debug.Log(transform.name + ": LoadPointsShooterOnPlatform", gameObject);
    }
    private void LoadPointsAirm()
    {
        if (points_Airm.Count > 0) return;
        Transform points = transform.Find("Points_Airm");
        foreach (Transform point in points)
        {
            points_Airm.Add(point);
        }
        Debug.Log(transform.name + ": LoadPointsAirm", gameObject);
    }
    private void LoadPointsMelee()
    {
        if (points_Melee.Count > 0) return;
        Transform points = transform.Find("Points_Melee");
        foreach (Transform point in points)
        {
            points_Melee.Add(point);
        }
        Debug.Log(transform.name + ": LoadPointsMelee", gameObject);
    }
    private void LoadPointsSlime()
    {
        if (points_Slime.Count > 0) return;
        Transform points = transform.Find("Points_Slime");
        foreach (Transform point in points)
        {
            points_Slime.Add(point);
        }
        Debug.Log(transform.name + ": LoadPointsSlime", gameObject);
    }
    private void LoadPointsExplosion()
    {
        if (points_Explosion.Count > 0) return;
        Transform points = transform.Find("Points_Explosion");
        foreach (Transform point in points)
        {
            points_Explosion.Add(point);
        }
        Debug.Log(transform.name + ": LoadPointsExplosion", gameObject);
    }
}
