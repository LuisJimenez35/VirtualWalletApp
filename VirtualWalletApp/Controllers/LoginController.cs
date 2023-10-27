using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using VirtualWalletApp.Database;
using VirtualWalletApp.Models;

namespace VirtualWalletApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult LoginIndex()
        {
            return View();
        }

        public ActionResult Login(string Email, string password)
        {
            int validacionResult = ValidateLoginUsers(Email, password);

            switch (validacionResult)
            {
                case 1:
                    return RedirectToAction("WalletIndex", "Wallet", new { userEmail = Email });
                case 0:
                    return RedirectToAction("LoginIndex", "Login");
                default:
                    return RedirectToAction("LoginIndex", "Login");
            }
        }

        private int ValidateLoginUsers(string Email, string password)
        {
            var queryPatameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", Email),
                new SqlParameter("@Password", password)
            };

            var Result = DatabaseHelper.ExecuteQuery("ValidateLoginData", queryPatameters);

            if (Result.Rows.Count == 1)
            {
                return Convert.ToInt32(Result.Rows[0]["Result"]);
            }
            return -2;
        }

        

    }
}
