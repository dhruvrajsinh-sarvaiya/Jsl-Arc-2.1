using CleanArchitecture.Core.Entities.Complaint;
using CleanArchitecture.Core.Interfaces.Complaint;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Complaint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Complaint
{
    public class CompainTrailServices : ICompainTrail
    {
        private readonly CleanArchitectureContext _dbContext;
        public CompainTrailServices(CleanArchitectureContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddCompainTrail(CompainTrailVirewModel compainTrail)
        {
            try
            {
                if (compainTrail == null)
                {
                    return 0;
                }
                else
                {
                    var compainTrailData = new CompainTrail();
                    {
                        compainTrailData.ComplainId = compainTrail.ComplainId;
                        if (string.IsNullOrEmpty(compainTrail.Complainstatus))
                            compainTrailData.Complainstatus = "Open";
                        else
                            compainTrailData.Complainstatus = compainTrail.Complainstatus;
                        compainTrailData.CreatedDate = DateTime.UtcNow;
                        compainTrailData.CreatedBy = compainTrail.UserID;
                        compainTrailData.Description = compainTrail.Description;
                        if (string.IsNullOrEmpty(compainTrail.Remark))
                            compainTrailData.Remark = " ";
                        else
                            compainTrailData.Remark = compainTrail.Remark;
                    }
                    _dbContext.Add(compainTrailData);
                    _dbContext.SaveChanges();
                    return compainTrailData.Id;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
        public void GetData(int userid)
        {
            try
            {
                
                var Client = _dbContext.Complainmaster.Where(o => o.UserID.Equals(userid)).Select(m => new CompainTrail
                {
                   Description=m.Description,
                   ComplainId=m.Id
                
                  
                }).ToList();



            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
