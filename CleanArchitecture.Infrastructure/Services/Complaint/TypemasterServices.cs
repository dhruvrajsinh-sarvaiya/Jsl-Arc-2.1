using CleanArchitecture.Core.Entities.Complaint;
using CleanArchitecture.Core.Interfaces.Complaint;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Complaint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Complaint
{
    public class TypemasterServices : ITypemaster
    {
        private readonly ICustomRepository<Typemaster> _TypemasterRepository;
        public TypemasterServices(ICustomRepository<Typemaster> TypemasterRepository)
        {
            _TypemasterRepository = TypemasterRepository;
        }
        public List<TypemasterViewModel> GettypeMaster(string Type)
        {
            try
            {
                var IpHistoryList = _TypemasterRepository.Table.Where(i => i.Type == Type).ToList();
                if (IpHistoryList == null)
                {
                    return null;
                }
                var LoginHistory = new List<TypemasterViewModel>();
                foreach (var item in IpHistoryList)
                {
                    TypemasterViewModel loginhistoryViewModel = new TypemasterViewModel()
                    {
                        Type = item.Type,
                        EnableStatus=item.EnableStatus,
                        SubType=item.SubType
                       };
                    LoginHistory.Add(loginhistoryViewModel);
                }
                return LoginHistory;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }

           
        }
    }
}
