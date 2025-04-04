using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Lab2.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index() // Iaction result is an interface that multi class implemnet it like ViewResult , ContentResult , JsonResult so you can use it as a return type better
        {
            return View();
        }

        public ViewResult WaysToPassParametersToView()
        {
            // first way
            this.ViewData["Id"] = 10; // it's an inherited property from Controller class 
            // view also inherites from razor page a ViewData property and the view data in this controller will be sent automatically to the view 
            
            // second way
            // we have another property as View Data called View Bag and it's just a wrapper around viewData
            // so any value exists in view data will also be in view bag
            // but the differnece between them is ViewData as a dictionary in the key value pair the value is of type object
            // but view Bag it's value of type dynamic and the resovation for dynamic type is in runtime 
            // it means 
            // when you say ViewData["id"] + 20 // you will have an error at compile time because the resolvation will be done in compile time and won't wait for the runtime
            // but when with ViewBag["id"] + 20 // won't raise an error at compile time be cause the resolvation will wait till runtime 
            this.ViewBag.Id2 = 30; // you cannot write it like viewdata and view data cannot be written as viewBag
                                   // you can even remove this 

            // third way --> pass an object dirctly to the view in the parameter called model // the view function has more than one overload by the way it alse inheired from the controller class and it returns new ResultView() object you might return it dirctlty if you want
            // and the razor page has a property called Model --> and it has a dirctive @model you can specify in it the type of the model if you wanna be in the safe side
            var std = new Student() { Id = 1, Name = "Dod",Age =  34 };
            return View(std);
            // return new ViewResult(); // it's not preferred to return an object directly because functions add more logic before returnning
        }

        public IActionResult Create()
        {
            return this.View();
        }
        /*
        public ViewResult WaysToReceiveDataFromView()
        {
            // every controller has a property called request so when ever you pass anything to the server it would be in it
            // it has the query string propery it has the route property it has header property it has form propery

            // but note you will have that from the form if it used get method
            if (Request.Query.Count() > 0) { // note if it was a post method and you didn't use this if condidtion you will have an exception
                this.ViewBag.Id = int.Parse(Request.Query["Id"]);
                this.ViewBag.Name = Request.Query["Name"].ToString();
                this.ViewBag.Age = int.Parse(Request.Query["Age"]);
            }
            // but if you used post method you have to use the below
            else if(Request.Form.Count() > 0)
            {
                this.ViewBag.Id = int.Parse(Request.Form["Id"]);
                this.ViewBag.Name = Request.Form["Name"].ToString();
                this.ViewBag.Age = int.Parse(Request.Form["Age"]);
            }
            // but we has a way instead of all of that we have a model binder that handles that for us

            if (Request.RouteValues["id"] != null)
            { // if i send any id in the route it would overwrite the in the viewbag
                this.ViewBag.Id = int.Parse(Request.RouteValues["id"].ToString());
            }

            // but instead of all of that and doing the parsing by our hands this will be done automatically using model binder i will comment this function and go for the below
       
            return View();
        }
        */

        public ViewResult WaysToReceiveDataFromView(int Id, string Name, int Age)
        {
            // using model binder i will only write the parameter in the action and it will handle the rest by himself
            // but note // there's priorities 
            // form first --> routervalues second --> then query string --> so if you passed an id in all of them
            // it will go for form first and search there and if it finds it there it will use it directly
            // and that will be applied if the form of method post
            // if it was of method get them routervalues first then query string there's not form here

            this.ViewBag.Id = Id;
            this.ViewBag.Name = Name;
            this.ViewBag.Age = Age;

            return View();
        }

        // you can even use object directly like a student and he will bind the properties directly
        // if you are going to name more than one action with the same name try to distigush between them
        // buy annotation like one for post and the other for get
        [HttpPost]
        public ViewResult WaysToReceiveDataFromView(string[] departments, Student std)
        { // note in the form the names of the input controllers has to be the same as the properties in the 
            // student model
            // and if you're gonna send more than one object prefix the name of the input controllers by the object name here its std 
            // <input name="std.Id"> and so on 
            this.ViewBag.Id = std.Id;
            this.ViewBag.Name = std.Name;
            this.ViewBag.Age = std.Age;

            foreach (var department in departments)
            {
                this.ViewBag.Departments += department;
            }
            // and if you wanna send more than value like in select option in html you have to revice it in the action here in an array
            return View();
        }
    }
}
