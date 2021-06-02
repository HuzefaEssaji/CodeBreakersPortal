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
    public class AdminController : Controller
    {
        private string project = "codebreakers-f72cc";
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"codebreakers-f72cc-firebase-adminsdk-mipsp-f44ddfe0b8.json";
        private AdminModel adminModel = new();
        public async Task<IActionResult> AdminAsync(UserModel userModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            CollectionReference usersRef = db.Collection("Admin");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();


            foreach (var item in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = item.ToDictionary();
                if (userModel.Email.Equals((string)documentDictionary["Email"]))
                {
                    adminModel.Name = (string)documentDictionary["Name"];
                    adminModel.Email = (string)documentDictionary["Email"];
                    adminModel.Department = (string)documentDictionary["Department"];
                    adminModel.PhoneNo = (string)documentDictionary["PhoneNo"];
                    return View(adminModel);
                }
            }
            return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> UpdateTaskAsync(AdminModel admin)
        {
            AdminModel adminModel = new();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            CollectionReference usersRef = db.Collection("Student");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();


            CollectionReference usersRefs = db.Collection("Admin");
            QuerySnapshot snapshots = await usersRefs.GetSnapshotAsync();


            foreach (var item in snapshots.Documents)
            {
                Dictionary<string, object> documentDictionary = item.ToDictionary();
                if ( documentDictionary.ContainsKey("Email") && admin.Email.Equals((string)documentDictionary["Email"]) )
                {
                    adminModel.Name = (string)documentDictionary["Name"];
                    adminModel.Email = (string)documentDictionary["Email"];
                    adminModel.Department = (string)documentDictionary["Department"];
                    adminModel.PhoneNo = (string)documentDictionary["PhoneNo"];
                    break;
                }
            }




            foreach (var item in snapshot.Documents)
            {
                DocumentReference docRef = db.Collection("Student").Document(admin.StudentEmail);
                Dictionary<string, object> documentDictionary = item.ToDictionary();
                if ( documentDictionary["Email"].ToString().Equals(admin.StudentEmail) && documentDictionary.ContainsKey("Task"))
                {
                    Dictionary<string, object> newTask = new Dictionary<string, object>
                        {
                            { "Task", admin.UpdateTask },
                        };
                    await docRef.SetAsync(newTask, SetOptions.MergeAll);
                    return View("Admin", adminModel);
                }
            }

            return RedirectToAction("Error", "Home");
        }
    }
}
