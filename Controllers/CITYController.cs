using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using testdata.models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using testdata.Models;

namespace testdata.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CITYController : ControllerBase
    {
        private readonly ConnectionStrings connectionStrings;

        public CITYController(IOptions<ConnectionStrings> connectionStrings)
        {
            this.connectionStrings = connectionStrings.Value;
        }
        //            select city_num,city_name_a,city_name_e from main.cr_city
        [HttpGet]
        public IEnumerable<CITY> Get()
        {
            return GetCity("");
        }
        [HttpGet("{city_name}")]
        public IEnumerable<CITY> Get(string city_name)
        {
            return GetCity(city_name);
        }
        public IEnumerable<CITY> GetCity(string city_name)
        {
            List<CITY> myList = new List<CITY>();
            SqlConnection connection = new SqlConnection(this.connectionStrings.MyDefaultConnection);
            //connection.Open();
            if (string.IsNullOrWhiteSpace(city_name)) city_name = "%%";
            else city_name = "%" + city_name + "%";
            SqlDataAdapter da = new SqlDataAdapter("select city_num,city_name from main.cr_city where city_name_a like @city_name", connection);
            da.SelectCommand.Parameters.Add(new SqlParameter("@city_name",city_name));
            DataSet ds=new DataSet();
            da.Fill(ds);
            if (ds.Tables.Count > 0) { 
                for(int i=0;i< ds.Tables[0].Rows.Count; i++)
                {
                    CITY CITY = new CITY();
                    CITY.city_num = int.Parse(ds.Tables[0].Rows[i]["city_num"].ToString());
                    CITY.city_name = ds.Tables[0].Rows[i]["city_name"].ToString();
                    myList.Add(CITY);
                }
            }
            return myList;
        }
    }
}