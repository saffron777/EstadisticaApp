using EstadisticaApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Java.Util.Jar.Attributes;

namespace EstadisticaApp.Data
{
    public class EstatisdicaRepository
    {
        string _dbPath;

        public string StatusMessage { get; set; }
        private SQLiteConnection conn;

        public EstatisdicaRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);

            conn.CreateTable<Dato>();
        }


        public void AddNewData(decimal x, decimal y)
        {
            int result = 0;
            try
            {
                Init();

                result = conn.Insert(new Dato { X = x, Y = y});
            }
            catch (Exception ex)
            {

                
            }
        }

        public Dato GetDatoById(int id) { return conn.Table<Dato>().FirstOrDefault(x => x.Id == id); }
        
        public List<Dato> GetDatos()
        {
            var result = new List<Dato>();  
            try
            {
                Init();

                result = conn.Table<Dato>().ToList();
            }
            catch (Exception ex)
            {

                
            }

            return result;
        }

        public void DeleteData(int id)
        {
            int result = 0;

            try
            {
                Init();

                
                result = conn.Delete<Dato>(id);
                
                
            }
            catch (Exception ex)
            {

                

            }


        }

        public void ClearData()
        {
            int result = 0;

            try
            {
                Init();
                
               result = conn.Table<Dato>().Delete();
                
            }
            catch (Exception ex)
            {



            }


        }

        public void UpdateData(int id, decimal x, decimal y)
        {
            int result = 0;

            try
            {
                Init();

                var dato = (from d in conn.Table<Dato>()
                           where d.Id == id
                           select d).FirstOrDefault();

                if(dato != null )
                {
                    dato.X= x;
                    dato.Y= y;
                    result = conn.Update(dato);
                }

               
            }
            catch (Exception ex)
            {

                
            }
        }
    }
}
