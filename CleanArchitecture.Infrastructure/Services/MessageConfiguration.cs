﻿using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CleanArchitecture.Core.ApiModels;
using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Data.Entity;

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
            //IQueryable result = from SM in _dbContext.ServiceTypeMaster
            //                    join CSTM in _dbContext.CommServiceTypeMaster on SM.ServiceTypeID equals CSTM.ServiceTypeID
            //                    join CSMP in _dbContext.CommServiceproviderMaster on CSTM.CommServiceTypeID equals CSMP.CommServiceTypeID
            //                    join CSM in _dbContext.CommServiceMaster on CSMP.CommSerproID equals CSM.CommSerproID
            //                    join CASM in _dbContext.CommAPIServiceMaster on CSM.CommServiceID equals CASM.CommServiceID
            //                    join RM in _dbContext.RequestFormatMaster on CSM.RequestID equals RM.RequestID
            //                    where SM.ServiceTypeID == ServiceTypeID && CSTM.CommServiceTypeID == CommServiceTypeID
            //                    select new CommunicationProviderList
            //                    {
            //                        SenderID = CASM.SenderID,
            //                        SMSSendURL = CASM.SMSSendURL,
            //                        Priority = CASM.Priority,
            //                        SMSBalURL = CASM.SMSBalURL,
            //                        RequestID = CSM.RequestID,
            //                        RequestFormat = RM.RequestFormat,
            //                        contentType = RM.contentType,
            //                        MethodType = RM.MethodType,
            //                        ServiceName = CSM.ServiceName,
            //                        UserID = CSMP.UserID,
            //                        Password = CSMP.Password,
            //                        Balance = CSMP.Balance
              //                  };
            // return Task.FromResult(result);

            IQueryable Result = _dbContext.CommunicationProviderList.FromSql(
                    @"select CASM.SenderID,CASM.SMSSendURL As SendURL,CASM.Priority,CASM.SMSBalURL,CSM.RequestID, RM.RequestFormat,
                        RM.ContentType,RM.MethodType, CSM.ServiceName,CSMP.UserID, CSMP.Password,CSMP.Balance ,CSM.CommServiceID,
                        ISNull(CSM.ResponseFailure,'') AS ResponseFailure ,ISNull(CSM.ResponseSuccess,'') AS ResponseSuccess ,ISNULL(TC.StatusRegex,'') AS StatusRegex,
                        ISNull(TC.StatusMsgRegex,'') AS StatusMsgRegex,ISNull(TC.BalanceRegex,'') AS BalanceRegex,ISNull(TC.ErrorCodeRegex,'') AS ErrorCodeRegex,
                        ISNull(TC.OprTrnRefNoRegex,'') AS OprTrnRefNoRegex,ISNull(TC.TrnRefNoRegex,'') AS TrnRefNoRegex,ISNull(TC.ResponseCodeRegex,'') AS ResponseCodeRegex,ISNull(TC.Param1Regex,'') AS Param1Regex,ISNull(TC.Param2Regex,'') AS Param2Regex,ISNull(TC.Param3Regex,'') AS Param3Regex
                        from ServiceTypeMaster SM
                        inner join CommServiceTypeMaster CSTM on SM.ServiceTypeID = CSTM.ServiceTypeID
                        inner join CommServiceproviderMaster CSMP on CSTM.CommServiceTypeID = CSMP.CommServiceTypeID
                        inner join CommServiceMaster CSM on CSMP.CommSerproID = CSM.CommSerproID
                        inner join CommAPIServiceMaster CASM on CSM.CommServiceID = CASM.CommServiceID
                        inner join RequestFormatMaster RM on CSM.RequestID = RM.RequestID
                        left join ThirdPartyAPIResponseConfiguration TC on TC.Id = CSM.ParsingDataID
                        where SM.ServiceTypeID = {0} and CSTM.CommServiceTypeID = {1}", ServiceTypeID, CommServiceTypeID);

            return Task.FromResult(Result);
        }
        //enCommunicationServiceType == ServiceTypeID
        //EnTemplateType === TemplateID
        // currently not used CommServiceID
        public Task<IQueryable> GetTemplateConfigurationAsync(long ServiceTypeID, int TemplateID, long CommServiceID = 0)
        {
            IQueryable Result = _dbContext.TemplateMasterData.FromSql(
                    @"select Top 1 Content,AdditionalInfo from TemplateMaster TM inner join CommServiceTypeMaster ST on ST.CommServiceTypeID = TM.CommServiceTypeID where TemplateID = {0} and ST.CommServiceTypeID = {1} and TM.status = 1", TemplateID, ServiceTypeID);
            return Task.FromResult(Result);
        }
    }
}
