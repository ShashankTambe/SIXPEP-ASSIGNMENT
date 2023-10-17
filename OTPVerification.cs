using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class OTPVerification : MonoBehaviour
{
    public TMP_InputField[] otpInputFields;
    private FirebaseAuth auth;
    private PhoneAuthProvider phoneAuthProvider;

    private void Start()
    {
        InitializeFirebase();
        phoneAuthProvider = PhoneAuthProvider.GetInstance(auth);
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });
    }

    public void VerifyOTP()
    {
        string verificationId = PlayerPrefs.GetString("VerificationID");

        if (!string.IsNullOrEmpty(verificationId))
        {
            string fullCode = GetFullCode();

            Credential credential = phoneAuthProvider.GetCredential(verificationId, fullCode);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    FirebaseUser user = task.Result;
                    Debug.Log("User signed in: " + user.DisplayName + " (" + user.UserId + ")");
                    // You can perform actions upon successful sign-in.
                }
                else
                {
                    Debug.LogError("Sign-in failed: " + task.Exception);
                }
            });
        }
    }

    private string GetFullCode()
    {
        string fullCode = "";
        foreach (TMP_InputField inputField in otpInputFields)
        {
            fullCode += inputField.text;
        }
        return fullCode;
    }
}
