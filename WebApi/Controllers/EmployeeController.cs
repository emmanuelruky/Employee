using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WebApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private readonly IWebHostEnvironment _Env;
        public EmployeeController(IConfiguration configuraton, IWebHostEnvironment Env)
        {
            _Configuration = configuraton;
            _Env = Env; 
        }
        [HttpGet]
        public JsonResult Get()
        {
            string Query = @"select Id, Name, Department, Convert (varchar(10), DateOfJoing, 120) as DateOfJoing, PhotoFileName from dbo.tbl_Employee";
            DataTable Table = new DataTable();
            string SqlDatasource = _Configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection MyCon = new SqlConnection(SqlDatasource))
            {
                MyCon.Open();
                using (SqlCommand MyCom = new SqlCommand(Query, MyCon))
                {
                    myReader = MyCom.ExecuteReader();
                    Table.Load(myReader);
                    myReader.Close();
                    MyCon.Close();
                }
            }

            return new JsonResult(Table);
        }
        [HttpPost]
        public JsonResult Post(Employee Emp)
        {
            string Query = @"insert into dbo.tbl_Employee (Name, Department,DateOfJoing,PhotoFileName) values (@Name, @Department,@DateOfJoing,@PhotoFileName)";
            DataTable Table = new DataTable();
            string SqlDatasource = _Configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection MyCon = new SqlConnection(SqlDatasource))
            {
                MyCon.Open();
                using (SqlCommand MyCom = new SqlCommand(Query, MyCon))
                {
                    MyCom.Parameters.AddWithValue("@Name", Emp.Name);
                    MyCom.Parameters.AddWithValue("@Department", Emp.Department);
                    MyCom.Parameters.AddWithValue("@DateOfJoing", Emp.DateOfJoining);
                    MyCom.Parameters.AddWithValue("@PhotoFileName", Emp.PhotoFileName);
                    myReader = MyCom.ExecuteReader();
                    Table.Load(myReader);
                    myReader.Close();
                    MyCon.Close();
                }
            }

            return new JsonResult("Created Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee Emp)
        {
            string Query = @"update dbo.tbl_Employee set 
                                                Name=@Name, 
                                                Deparment=@Department,
                                                DateOfJoing=@DateOfJoing,
                                                PhotoFileName=@PhotoFileName 
                                                where Id = @Id";
            DataTable Table = new DataTable();
            string SqlDatasource = _Configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection MyCon = new SqlConnection(SqlDatasource))
            {
                MyCon.Open();
                using (SqlCommand MyCom = new SqlCommand(Query, MyCon))
                {
                    MyCom.Parameters.AddWithValue("@Id", Emp.ID);
                    MyCom.Parameters.AddWithValue("@Name", Emp.Name);
                    MyCom.Parameters.AddWithValue("@Department", Emp.Department);
                    MyCom.Parameters.AddWithValue("@DateOfJoing", Emp.DateOfJoining);
                    MyCom.Parameters.AddWithValue("@PhotoFileName", Emp.PhotoFileName);
                    myReader = MyCom.ExecuteReader();
                    Table.Load(myReader);
                    myReader.Close();
                    MyCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete]
        public JsonResult Delete(int Id)
        {
            string Query = @"Delete dbo.tbl_Employee where Id = @Id";
            DataTable Table = new DataTable();
            string SqlDatasource = _Configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection MyCon = new SqlConnection(SqlDatasource))
            {
                MyCon.Open();
                using (SqlCommand MyCom = new SqlCommand(Query, MyCon))
                {

                    MyCom.Parameters.AddWithValue("Id", Id);
                    myReader = MyCom.ExecuteReader();
                    Table.Load(myReader);
                    myReader.Close();
                    MyCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string FileName = postedFile.FileName;
                var PhysicalPath = _Env.ContentRootPath + "/Photo" + FileName;

                using (var stream=new FileStream(PhysicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(FileName);
            }
            catch(Exception)
            {
                return new JsonResult("anonymose.png");
            }
        }
    }
}
