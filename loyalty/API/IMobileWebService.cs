using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMobileWebService" in both code and config file together.
    [ServiceContract]
    public interface IMobileWebService
    {
        [OperationContract]
        string HelloWorld();

        [OperationContract]
        MemberObject GetMemberDetail();

        [OperationContract]
        void MemberLogin(string loginToken, string password, out int resultCode, out string resultContent);

        //[OperationContract]
        //void MemberCreate(string email, string password, out int resultCode, out MemberObject newMember);

        [OperationContract]
        void MissionIBeacon(int member_id, out int resultCode);

        [OperationContract]
        void MissionGetPoint(int member_id, out int resultCode);

        [OperationContract]
        void CreateCombineRedemption(int member_id, int coupon_id, int no_of_ppl, out int resultCode, out string combineRedemptionCode, out int new_combine_id);

        [OperationContract]
        void JoinCombineRedemption(int member_id, string combineRedemptionCode, out int resultCode, out int newCombineID, out int coupon_id, out int noOfPpl, out int position, out int pointRequire);

        [OperationContract]
        void CancelJoinCombineRedemption(int member_id, int combine_id, out int resultCode);

        [OperationContract]
        void NotifyHostCombineRedemption(int member_id, int combine_id, out int resultCode, out int totalJoined, out int new_position, out int new_status);

        [OperationContract]
        void ConfirmCombineRedemption(int member_id, int combine_id, out int resultCode);

        [OperationContract]
        void PointAdjust(int member_id, int pointAdjust, out int resultCode);

        [OperationContract]
        void GetCombineRedemptionStatus(int combine_id, out int resultCode, out int status);

        [OperationContract]
        void MemberLoginJson(string loginToken, string password, out int resultCode, out string content);

       


        //Test
        [OperationContract]
        void TestGetLogin(out string eloginToken, out string epassword);

       

        [OperationContract]
        void TestEncrypt(string message, out string eMessage);

        [OperationContract]
        void TestDecrypt(string eMessage, out string message);



        //new web service 
        [OperationContract]
        void NewGetMemberDetail(string eContent, out int resultCode, out string resultContent);

        [OperationContract]
        void GetMemberPointHistoryList(string eContent, out int resultCode, out string resultContent);

        [OperationContract]
        void MemberRegister(string eContent, out int resultCode, out string resultContent);
               
        [OperationContract]
        void MemberNormalLogin(string eContent, out int resultCode, out string resultContent);

        [OperationContract]
        void MemberLoginFB(string eContent, out int resultCode, out string resultContent);

        [OperationContract]
        void GetGiftList(string eContent, out int resultCode, out string resultContent);
        [OperationContract]
        void GetGiftDetail(string eContent, out int resultCode, out string resultContent);
        [OperationContract]
        void GetProductList(string eContent, out int resultCode, out string resultContent);
        //end of new web service 
    }
}
