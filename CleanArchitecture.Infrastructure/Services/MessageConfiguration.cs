using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CleanArchitecture.Core.ApiModels;

namespace CleanArchitecture.Infrastructure.Services
{
    public class MessageConfiguration : IMessageConfiguration
    {
        private readonly CleanArchitectureContext _dbContext;

        public MessageConfiguration(CleanArchitectureContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<IQueryable> GetAPIConfigurationAsync(long ServiceTypeID, long CommServiceTypeID)
        {
            IQueryable result = from SM in _dbContext.ServiceTypeMaster
                             join CSTM in _dbContext.CommServiceTypeMaster on SM.ServiceTypeID equals CSTM.ServiceTypeID
                             join CSMP in _dbContext.CommServiceproviderMaster on CSTM.CommServiceTypeID equals CSMP.CommServiceTypeID
                             join CSM in _dbContext.CommServiceMaster on CSMP.CommSerproID equals CSM.CommSerproID
                             join CASM in _dbContext.CommAPIServiceMaster on CSM.CommServiceID equals CASM.CommServiceID
                             join RM in _dbContext.RequestFormatMaster on CSM.RequestID equals RM.RequestID
                             where SM.ServiceTypeID == ServiceTypeID && CSTM.CommServiceTypeID == CommServiceTypeID
                             select new  CommunicationProviderList
                             {
                                 SenderID = CASM.SenderID,
                                 SMSSendURL = CASM.SMSSendURL,
                                 Priority = CASM.Priority,
                                 SMSBalURL = CASM.SMSBalURL,
                                 RequestID = CSM.RequestID,
                                 RequestFormat = RM.RequestFormat,
                                 contentType = RM.contentType,
                                 MethodType = RM.MethodType,
                                 ServiceName = CSM.ServiceName,
                                 UserID = CSMP.UserID,
                                 Password = CSMP.Password,
                                 Balance = CSMP.Balance
                             };
                return Task.FromResult(result);
            
            //return null;
            
        }

        public Task GetTemplateConfigurationAsync(long ServiceTypeID, long CommServiceID, int TemplateID)
        {
            return Task.FromResult(0);
        }
    }
}
