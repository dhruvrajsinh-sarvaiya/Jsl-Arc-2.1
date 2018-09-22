using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IMessageConfiguration
    {
        // service type id - ex commmunication , 
        //communication service type id ex SMS ,
        //Communication service provider id ,
        //service id , 
        //APIid  
        //priority wise ,
        //Request format config type id (internally use)
        //Task GetAPIConfigurationAsync(long ServiceTypeID, long CommServiceTypeID,long CommSerproID, long CommServiceID, long APIId, int Priority);
        Task GetAPIConfigurationAsync(long ServiceTypeID, long CommServiceTypeID,long CommSerproID, int Priority);

        // service type id - ex commmunication , 
        //communication service type id ex SMS ,
        // Template ID
        Task GetTemplateConfigurationAsync(long ServiceTypeID, long CommServiceID,int TemplateID);
    }
}
