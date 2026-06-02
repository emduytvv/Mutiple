using System;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseDatabaseManager : Singleton<FirebaseDatabaseManager>
{
    private DatabaseReference _reference;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void ReadPlayerName(string uid, Action<string> onComplete)
    {
        _reference.Child("Users").Child(uid).Child("name")
            .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
                onComplete?.Invoke(task.Result.Value.ToString());
            else
                onComplete?.Invoke(null);
        });
    }
    public void SavePlayerName(string uid, string name)
    {
        _reference.Child("Users").Child(uid).Child("name")
            .SetValueAsync(name).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("Saved player name: " + name);
            else
                Debug.Log("Save failed: " + task.Exception);
        });
    }
}
