using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatisBasePay.Entities;
using DatisBasePay.Services;
using DatisBasePay.ViewModels;

namespace DatisBasePay.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private IEmployeeData _employeeData;

		public HomeController(IEmployeeData employeeData)
		{
			ViewBag.Title = "Datis Base Pay";
			_employeeData = employeeData;
		}

		public IActionResult Index()
		{
			var model = new HomePageViewModel();
			return View(model);
		}

		[HttpGet]
		[Route("Employees")]
		public IActionResult Get()
		{
			var model = new EmployeeViewModel();
			model.Employees = _employeeData.GetAll();
			return Json(model);
		}

		[HttpGet]
		[Route("/Employee/{id}")]
		public IActionResult Get(int id)
		{
			var model = _employeeData.Get(id);
			return Json(model);
		}

		[HttpPost]
		[Route("Employee")]
		public IActionResult Post([FromBody]EmployeeViewModel model)
		{
			if (ModelState.IsValid)
			{
				var employee = new Employee();
				employee.EmployeeName = model.EmployeeName;
				employee.Salary = model.Salary;
				employee.TaxRate = model.TaxRate;
				employee.Contribution = model.Contribution;
				employee.StateOfResidence = model.StateOfResidence;
				employee = _employeeData.Add(employee);
				_employeeData.Commit();
				return Json(employee);
			}
			return View(model);
		}

		[HttpPut]
		[Route("Employee")]
		public IActionResult Update(EmployeeViewModel model)
		{
			var employee = _employeeData.Get(model.Id);
			if (ModelState.IsValid)
			{
				employee.EmployeeName = model.EmployeeName;
				employee.Salary = model.Salary;
				employee.TaxRate = model.TaxRate;
				employee.StateOfResidence = model.StateOfResidence;
				_employeeData.Update(employee);
				_employeeData.Commit();
			}
			return View(employee);
		}

		[HttpDelete]
		[Route("/Employee/{id}")]
		public IActionResult Delete(int id)
		{
			var employee = _employeeData.Get(id);
			_employeeData.Delete(employee);
			_employeeData.Commit();
			return Json(employee);
		}
	}
}
