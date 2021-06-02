using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Controllers
{
    public class LoginController : Controller
    {
        private string project = "codebreakers-f72cc";
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"codebreakers-f72cc-firebase-adminsdk-mipsp-f44ddfe0b8.json";
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginProcessAsync(UserModel userModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            CollectionReference usersRef = db.Collection("Users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                if (userModel.Email.ToString().Equals(documentDictionary["Email"]) && userModel.Password.ToString().Equals(documentDictionary["Password"]))
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userModel.Email)
                    };

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                        );
                    var principal = new ClaimsPrincipal(identity);
                    var prop = new AuthenticationProperties();
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        prop
                        ).Wait();


                    switch ((string)documentDictionary["Role"])
                    {
                        case "student":
                            {
                                return RedirectToAction("Student", "Student" ,userModel);
                            }
                        case "admin":
                            {
                                return RedirectToAction("Admin", "Admin" ,userModel);
                            }
                        case "super":
                            {
                                return RedirectToAction("Super", "Super");
                            }
                        default:
                            break;
                    }
                    break;
                }
            }
            return RedirectToAction("Error","Home");
        }
    }
}
