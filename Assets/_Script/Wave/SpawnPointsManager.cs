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
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPointsBat();
        LoadPointsMagicMini();
        LoadPointsShooterOnPlatform();
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
}
