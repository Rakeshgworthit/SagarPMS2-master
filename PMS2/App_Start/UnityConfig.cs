using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using PMS.Repository.Infrastructure;
using PMS.Repository.DataService;

namespace PMS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<PMS.Controllers.AccountController>(new InjectionConstructor());
            container.RegisterType<PMS.Controllers.ManageController>(new InjectionConstructor());

            container.RegisterType<IuserRepository, UserRepository>();
            container.RegisterType<ICustomerRepositor, CustomerService>();
            container.RegisterType<ISupplierRepositor, SupplierService>();
            container.RegisterType<IProject, ProjectService>();
            container.RegisterType<IProjectAdditionRepository, ProjectAdditionService>();
            container.RegisterType<IPaymentsRepositor, PaymentsService>();
            container.RegisterType<IReceiptsRepositor, ReceiptsService>();
            container.RegisterType<IPaymentDetailRepository, PaymentDetailService>();

            container.RegisterType<IPaymentDetailsDescription, PaymentDetailsDescriptionService>();

            container.RegisterType<ICompanyRepositor, CompanyService>();
            container.RegisterType<IBanksRepositor, BanksService>();
            container.RegisterType<IBranchesRepositor, BranchesService>();
            container.RegisterType<ISalesmenRepositor, SalesmenService>();
            container.RegisterType<ILoanRepositor, LoanService>();

            container.RegisterType<IProjectsBudgetRepository, ProjectsBudgetService>();
            container.RegisterType<IProjectsBudgetDetailRepository, ProjectsBudgetDetailsService>();

            container.RegisterType<IDiscountRepository, DiscountService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}