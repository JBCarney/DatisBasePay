using Microsoft.AspNetCore.Mvc;

namespace DatisBasePay.ViewComponents
{
	public class LoginLogoutViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
