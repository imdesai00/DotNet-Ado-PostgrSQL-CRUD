using Npgsql;
using Npgsql.Internal;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace adocursorpostgre
{
    public class Npgdb
    {


        public void insertdatasp(int studentid, string firstname, string lastname, int age, string gender, double gread)
        {

            using (var con = GetConnection())
            {
                NpgsqlCommand insert = new NpgsqlCommand("call insertdata(@astudentid,@afirstname,@alastname,@aage,@agender,@agrade)", con);
                insert.Parameters.AddWithValue("@astudentid", studentid);
                insert.Parameters.AddWithValue("@afirstname", firstname);
                insert.Parameters.AddWithValue("@alastname", lastname);
                insert.Parameters.AddWithValue("@aage", age);
                insert.Parameters.AddWithValue("@agender", gender);
                insert.Parameters.AddWithValue("@agrade", gread);
                con.Open();
                insert.ExecuteNonQuery();
            }
        }
        public void deletedatasp(int studentid)
        {
            using (var con = GetConnection())
            {
                NpgsqlCommand delete = new NpgsqlCommand("call deletedata(:astudentid)", con);
                delete.Parameters.AddWithValue("astudentid", DbType.Int16).Value = studentid;
                delete.CommandType = CommandType.Text;
                con.Open();
                delete.ExecuteNonQuery();
            }
        }
        public void updatedatasp(int studentid, string firstname, string lastname)
        {
            using (var con = GetConnection())
            {
                NpgsqlCommand update = new NpgsqlCommand("call updatedata(@astudentid,@afirstname,@alastname)", con);
                update.Parameters.AddWithValue("@astudentid", studentid);
                update.Parameters.AddWithValue("@afirstname", firstname);
                update.Parameters.AddWithValue("@alastname", lastname);
                con.Open();
                update.ExecuteNonQuery();
            }
        }
        public void selectdatabytablemethod()
        {

            using (var con = GetConnection())
            {

                con.Open();
                //using simple method in function
                string selectt = "select * from selectdatat();";
                NpgsqlCommand selectcmd = new NpgsqlCommand(selectt, con);
                NpgsqlDataReader dr = selectcmd.ExecuteReader();
                while (dr.Read())
                    Console.Write("{0}\t{1}\t{2}\t{3}\t{4}\t{5} \n", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5]);


                // using datatbale function
                string selecta = "select * from selectdatat();";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(selecta, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dra in dt.Rows)
                {
                    Console.Write("{0}\t{1}\t{2}\t{3}\t{4}\t{5} \n", dra[0], dra[1], dra[2], dra[3], dra[4], dra[5]);
                }

                // using dataset function
                string selectb = "select * from selectdatat();";
                NpgsqlDataAdapter daa = new NpgsqlDataAdapter(selectb, con);
                DataSet dta = new DataSet();
                da.Fill(dt);
                foreach (DataRow dra in dta.Tables[0].Rows)
                {
                    Console.Write("{0}\t{1}\t{2}\t{3}\t{4}\t{5} \n", dra[0], dra[1], dra[2], dra[3], dra[4], dra[5]);
                }

            }
        }

        public void selectdatabyrefcursor()
        {
            DataSet ds = new DataSet();
            using (var con = GetConnection())
            {
                con.Open();
                using (NpgsqlCommand select = new NpgsqlCommand("select * from selectdata('ref0');fetch all in \"ref0\";", con))
                {
                    select.CommandType = CommandType.Text;
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(select);
                    da.Fill(ds);

                    foreach (DataRow reader in ds.Tables[1].Rows)
                    {
                        int StudentID = Convert.ToInt32(reader["StudentID"]);
                        string FirstName = Convert.ToString(reader["FirstName"]);
                        string LastName = Convert.ToString(reader["LastName"]);
                        int Age = Convert.ToInt32(reader["Age"]);
                        string Gender = Convert.ToString(reader["Gender"]);
                        double Grade = Convert.ToDouble(reader["Grade"]);

                        Console.WriteLine($"studentid:{StudentID}, firstname:{FirstName}, lastname:{LastName}, age:{Age}, gender:{Gender}, grade:{Grade},");
                    }

                }
            }
        }
        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection("server=localhost; database=demo; username=postgres; password=123456;");
        }
    }
}
