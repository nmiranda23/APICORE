using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APICORE.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace APICORE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista(){
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL)){
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using(var rd = cmd.ExecuteReader()){
                        while(rd.Read()){
                            lista.Add(new Producto(){
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        };
                    };
                }
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = lista });
            }
            catch (Exception error)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto){
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL)){
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using(var rd = cmd.ExecuteReader()){
                        while(rd.Read()){
                            lista.Add(new Producto(){
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        };
                    };
                }
                producto = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = producto });
            }
            catch (Exception error)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message, response = producto });
            }
        }

        [HttpPost]
        [Route("Crear")]
        public IActionResult Crear([FromBody] Producto objeto){

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL)){
                    conexion.Open();
                    var cmd = new SqlCommand("sp_GuardarProducto", conexion);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { message = "OK"});
            }
            catch (Exception error)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto){

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL)){
                    conexion.Open();
                    var cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", objeto.IdProducto == 0 ? DBNull.Value : objeto.IdProducto);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra is null ? DBNull.Value : objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { message = "OK"});
            }
            catch (Exception error)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto){

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL)){
                    conexion.Open();
                    var cmd = new SqlCommand("sp_EliminarProducto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { message = "OK"});
            }
            catch (Exception error)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
            }
        }
    }
}