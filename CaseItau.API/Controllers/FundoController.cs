using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseItau.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Security.Policy;
using System.Data;

namespace CaseItau.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundoController : ControllerBase
    {
        // GET: api/Fundo
        [HttpGet]
        public IEnumerable<Fundo> Get()
        {
            var lista = new List<Fundo>();
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT F.*, T.NOME AS NOME_TIPO FROM FUNDO F INNER JOIN TIPO_FUNDO T ON T.CODIGO = F.CODIGO_TIPO";
            cmd.CommandType = System.Data.CommandType.Text;
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                var f = new Fundo();
                f.Codigo = reader[0].ToString();
                f.Nome = reader[1].ToString();
                f.Cnpj = reader[2].ToString();
                f.CodigoTipo = int.Parse(reader[3].ToString());
                f.Patrimonio = (reader[4] != null) ? decimal.Parse(reader[4].ToString()) : 0;
                f.NomeTipo = reader[5].ToString();                
                lista.Add(f);
            }
            reader.Close();
            cmd.Cancel();
            cmd.Dispose();
            con.Close();
            return lista;
        }

        // GET: api/Fundo/ITAUTESTE01
        [HttpGet("{codigo}", Name = "Get")]
        public Fundo Get(string codigo)
        {
            Fundo saida = null;
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT F.*, T.NOME AS NOME_TIPO FROM FUNDO F INNER JOIN TIPO_FUNDO T ON T.CODIGO = F.CODIGO_TIPO WHERE F.CODIGO = '" + codigo + "'";
            cmd.CommandType = System.Data.CommandType.Text;
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var f = new Fundo();
                f.Codigo = reader[0].ToString();
                f.Nome = reader[1].ToString();
                f.Cnpj = reader[2].ToString();
                f.CodigoTipo = int.Parse(reader[3].ToString());
                f.Patrimonio = decimal.Parse(reader[4].ToString());
                f.NomeTipo = reader[5].ToString();
                saida= f;
            }
            reader.Close();
            cmd.Cancel();
            cmd.Dispose();
            con.Close();
            
            return saida;
        }

        // POST: api/Fundo
        [HttpPost]
        public void Post([FromBody] Fundo value)
        {
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");
            var cmd = con.CreateCommand();
            try
            {                
                con.Open();                
                cmd.CommandText = "INSERT INTO FUNDO VALUES('" + value.Codigo + "','" + value.Nome + "','" + value.Cnpj + "'," + value.CodigoTipo.ToString() + ",0)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                throw new Exception("Erro de Base: "+e.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    cmd.Cancel();
                    cmd.Dispose();
                    con.Close();
                }
            }

        }

        // PUT: api/Fundo/ITAUTESTE01
        [HttpPut("{codigo}")]
        public void Put(string codigo, [FromBody] Fundo value)
        {            
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");
            var cmd = con.CreateCommand();
            try
            {
                con.Open();
                cmd.CommandText = "UPDATE FUNDO SET Nome = '" + value.Nome + "', CNPJ = '" + value.Cnpj + "', CODIGO_TIPO = " + value.CodigoTipo + " WHERE CODIGO = '" + codigo + "'";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw new Exception("Erro de Base: " + e.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    cmd.Cancel();
                    cmd.Dispose();
                    con.Close();
                }
            }



        }

        // DELETE: api/Fundo/ITAUTESTE01
        [HttpDelete("{codigo}")]
        public void Delete(string codigo)
        {
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");            
            var cmd = con.CreateCommand();
            try
            {
                con.Open();
                cmd.CommandText = "DELETE FROM FUNDO WHERE CODIGO = '" + codigo + "'";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception e)
            {

                throw new Exception("Erro de Base: " + e.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    cmd.Cancel();
                    cmd.Dispose();
                    con.Close();
                }
            }
            
        }

        [HttpPut("{codigo}/patrimonio")]
        public void MovimentarPatrimonio(string codigo, [FromBody] Fundo value)
        {
            
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");            
            var cmd = con.CreateCommand();
            try
            {
                double teste = double.Parse(value.Patrimonio.ToString());
                con.Open();
                cmd.CommandText = "UPDATE FUNDO SET PATRIMONIO = IFNULL(PATRIMONIO,0) + '" + teste.ToString().Replace(',', '.') + "' WHERE CODIGO = '" + codigo + "'";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw new Exception("Erro de Base: " + e.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    cmd.Cancel();
                    cmd.Dispose();
                    con.Close();
                }
            }
            
        }

        // GET: api/fundo/tipoFundo
        [HttpGet("tipoFundo", Name = "GetTipoFundo")]
        public IEnumerable<TipoFundo> GetTipoFundo()
        {

            var lista = new List<TipoFundo>();
            var con = new SQLiteConnection("Data Source=dbCaseItau.s3db");
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT T.* FROM TIPO_FUNDO T";
            cmd.CommandType = System.Data.CommandType.Text;
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var f = new TipoFundo();
                f.Codigo = int.Parse(reader[0].ToString());
                f.Nome = reader[1].ToString();
                lista.Add(f);
            }
            reader.Close();
            cmd.Cancel();
            cmd.Dispose();
            con.Close();
            return lista;
        }
        

    }
}
