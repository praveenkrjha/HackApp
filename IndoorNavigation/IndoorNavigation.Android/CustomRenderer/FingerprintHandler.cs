using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Runtime;
using Android.Security.Keystore;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using IndoorNavigation.Droid.CustomRenderer;
using Java.Security;
using Javax.Crypto;
using Xamarin.Forms;
[assembly: Dependency(typeof(FingerprintHandler))]
namespace IndoorNavigation.Droid.CustomRenderer
{

    internal class FingerprintHandler : FingerprintManager.AuthenticationCallback, IFingerprint
    {


        private Context mainActivity; private KeyStore keyStore; private Cipher cipher; private string KEY_NAME = "AutoCloud"; public FingerprintHandler() { this.mainActivity = Forms.Context; }
        public override void OnAuthenticationFailed()
        {
            Toast.MakeText(mainActivity, "Fingerprint Authentication failed!", ToastLength.Long).Show();
        }
        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            FModel fModel = new FModel();
            fModel.flag = true;
            Toast.MakeText(mainActivity, "Fingerprint Authentication Successful!", ToastLength.Long).Show();
            //MarkAttendancePage.FPNavigate();
        }
        private bool CipherInit()
        {
            try
            {
                cipher = Cipher.GetInstance(KeyProperties.KeyAlgorithmAes + "/" + KeyProperties.BlockModeCbc + "/" + KeyProperties.EncryptionPaddingPkcs7); keyStore.Load(null); IKey key = (IKey)keyStore.GetKey(KEY_NAME, null); cipher.Init(CipherMode.EncryptMode, key); return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void GenKey()
        {
            keyStore = KeyStore.GetInstance("AndroidKeyStore");
            KeyGenerator keyGenerator = null;
            keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, "AndroidKeyStore");
            keyStore.Load(null);
            keyGenerator.Init(new KeyGenParameterSpec.Builder(KEY_NAME, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt).SetBlockModes(KeyProperties.BlockModeCbc).SetUserAuthenticationRequired(true).SetEncryptionPaddings(KeyProperties.EncryptionPaddingPkcs7).Build());
            keyGenerator.GenerateKey();
        }
        internal void StartAuthentication(FingerprintManager fingerprintManager, FingerprintManager.CryptoObject cryptoObject)
        {
            CancellationSignal cancellationSignal = new CancellationSignal();
            if (ActivityCompat.CheckSelfPermission(mainActivity, Manifest.Permission.UseFingerprint) != (int)Android.Content.PM.Permission.Granted)
                return;
            fingerprintManager.Authenticate(cryptoObject, cancellationSignal, 0, this, null);
        }
        public void Authenticate()
        {
            KeyguardManager keyguardManager = (KeyguardManager)Forms.Context.GetSystemService("keyguard");
            FingerprintManager fingerprintManager = (FingerprintManager)Forms.Context.GetSystemService("fingerprint");
            if (ActivityCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.UseFingerprint) != (int)Android.Content.PM.Permission.Granted)
                return;
            if (!fingerprintManager.IsHardwareDetected) Toast.MakeText(Forms.Context, "FingerPrint authentication permission not enable", ToastLength.Short).Show();
            else
            {
                if (!fingerprintManager.HasEnrolledFingerprints)
                    Toast.MakeText(Forms.Context, "Register at least one fingerprint in Settings", ToastLength.Short).Show();
                else
                {
                    if (!keyguardManager.IsKeyguardSecure)
                        Toast.MakeText(Forms.Context, "Lock screen security not enable in Settings", ToastLength.Short).Show();
                    else GenKey();
                    if (CipherInit())
                    {
                        FingerprintManager.CryptoObject cryptoObject = new FingerprintManager.CryptoObject(cipher);

                        StartAuthentication(fingerprintManager, cryptoObject);
                    }
                }
            }
        }
    }
}