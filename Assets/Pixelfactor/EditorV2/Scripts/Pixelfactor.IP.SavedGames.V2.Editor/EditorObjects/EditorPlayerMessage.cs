using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    /// <summary>
    /// Defines a message sent to the player. E.g. distress calls, news
    /// </summary>
    public class EditorPlayerMessage : MonoBehaviour
    {
        public int Id = -1;
        public int MessageTemplateId = -1;
        public double EngineTimeStamp;
        public float ShowTime = -1.0f;
        public bool AllowDelete;
        public bool Opened;
        public EditorUnit SenderUnit;
        public EditorUnit SubjectUnit;
        public string ToText;
        public string FromText;
        public string SubjectText;
        [Multiline(7)]
        public string MessageText;

        /// <summary>
        /// Determines whether player is notified when message received<br />
        /// Applies to delayed messages where <see cref="ShowTime"/> greater than zero
        /// </summary>
        public bool Notifications = true;

        /// <summary>
        /// Determines whether player gets alert when message is received
        /// Applies to delayed messages where <see cref="ShowTime"/> greater than zero
        /// </summary>
        public bool Important = true;
    }
}
