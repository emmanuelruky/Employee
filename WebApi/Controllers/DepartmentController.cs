using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        public DepartmentController(IConfiguration configuraton)
        {
            _Configuration = configuraton;
        }

        [HttpGet]
        public JsonResult Get()
        { 
        string Query = @"select Id, Name from dbo.tbl_Depertment";
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
        public JsonResult Post(Department dep)
        {
            string Query = @"insert into dbo.tbl_Depertment values (@Name)";
            DataTable Table = new DataTable();
            string SqlDatasource = _Configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection MyCon = new SqlConnection(SqlDatasource))
            {
                MyCon.Open();
                using (SqlCommand MyCom = new SqlCommand(Query, MyCon))
                {
                    MyCom.Parameters.AddWithValue("@Name", dep.Name);
                    myReader = MyCom.ExecuteReader();
                    Table.Load(myReader);
                    myReader.Close();
                    MyCon.Close();
                }
            }

            return new JsonResult("Created Successful");
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string Query = @"update dbo.tbl_Depertment set Name=@Name where Id = @Id";
            DataTable Table = new DataTable();
            string SqlDatasource = _Configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection MyCon = new SqlConnection(SqlDatasource))
            {
                MyCon.Open();
                using (SqlCommand MyCom = new SqlCommand(Query, MyCon))
                {
                    MyCom.Parameters.AddWithValue("@Id", dep.Id);
                    MyCom.Parameters.AddWithValue("@Name", dep.Name);
                    myReader = MyCom.ExecuteReader();
                    Table.Load(myReader);
                    myReader.Close();
                    MyCon.Close();
                }
            }

            return new JsonResult("Updated Successful");
        }

        [HttpDelete]
        public JsonResult Delete(int Id)
        {
            string Query = @"Delete dbo.tbl_Depertment where Id = @Id";
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

            return new JsonResult("Deleted Successful");
        }
    }
}
