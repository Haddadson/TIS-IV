using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MPMG.Repositories
{
    public interface IRepositorioBase : IDisposable
    {
        IDbConnection Connection { get; }
    }

    public class RepositorioBase<TObject> : IRepositorioBase
    {
        #region "Propriedades e variáveis"

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySql"]?.ConnectionString;
        protected bool disposed = false;

        private IDbConnection _connection = null;

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        #endregion

        #region Constructor

        public RepositorioBase()
        {
            //_connection = FabricaConexao.CreateNewConnection();
        }

        #endregion

        protected List<TObject> Listar(string sql, object parametros)
        {
            return TrimStrings(this.Query<TObject>(sql, parametros)).ToList();
        }

        protected TObject Obter(string sql, object parametros)
        {
            return TrimStrings(this.Query<TObject>(sql, parametros)).FirstOrDefault();
        }

        protected IEnumerable<T> Query<T>(string sql, object parametros)
        {
            using (var cnn = new MySqlConnection(connectionString))
            {
                return Dapper.SqlMapper.Query<T>(cnn, sql, parametros, null, true, null, null);
            }
        }


        protected int Execute(string sql, object parametros)
        {
            using (var cnn = new MySqlConnection(connectionString))
            {
                return Dapper.SqlMapper.Execute(cnn, sql, parametros, null, null, null);
            }
        }

        private static IEnumerable<T> TrimStrings<T>(IEnumerable<T> objetos)
        {
            IEnumerable<PropertyInfo> propriedadesTipoString = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                                        .Where(x => x.PropertyType == typeof(string) && x.CanRead && x.CanWrite);
            foreach (PropertyInfo prop in propriedadesTipoString)
            {
                foreach (T obj in objetos)
                {
                    if (obj != null)
                    {
                        string valor = (string)prop.GetValue(obj);
                        string valorTrim = SafeTrim(valor);
                        prop.SetValue(obj, valorTrim);
                    }
                }
            }
            return objetos;
        }

        private static string SafeTrim(string original)
        {
            if (original == null)
            {
                return null;
            }
            return original.Trim();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && _connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connection.Dispose();
            }

            disposed = true;
        }
    }
}
