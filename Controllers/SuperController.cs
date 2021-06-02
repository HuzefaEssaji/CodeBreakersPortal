using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Controllers
{
    [Authorize]
    public class SuperController : Controller
    {
        private string project = "codebreakers-f72cc";
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"codebreakers-f72cc-firebase-adminsdk-mipsp-f44ddfe0b8.json";

        public IActionResult Super()
        {
            return View();
        }

        public async Task<IActionResult> SuperRouteAsync(UserModel userModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            DocumentReference docRef = db.Collection("Users").Document(userModel.Email);

            Dictionary<string, object> newUser = new()
            {
                { "Name", userModel.Email },
                { "Password", userModel.Password },
                { "Role", userModel.Role },
            };
            await docRef.SetAsync(newUser, SetOptions.MergeAll);


            if (userModel.Role.ToLower().Equals("admin"))
            {
                return View("CreateAdmin");
            }
            else if (userModel.Role.ToLower().Equals("student"))
            {
                return View("CreateStudent");
            }
            return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> CreateAdminAsync(AdminModel adminModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            DocumentReference docRef = db.Collection("Admin").Document(adminModel.Email);

            Dictionary<string, object> newAdmin = new()
            {
                { "Name", adminModel.Name },
                { "Email", adminModel.Email },
                { "PhoneNo", adminModel.PhoneNo },
                { "Department", adminModel.Department },
            };
            await docRef.SetAsync(newAdmin, SetOptions.MergeAll);
            return RedirectToAction("Super");
        }

        public async Task<IActionResult> CreateStudentAsync(StudentModel studentModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            DocumentReference docRef = db.Collection("Student").Document(studentModel.Email);

            Dictionary<string, object> newStudent = new()
            {
                { "Name", studentModel.Name },
                { "Email", studentModel.Email },
                { "PhoneNo", studentModel.PhoneNo },
                { "Task", studentModel.Task },
                { "Department", studentModel.Department },
            };
            await docRef.SetAsync(newStudent, SetOptions.MergeAll);
            return RedirectToAction("Super");
        }
    }
}
