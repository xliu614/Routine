using Routine.Api.DtoParems;
using Routine.Api.Entities;

namespace Routine.Api.Services
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Get the list of the companies
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters);
        /// <summary>
        /// Get the list of the companies by the list of Guid
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        /// <summary>
        /// Get a company by its Guid
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<Company> GetCompanyAsync(Guid companyId);
        /// <summary>
        /// Add a company
        /// </summary>
        /// <param name="company"></param>
        void AddCompany(Company company);
        /// <summary>
        /// Update a company
        /// </summary>
        /// <param name="company"></param>
        void UpdateCompany(Company company);
        /// <summary>
        /// Remove a company
        /// </summary>
        /// <param name="company"></param>
        void DeleteCompany(Company company);
        /// <summary>
        /// Get if the company is already in the system
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<bool> CompanyExistsAsync(Guid companyId);

        /// <summary>
        /// Get all the employees in a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string? genderDisplay, string? q);
        /// <summary>
        /// Get a specific employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId);
        /// <summary>
        /// Add employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employee"></param>
        void AddEmployee(Guid companyId, Employee employee);
        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="employee"></param>
        void UpdateEmployee(Employee employee);
        /// <summary>
        /// Remove employee
        /// </summary>
        /// <param name="employee"></param>
        void DeleteEmployee(Employee employee);

        Task<bool> SaveAsync();
   
    }
}
