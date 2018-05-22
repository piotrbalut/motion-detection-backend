namespace MotionDetection.Backend.Interfaces.Resources
{
    public interface ICommonResource
    {
		#region General

	    string AppName { get; }

        #endregion

        #region Requests

        string RequestInvalidEmail { get; }

        #endregion

        #region MailService

        string MailServiceConfirmationCodeTitle { get; }
        string MailServiceConfirmationCodeMessage { get; }

        #endregion
    }
}
