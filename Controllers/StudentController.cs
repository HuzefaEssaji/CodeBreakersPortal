using Firebase.Auth;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Source.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Source.Controllers
{

    public class StudentController : Controller
    {
        private static string apiKey = "AIzaSyC7evcDGEfTe14TNKbh_F9EIeVE_7Yaz6A";
        private static string Bucket = "codebreakers-f72cc.appspot.com";
        private static string AuthEmail = "thecodebreakers.rcoem@gmail.com";
        private static string AuthPassword = "Thecodebreakers#rcoem";

        private string project = "codebreakers-f72cc";
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"codebreakers-f72cc-firebase-adminsdk-mipsp-f44ddfe0b8.json";
        public async Task<IActionResult> StudentAsync(UserModel userModel)
        {
            if (Authorization.isStudent)
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                FirestoreDb db = FirestoreDb.Create(project);

                CollectionReference usersRef = db.Collection("Student");
                QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

                StudentModel studentModel = new();

                foreach (var item in snapshot.Documents)
                {
                    Dictionary<string, object> documentDictionary = item.ToDictionary();
                    if (userModel.Email.Equals((string)documentDictionary["Email"]))
                    {
                        studentModel.Email = (string)documentDictionary["Email"];
                        studentModel.Name = (string)documentDictionary["Name"];
                        studentModel.PhoneNo = (string)documentDictionary["PhoneNo"];
                        studentModel.Department = (string)documentDictionary["Department"];
                        studentModel.Task = (string)documentDictionary["Task"];

                        return View(studentModel);
                    }
                } 
            }

            return RedirectToAction("Error","Home");
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> UploadAsync(List<IFormFile> files)
        {
            string email = Request.Form["email"] + "\\";

            long size = files.Sum(f => f.Length);
            string filePath = null;

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", email); //we are using Temp file name just for the example. Add your own file path.
                    filePaths.Add(filePath);
                    DirectoryInfo di = Directory.CreateDirectory(filePath);
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", email ,formFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    var stream1 = new FileStream(filePath, FileMode.Open);
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail,AuthPassword);
                    var cancellation = new CancellationTokenSource();
                    var task = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                        })
                    .Child("Data")
                    .Child(email)
                    .Child(formFile.FileName)
                    .PutAsync(stream1, cancellation.Token);
                    ViewBag.link = await task;
                }
            }

            return RedirectToAction("LogOut","Login");
        }

    }
}
