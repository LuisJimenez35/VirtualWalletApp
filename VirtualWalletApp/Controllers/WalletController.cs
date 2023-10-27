using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using VirtualWalletApp.Database;
using VirtualWalletApp.Models;

namespace VirtualWalletApp.Controllers
{
    public class WalletController : Controller
    {
        public ActionResult WalletIndex(string userEmail)
        {
            // Obtener la lista de tarjetas utilizando el método en LoginController
            List<DataTarjet> tarjetas = GetTarjetas(userEmail);

            // Agregar la lista al ViewBag
            ViewBag.Tarjetas = tarjetas;

            ViewBag.UserEmail = userEmail;

            return View();
        }

        private List<DataTarjet> GetTarjetas(string userEmail)
        {
            // Obtener la DataTable de tarjetas utilizando el método en LoginController
            DataTable tarjetasData = GetTarjetDataByEmail(userEmail);

            // Convertir la DataTable a una lista de objetos DataTarjet
            List<DataTarjet> tarjetas = new List<DataTarjet>();
            foreach (DataRow dr in tarjetasData.Rows)
            {
                tarjetas.Add(new DataTarjet
                {
                    Logo = dr["Logo"].ToString(),
                    Bank = dr["Bank"].ToString(),
                    Emitter = dr["Emitter"].ToString(),
                    OwnerName = dr["OwnerName"].ToString(),
                    TarjetNumber = dr["TarjetNumber"].ToString(),
                    CVV = dr["CVV"].ToString(),
                    ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"]),
                    ID = Convert.ToInt32(dr["ID"]),
                    OwnerDNI = Convert.ToInt32(dr["OwnerDNI"])                 
                });
            }

            return tarjetas;
        }
        private DataTable GetTarjetDataByEmail(string userEmail)
        {
            var queryParameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", userEmail)
            };

            return DatabaseHelper.ExecuteQuery("GetTarjetDataByEmail", queryParameters);
        }

        public IActionResult CreateTarjet(string Bank, string Emitter, string OwnerName, string TarjetNumber, string CVV, DateTime ExpirationDate, string userEmail, string Logo, string OwnerDNI)
        {
            int ownerDNINumber = Convert.ToInt32(OwnerDNI);

            var queryParameters = new List<SqlParameter>
            {
                new SqlParameter("@Bank", Bank),
                new SqlParameter("@Emitter", Emitter),
                new SqlParameter("@OwnerName", OwnerName),
                new SqlParameter("@TarjetNumber", TarjetNumber),
                new SqlParameter("@CVV", CVV),
                new SqlParameter("@ExpirationDate", ExpirationDate),
                new SqlParameter("@Logo", "/WalletLogo/" + Logo),
                new SqlParameter("@OwnerDNI", ownerDNINumber)
            };

            DatabaseHelper.ExecuteQuery("CreateTarjet", queryParameters);

            return RedirectToAction("WalletIndex", "Wallet", new { userEmail = userEmail });
        }

        public IActionResult UpdateTarjet(string Bank, string Emitter, string OwnerName, string TarjetNumber, string CVV, DateTime ExpirationDate, string userEmail, string Logo,string ID , string OwnerDNI)
        {
            int ownerDNINumber = Convert.ToInt32(OwnerDNI);
            

            var queryParameters = new List<SqlParameter>
            {
                new SqlParameter("@Bank", Bank),
                new SqlParameter("@Emitter", Emitter),
                new SqlParameter("@OwnerName", OwnerName),
                new SqlParameter("@TarjetNumber", TarjetNumber),
                new SqlParameter("@CVV", CVV),
                new SqlParameter("@ExpirationDate", ExpirationDate),
                new SqlParameter("@Logo", "/WalletLogo/" + Logo),
                new SqlParameter("@ID", ID),
                new SqlParameter("@OwnerDNI", ownerDNINumber)
                
            };

            DatabaseHelper.ExecuteQuery("UpdateTarjet", queryParameters);

            return RedirectToAction("WalletIndex", "Wallet", new { userEmail = userEmail });

        }

        public IActionResult DeleteTarjet(string ID, string userEmail)
        {
            var queryParameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", ID)
            };

            DatabaseHelper.ExecuteQuery("DeleteTarjet", queryParameters);

            return RedirectToAction("WalletIndex", "Wallet", new { userEmail = userEmail });
        }

    }

}

 