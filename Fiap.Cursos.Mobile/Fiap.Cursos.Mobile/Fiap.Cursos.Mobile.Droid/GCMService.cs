using System;

using Android.App;
using Android.Content;
using Gcm.Client;
using Android.Util;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using Android.Support.V4.App;
using Android.Media;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace Fiap.Cursos.Mobile.Droid
{
    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] { "814187753939" };
    }

    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService() : base(PushHandlerBroadcastReceiver.SENDER_IDS) { }

        protected override void OnRegistered(Context context, string registrationId)
        {
            RegistrationID = registrationId;
            Log.Verbose("PushHandlerBroadcastReceiver", $"GCM Registrado: {RegistrationID}");

            Push push = App.Client.GetPush();
            MainActivity.CurrentActivity.RunOnUiThread(() => Register(push, null));
        }

        public async void Register(Push push, IEnumerable<string> tags)
        {
            try
            {
                string disciplinaTemplate = "{\"data\":{\"nome\":\"$(Nome)\", \"coordenador\":\"$(Coordenador)\"}}";

                JObject templates = new JObject();
                templates["disciplinaAlteradaTemplate"] = new JObject { { "body", disciplinaTemplate } };

                await push.RegisterAsync(RegistrationID, templates);
                Log.Info("Push Instala��o Id", push.InstallationId.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info("PushHandlerBroadcastReceiver", "GCM Mensagem recebida!");

            StringBuilder msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            ISharedPreferences prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            ISharedPreferencesEditor edit = prefs.Edit();
            edit.PutString("last_msg", msg.ToString());
            edit.Commit();

            string nome = intent.Extras.GetString("nome");
            string coordernador = intent.Extras.GetString("coordenador");
            bool isTemplate = !string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(coordernador);
            string title = isTemplate ? "Disciplina alterada!" : "Detalhes de mensagem desconhecida";
            string message = isTemplate ? $"Disciplina {nome} alterada por {coordernador}." : msg.ToString();
            CreateNotification(title, message);
        }

        private void CreateNotification(string title, string message)
        {
            NotificationManager notificationManager = GetSystemService(NotificationService) as NotificationManager;

            Intent uiIntent = new Intent(this, typeof(MainActivity));

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            Notification notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, 0))
                    .SetSmallIcon(Android.Resource.Drawable.SymActionEmail)
                    .SetTicker(title)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                    .SetAutoCancel(true).Build();

            notificationManager.Notify(1, notification);
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "Removido registro do Id: " + registrationId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "GCM Erro: " + errorId);
        }
    }
}