namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    /// <summary> 
    /// Отговор, съдържащ броя на поканените участници.
    /// </summary>
    public class AddParticipantsResponse
    {
        /// <summary>
        /// Цяло число, което указва броя на поканените участници.
        /// </summary>
        public int Added { get; set; }
    }
}
