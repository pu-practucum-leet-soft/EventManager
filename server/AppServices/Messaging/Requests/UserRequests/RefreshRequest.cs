namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// Заявка за обновяване на достъп (access) токен чрез валиден refresh токен.
    /// Използва се, когато текущият JWT е изтекъл или предстои да изтече.
    /// </summary>
    public sealed class RefreshRequest
    {
        /// <summary>
        /// Refresh токенът, издаден на потребителя при първоначалното вписване.
        /// Служи за издаване на нов JWT без повторно въвеждане на креденшъли.
        /// </summary>
        public string RefreshToken { get; set; } = default!;
    }
}
