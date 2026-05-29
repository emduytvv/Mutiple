using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseDatabaseManager : SaiMonoBehaviour
{
    private DatabaseReference reference;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        WriteDatabase("123", "Xin chao the gioi!");
        ReadDatabase("123");
    }


    public void WriteDatabase(string id, string message)
    {
        reference.Child("Users").Child(id).SetValueAsync(message).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("Ghi du lieu thanh cong");
            else
                Debug.Log("Ghi du lieu that bai: " + task.Exception);
        });
    }


    public void ReadDatabase(string id)
    {
        reference.Child("Users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Doc du lieu thanh cong: " + snapshot.Value.ToString());
            }
            else
                Debug.Log("Doc du lieu that bai: " + task.Exception);
        });
    }
}
