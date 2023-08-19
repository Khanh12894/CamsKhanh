namespace XichLip.WebApi.Constants
{
    public class WsConstants
    {
        public const int CodeStatusSuccess = 0;
        public const int CodeStatusFail = 1;
        public const int CodeStatusFailException = 500;
        public const int CodeStatusAuthenticationFailedFail = 401;
        public const int CodeBadRequest = 400;

        public const string MessageGetDataSuccess = "Get data successfully";
        public const string MessageGetDataFail = "Get data fail";
        public const string MessageSaveSuccess = "Save successfully";
        public const string MessageSaveFail = "Save unsuccessfully";
        public const string MessageDeleteSuccess = "Delete successfully";
        public const string MessageDeleteFail = "Delete unsuccessfully";
        public const string MessageItemNotExists = "Item does not exist. It may have been deleted by another user.";
        public const string MessageSignUpSuccess = "Signup successfully";
        public const string MessageSignUpFail = "Signup fail";
        public const string MessageSignInSuccess = "Signin successfully";
        public const string MessageSignInFail = "Signin fail";
        public const string MessageSignUpInvalidConfirmPassword = "Confirm password not match with password";
        public const string MessageSignUpExistedEmail = "Your input email has existed";
        public const string MessageException = "Internal Server Error";
        public const string MessagePermissionDeny = "Permission Deny";
        public const string MessageUserHasExist = "User has exist!";
        public const string MessageChangePasswordSuccess = "Change password successful";
        public const string MessageChangePasswordFailed = "Can not change the password";
        public const string Message = "Can not change the password";

        public const string MessageQuantityAvailableForCancelUpdated = "Please refresh browser to update new data.";
        public const string MessageSetDataSuccess = "Set data successfully";

        public const string StatusSuccess = "success";
        public const string Statusfail = "failed";
        public const string CannotDelete = "Can not delete";
        
        public const string MessageInvalidAddress = "Invalid Address";
        public const string MessageValidAddress = "Valid Address";

        //Momo request type
        public const string Capture = "capture"; //xac nhan giao dich
        public const string RevertAuthorize = "revertAuthorize"; // huy giao dich treo tien vi that bai;
    }
}
