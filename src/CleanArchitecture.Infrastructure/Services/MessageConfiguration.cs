using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Services
{
    public class MessageConfiguration : IMessageConfiguration
    {
        private readonly AppDbContext _dbContext;

        public MessageConfiguration(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task GetAPIConfigurationAsync(long ServiceTypeID, long CommServiceTypeID, long CommSerproID, int Priority)
        {
            using (AppDbContext ctx = new AppDbContext())
            {
                var result = from SM in ctx.ServiceTypeMaster
                             join CSTM in ctx.CommServiceTypeMaster on SM.ServiceTypeID equals CSTM.ServiceTypeID
                             join CSMP in ctx.CommServiceproviderMaster on CSTM.CommServiceTypeID equals CSMP.CommServiceTypeID
                             join CSM in ctx.CommServiceMaster on CSMP.CommSerproID equals CSM.CommSerproID
                             join CASM in ctx.CommAPIServiceMaster on CSM.CommServiceID equals CASM.CommServiceID
                             join RM in ctx.RequestFormatMaster on CSM.RequestID equals RM.RequestID
                             where SM.ServiceTypeID == ServiceTypeID  && CSTM.CommServiceTypeID == CommServiceTypeID 
                             && CSMP.CommSerproID == CommSerproID && CASM.Priority == priority
                             select new 
                             {
                                 CASM.SenderID,
                                 CASM.SMSSendURL,
                                 CASM.Priority,
                                 CASM.SMSBalURL,
                                 CSM.RequestID,
                                 RM.RequestFormat,
                                 RM.contentType,
                                 RM.MethodType,
                                 CSM.ServiceName,
                                 CSMP.UserID,
                                 CSMP.Password,
                                 CSMP.Balance
                             };
                return Task.FromResult(result);
            }                
            
        }

        public Task GetTemplateConfigurationAsync(long ServiceTypeID, long CommServiceID, int TemplateID)
        {
            return Task.FromResult(0);
        }
    }
}
