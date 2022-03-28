using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Profiles;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Profiles.Queries
{
    public class ProfileQueries : IProfileQueries
    {
        private readonly IConfiguration _configuration;
        private readonly VueTicketDbContext _vueTicketDbContext;
        public ProfileQueries(IConfiguration configuration, VueTicketDbContext vueTicketDbContext)
        {
            _configuration = configuration;
            _vueTicketDbContext = vueTicketDbContext;
        }
        public UsermasterEditView GetprofileById(long userId)
        {
            try
            {
                using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
                var connection = sqlDataAccessManager.StartConnection();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return connection.Query<UsermasterEditView>("Usp_Usermasters_GetAgentByUserId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public UserProfileView GetUserprofileById(long userId)
        {
            try
            {
                using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
                var connection = sqlDataAccessManager.StartConnection();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                return connection.Query<UserProfileView>("Usp_UserProfileByUserId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Signatures GetSignatureDetails(long userId)
        {
            try
            {
                var data = (from signature in _vueTicketDbContext.Signatures
                            where signature.UserId == userId
                            select signature).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsProfileImageExists(long userId)
        {
            try
            {

                var data = (from pi in _vueTicketDbContext.ProfileImageStatus
                            where pi.UserId == userId
                            select pi).Any();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetProfileImageBase64String(long userId)
        {
            try
            {
                var data = (from pi in _vueTicketDbContext.ProfileImage
                            where pi.UserId == userId
                            select pi.ProfileImageBase64String).FirstOrDefault();
                return data;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetSignature(long userId)
        {
            try
            {
                var signaturevalue = (from signature in _vueTicketDbContext.Signatures
                    where signature.UserId == userId
                    select signature.Signature).FirstOrDefault();


                return signaturevalue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CheckSignatureAlreadyExists(long userId)
        {
            try
            {
                var signaturesExists = (from sign in _vueTicketDbContext.Signatures
                                        where sign.UserId == userId
                                        select sign).Any();
                return signaturesExists;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}