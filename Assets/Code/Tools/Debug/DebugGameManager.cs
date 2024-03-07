using UnityEngine;

namespace Tools
{
    public static class DebugGameManager
    {
        public static void Log(string logMessage, DebugTags[] tags)
        {
            Log(tags.Length == 0 ? logMessage : GetTaggedMessage(logMessage, tags));
        }
        
        public static void Log(string logMessage)
        {
            Debug.Log(logMessage);
        }

        public static void Error(string errorMessage, DebugTags[] tags)
        {
            Error(tags.Length == 0 ? errorMessage : GetTaggedMessage(errorMessage, tags));
        }

        private static void Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
        }

        public static void Warning(string warningMessage, DebugTags[] tags)
        {
            Error(tags.Length == 0 ? warningMessage : GetTaggedMessage(warningMessage, tags));
        }

        public static void Warning(string warningMessage)
        {
            Debug.LogWarning(warningMessage);
        }

        private static string GetTaggedMessage(string logMessage, DebugTags[] tags)
        {
            string tagsMessage = null;

            for (int i = 0; i < tags.Length; i++)
            {
                tagsMessage += $"[{tags[i].GetDescription()}]";
            }

            tagsMessage += logMessage;

            return tagsMessage;
        }
    }
}
