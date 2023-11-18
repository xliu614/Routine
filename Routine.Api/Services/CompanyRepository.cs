using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.Entities;

namespace Routine.Api.Services
{
    public class CompanyRepository:ICompanyRepository
    {
        #region ctor
        private readonly RoutineDbContext _context;
        public CompanyRepository(DbContext context)
        {
            _context = (RoutineDbContext?)context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Company
        /// <summary>
        /// Get Company list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }
        /// <summary>
        /// Get a single company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));
            return await _context.Companies.FirstOrDefaultAsync(predicate: x => x.Id == companyId);
        }
        /// <summary>
        /// Get multiple companies from a collection of company ids
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
                throw new ArgumentNullException(nameof(companyIds));
            return await _context.Companies.Where(x => companyIds.Contains(x.Id)).
                   OrderBy(x => x.Name).ToListAsync();
        }
        /// <summary>
        /// Add a company to the system
        /// </summary>
        /// <param name="company"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddCompany(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));
            company.Id = Guid.NewGuid();
            foreach (var employee in company.Employees) {
                employee.Id = Guid.NewGuid();
            }
            _context.Companies.Add(company);
        }
        /// <summary>
        /// No need to implement this
        /// </summary>
        /// <param name="company"></param>
        public void UpdateCompany(Company company) {
        }
        public void DeleteCompany(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));
            _context.Companies.Remove(company);
        }
        /// <summary>
        /// Get to know if a company exists in the system
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if(companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }
        #endregion

        #region Employee
        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId));
            return await _context.Employees.FirstOrDefaultAsync(x => x.CompanyId == companyId && x.Id == employeeId);
        }
        /// <summary>
        /// List all the employees of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
               throw new ArgumentNullException(nameof(companyId));
            return await _context.Employees.
                   Where(x => x.CompanyId == companyId)
                   .OrderBy(x => x.EmployeeNo)
                   .ToListAsync();
        }
        public void AddEmployee(Guid companyId, Employee employee)
        {
            if(companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));
            if(employee == null)
                throw new ArgumentNullException(nameof(employee)); 
            employee.CompanyId = Guid.NewGuid();
            _context.Employees.Add(employee);
        }        

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public void UpdateEmployee(Employee employee)        {
            
        }
        #endregion
        #region Other operations
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
        #endregion
    }
}
