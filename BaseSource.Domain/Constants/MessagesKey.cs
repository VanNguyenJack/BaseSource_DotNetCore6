using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Constants
{
    public static class MessagesKey
    {
        #region Identity
        public const string UserNotFound = "UserNotFound";
        public const string AccountNotConfirmed = "AccountNotConfirmed";
        public const string AccountEmailNotConfirmed = "AccountEmailNotConfirmed";
        public const string AccountPhoneNumberNotConfirmed = "AccountPhoneNumberNotConfirmed";
        public const string AccountInactive = "AccountInactive";
        public const string InvalidCredentials = "InvalidCredentials";
        public const string NoAccount = "NoAccount";
        public const string InvalidRefreshToken = "InvalidRefreshToken";
        public const string ExpiredRefreshToken = "ExpiredRefreshToken";
        public const string UsernameExists = "UsernameExists";
        public const string EmailRegistered = "EmailRegistered";
        public const string PhoneNumberRegistered = "PhoneNumberRegistered";
        public const string ErrorProcessing = "ErrorProcessing";
        public const string AnErrorOccurWithDetail = "AnErrorOccurWithDetail";
        public const string Authenticated = "Authenticated";
        public const string InvalidFacebookAccessToken = "InvalidFacebookAccessToken";
        public const string InvalidGoogleAccessToken = "InvalidGoogleAccessToken";
        public const string ResetPasswordError = "ResetPasswordError";
        public const string ResetPassword = "ResetPassword";
        public const string ResetPasswordMail = "ResetPasswordMail";
        public const string ResetPasswordSuccess = "ResetPasswordSuccess";
        public const string ResetPasswordForUser = "ResetPasswordForUser";
        public const string ConfirmRegister = "ConfirmRegister";
        public const string ConfirmRegistrationSubject = "ConfirmRegistrationSubject";
        public const string Registered = "Registered";
        public const string ConfirmingError = "ConfirmingError";
        public const string InvalidEmailConfirmationToken = "InvalidEmailConfirmationToken";
        public const string EmailConfirmed = "EmailConfirmed";
        public const string LogOut = "LogOut";
        public const string NotFound = "NotFound";
        public const string PropertyRequired = "PropertyRequired";
        public const string GroupByClauseRequired = "GroupByClauseRequired";
        public const string PropertyNotExceedLength = "PropertyNotExceedLength";
        public const string NameExists = "NameExists";
        public const string NotExists = "NotExists";
        public const string InvalidEmployeeLinkUser = "InvalidEmployeeLinkUser";
        public const string InvalidTenantAlreadyExists = "InvalidTenantAlreadyExists";
        public const string UserIdEmpty = "UserIdEmpty";
        public const string AdminPasswordEmpty = "AdminPasswordEmpty";
        public const string InvalidImage = "InvalidImage";
        public const string ImageIsTooLarge = "ImageIsTooLarge";
        public const string InvalidOrderSpecified = "InvalidOrderSpecified";
        public const string ColumnNotSupportedForGrouping = "ColumnNotSupportedForGrouping";
        public const string GroupingByMultipleColumnsNotSupported = "GroupingByMultipleColumnsNotSupported";
        public const string UserIsNotSupervisor = "User Is Not Supervisor.";

        #endregion
        public const string Success = "Success";
        public const string Exception = "Exception";
        public const string InternalServerError = "InternalServerError";
        public const string StoreException = "StoreException";
        public const string Information = "Information";
        public const string Error = "Error";

        public const string PhysicalFileNotExist = "File [{0}] not exist in our system";
        public const string Exception_Database_Effect = "Database error. Please screenshot and contact WMSSupport@regallogistics.com.";
    }
}
