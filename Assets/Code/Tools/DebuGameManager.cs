using UnityEngine;

namespace Tools
{
    public static class DebuGameManager
    {
        public static void Log(string logMessage, string[] tags)
        {
            if (tags.Length == 0)
            {
                Log(logMessage);
            }
            else
            {
                Log(GetTaggedMessage(logMessage, tags));
            }
        }
        
        public static void Log(string logMessage)
        {
            Debug.Log(logMessage);
        }

        public static void Error(string errorMessage, string[] tags)
        {
            if (tags.Length == 0)
            {
                Error(errorMessage);
            }
            else
            {
                Error(GetTaggedMessage(errorMessage, tags));
            }
        }

        public static void Error(string errorMessage)
        {
            Debug.LogError(errorMessage);
        }

        public static void Warning(string warningMessage, string[] tags)
        {
            if (tags.Length == 0)
            {
                Error(warningMessage);
            }
            else
            {
                Error(GetTaggedMessage(warningMessage, tags));
            }
        }

        public static void Warning(string warningMessage)
        {
            Debug.LogWarning(warningMessage);
        }

        private static string GetTaggedMessage(string logMessage, string[] tags)
        {
            string tagsMessage = null;

            for (int i = 0; i < tags.Length; i++)
            {
                tagsMessage += $"[{tags[i]}]";
            }

            tagsMessage += logMessage;

            return tagsMessage;
        }
    }
}
