using CleanArchitecture.Core.Entities.Complaint;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces.Complaint;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Complaint;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Complaint
{
    public class ComplainmasterServices : IComplainmaster
    {
        private readonly CleanArchitectureContext _dbContext;
        private readonly ICustomRepository<Complainmaster> _ComplainmasterRepository;

        public ComplainmasterServices(ICustomRepository<Complainmaster> customRepository, CleanArchitectureContext context)
        {
            _ComplainmasterRepository = customRepository;
            _dbContext = context;
        }
        public long AddComplainmaster(ComplainmasterReqViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var Compaintmaster = new Complainmaster()
                    {
                        Description=model.Description,
                        Subject=model.Subject,
                        TypeId=model.TypeId,
                        UserID=model.UserID,
                        Status=0,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = model.UserID
                    };
                    _ComplainmasterRepository.Insert(Compaintmaster);
                    return Compaintmaster.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
           
        }

        public List<CompainDetailResponse> GetComplain(int ComplainId)
        {
            try
            {
                string Qry = "";
                IQueryable<CompainDetailResponse> Result;
                Qry = @"Select Tm.SubType,cm.Id as CompainNumber,cm.Subject,ct.Description,ct.Complainstatus,ct.CreatedDate From Complainmaster CM Inner join CompainTrail CT On Cm.Id=Ct.ComplainId
                        Inner join Typemaster Tm on tm.Id=cm.TypeId  Where Cm.id=" + ComplainId + " order by cm.CreatedDate desc ";
                Result = _dbContext.compainDetailResponse.FromSql(Qry);
                return Result.ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        public List<UserWiseCompaintDetailResponce> GetComplainByUserWise(int UserId)
        {
            try
            {
                string Qry = "";
                IQueryable<UserWiseCompaintDetailResponce> Result;
                Qry = @" Select Cm.Subject,Cm.Description,Tm.SubType,Cm.Id as CompainNumber
  From Complainmaster Cm Inner join Typemaster Tm On Cm.TypeId=tm.Id  Where cm.UserID=" + UserId + " order by Cm.CreatedBy Desc";
                Result = _dbContext.userWiseCompaintDetailResponce.FromSql(Qry);
                return Result.ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
