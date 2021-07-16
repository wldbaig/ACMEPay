namespace API.Common
{
    public class Enumeration
    {
        #region Structs
         
        public struct MessageType
        {
            public const string Error = "Error";
            public const string Info = "Info";
            public const string Warning = "Warning";
            public const string Success = "Success";
        }
        #endregion

        #region Enums
        public enum ErrorLogSource
        {
            API = 1
        }

        public enum PaymentStatus
        {
            Authorized = 1,
            Captured = 2,
            Voided = 3
        }
        #endregion
    }
}
