using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class FirebaseLoginManager : SaiMonoBehaviour
{
    [Header("Register")]
    [SerializeField] private InputField ipRegisterEmail;
    [SerializeField] private InputField ipRegisterPassword;
    [SerializeField] private Button buttonRegister;
    [Header("Login")]
    [SerializeField] private InputField ipLoginEmail;
    [SerializeField] private InputField ipLoginPassword;
    [SerializeField] private Button buttonLogin;
    [Header("Login")]
    [SerializeField] private Button buttonMoveToLogin;
    [SerializeField] private Button buttonMoveToRegister;
    [SerializeField] private GameObject loginForm;
    [SerializeField] private GameObject registerForm;

    private FirebaseAuth auth;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        auth = FirebaseAuth.DefaultInstance;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        buttonRegister.onClick.AddListener(RegisterAccount);
        buttonLogin.onClick.AddListener(LoginAccount);

        buttonMoveToLogin.onClick.AddListener(SwitchForm);
        buttonMoveToRegister.onClick.AddListener(SwitchForm);

    }

    public void RegisterAccount()
    {
        string email = ipRegisterEmail.text;
        string password = ipRegisterPassword.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Dang ki bi huy");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Dang ki that bai: " + task.Exception);
                return;
            }
            Debug.Log("Dang ki thanh cong: " + task.Result.User.Email);
        });
    }
    public void LoginAccount()
    {
        string email = ipLoginEmail.text;
        string password = ipLoginPassword.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Dang nhap bi huy");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Dang nhap that bai: " + task.Exception);
                return;
            }
            Debug.Log("Dang nhap thanh cong: " + task.Result.User.Email);
            FirebaseUser user = task.Result.User;

            SceneManager.LoadScene("Menu");
        });
    }
    public void SwitchForm()
    {
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }
}
