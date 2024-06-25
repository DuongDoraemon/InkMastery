using Microsoft.AspNetCore.Http;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.StatisticRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataStatistics;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.Enumerates;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Application.ImplementServices
{
    public class StatisticService : IStatisticService
    {
        private readonly IBaseReposiroty<Bill> _billRepository;
        private readonly IBaseReposiroty<User> _userRepository;
        private readonly IBaseReposiroty<Team> _teamRepository;
        private readonly IBaseReposiroty<Customer> _customerRepository;
        private readonly IBaseReposiroty<Project> _projectRepository;
        public readonly UserConverter _userConverter;
        private readonly TeamConverter _teamConverter;
        private readonly IHttpContextAccessor _contextAccessor;
        public StatisticService(IBaseReposiroty<Bill> billRepository, IBaseReposiroty<User> userRepository, IBaseReposiroty<Team> teamRepository, IBaseReposiroty<Customer> customerRepository, IBaseReposiroty<Project> projectRepository, UserConverter userConverter, TeamConverter teamConverter, IHttpContextAccessor contextAccessor)
        {
            _billRepository = billRepository;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _customerRepository = customerRepository;
            _projectRepository = projectRepository;
            _userConverter = userConverter;
            _teamConverter = teamConverter;
            _contextAccessor = contextAccessor;
        }
        public async Task<IQueryable<DataResponseStatisticSalary>> GetStatisticSalary(Guid userId)
        {
            var user = await _userRepository.GetByIDAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var listProject = await _projectRepository.GetAllAsync(x => x.EmployeeCreateId == userId && x.Progress == 100);
            if (listProject == null || !listProject.Any())
            {
                return Enumerable.Empty<DataResponseStatisticSalary>().AsQueryable();
            }

            List<DataResponseStatisticSalary> result = new List<DataResponseStatisticSalary>();

            foreach (var project in listProject)
            {
                var listBill = await _billRepository.GetAllAsync(x => x.ProjectId == project.Id && x.BillStatus == BillStatusEnum.Paid);
                if (listBill == null || !listBill.Any())
                {
                    continue;
                }

                var groupedByMonthYear = listBill
                    .GroupBy(b => new { b.CreateTime.Month, b.CreateTime.Year })
                    .Select(g => new
                    {
                        Month = g.Key.Month,
                        Year = g.Key.Year,
                        Salary = g.Sum(b => b.TotalMoney)
                    });

                foreach (var group in groupedByMonthYear)
                {
                    var existingEntry = result.FirstOrDefault(r => r.Month == group.Month);
                    if (existingEntry != null)
                    {
                        existingEntry.Salary += group.Salary * project.CommissionPercentage;
                    }
                    else
                    {
                        result.Add(new DataResponseStatisticSalary
                        {
                            Month = group.Month,
                            Salary = group.Salary * project.CommissionPercentage,
                            User = _userConverter.EntityToDTOForUser(user)
                        });
                    }
                }
            }

            return result.AsQueryable();
        }


        public async Task<IQueryable<DataResponseStatisticSales>> GetStatisticSales(Request_StatisticSales request)
        {
            var query = await _billRepository.GetAllAsync(record => record.IsActive && record.BillStatus == BillStatusEnum.Paid);

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (request.StartTime.HasValue)
            {
                query = query.Where(record => record.CreateTime >= request.StartTime.Value);
            }
            if (request.EndTime.HasValue)
            {
                query = query.Where(record => record.CreateTime <= request.EndTime.Value);
            }

            var groupedByMonth = query
                .GroupBy(record => new { record.CreateTime.Year, record.CreateTime.Month })
                .Select(g => new DataResponseStatisticSales
                {
                    Month = g.Key.Month,
                    Sales = g.Sum(x => x.TotalMoney)
                });

            return groupedByMonth.AsQueryable();
        }



        public Task<IQueryable<DataResponseStatisticSalesOfUser>> GetStatisticSalesOfUserAsync(Guid userId, Request_StatisticSalesOfUser request)
        {
            throw new NotImplementedException();
        }
    }
}
