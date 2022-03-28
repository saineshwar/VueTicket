using System;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using TicketCore.Models.Profiles;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Profiles.Command
{
    public class ProfileCommand : IProfileCommand
    {
        private readonly IConfiguration _configuration;
        private readonly VueTicketDbContext _vueTicketDbContext;
        public ProfileCommand(IConfiguration configuration, VueTicketDbContext vueTicketDbContext)
        {
            _configuration = configuration;
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int UpdateUserMasterDetails(long userId, UsermasterEditView usermasterEditView)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@FirstName", usermasterEditView.FirstName);
                param.Add("@LastName", usermasterEditView.LastName);
                param.Add("@EmailId", usermasterEditView.EmailId);
                param.Add("@MobileNo", usermasterEditView.MobileNo);
                param.Add("@Gender", usermasterEditView.Gender);
                var result = connection.Execute("Usp_Usermasters_UpdateUserMasterDetails", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return result;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return 0;
                }

            }
            catch (Exception)
            {
                sqlDataAccessManager.Rollback();
                throw;
            }
        }

        public void UpdateProfileImage(ProfileImage profileImage)
        {
            try
            {
                using var dbContextTransaction = _vueTicketDbContext.Database.BeginTransaction();
                try
                {
                    _vueTicketDbContext.ProfileImage.Add(profileImage);
                    var result = _vueTicketDbContext.SaveChanges();

                    if (result != 0)
                    {
                        ProfileImageStatus profileImageStatus = new ProfileImageStatus
                        {
                            UserId = profileImage.UserId,
                            CreatedDate = DateTime.Now,
                            Isuploaded = true,
                            ProfileImageId = profileImage.ProfileImageId
                        };
                        _vueTicketDbContext.ProfileImageStatus.Add(profileImageStatus);
                        _vueTicketDbContext.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int DeleteProfileImage(long userId)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                var param = new DynamicParameters();
                param.Add("@UserId", userId);
                var result = connection.Execute("Usp_ProfileImage_DeleteProfileImage", param, transaction, 0, CommandType.StoredProcedure);

                if (result > 0)
                {
                    sqlDataAccessManager.Commit();
                    return result;
                }
                else
                {
                    sqlDataAccessManager.Rollback();
                    return 0;
                }

            }
            catch (Exception)
            {
                sqlDataAccessManager.Rollback();
                throw;
            }
        }

        public int UpdateSignature(Signatures signatures)
        {
            using var dbContextTransaction = _vueTicketDbContext.Database.BeginTransaction();
            try
            {
                int result = -1;
                _vueTicketDbContext.Signatures.Add(signatures);
                result = _vueTicketDbContext.SaveChanges();
                dbContextTransaction.Commit();
                return result;
            }
            catch (Exception)
            {
                dbContextTransaction.Rollback();
                throw;
            }
        }

        public int DeleteSignature(Signatures signatures)
        {
            using var dbContextTransaction = _vueTicketDbContext.Database.BeginTransaction();
            try
            {
                var result = -1;
                if (signatures != null) _vueTicketDbContext.Signatures.Remove(signatures);
                result = _vueTicketDbContext.SaveChanges();
                dbContextTransaction.Commit();
                return result;
            }
            catch (Exception)
            {
                dbContextTransaction.Rollback();
                throw;
            }
        }

    }
}