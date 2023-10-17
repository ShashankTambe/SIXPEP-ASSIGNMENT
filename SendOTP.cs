using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class SendOTP : MonoBehaviour
{
    public TMP_InputField phoneNumberInputField;

    private FirebaseAuth auth;
    private PhoneAuthProvider provider;

    private void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            provider = PhoneAuthProvider.GetInstance(auth);
        });
    }

    public void SendOTPRequest()
    {
        string phoneNumber = phoneNumberInputField.text;

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            // Send OTP request
            provider.VerifyPhoneNumber(phoneNumber, 60, null, verificationCompleted: (credential) => {
                // Auto-retrieved on some Android devices.
                // You can proceed with sign-in (credential) here.
                // In this example, we'll switch to the OTP verification scene.
                SceneManager.LoadScene("OTPVerificationScene");
            }, verificationFailed: (exception) => {
                Debug.LogError("Phone verification failed");
            }, codeSent: (id, token) => {
                // Store the verification ID (id) for later use
                PlayerPrefs.SetString("VerificationID", id);
            }, codeAutoRetrievalTimeOut: (id) => {
                // Auto-retrieval timeout
            });
        }
    }
}
