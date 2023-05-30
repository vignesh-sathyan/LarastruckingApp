using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public static class SqlUtil
    {
        /// <summary>
        /// Creates a model object from data returned by the database.
        /// </summary>
        /// <typeparam name="T">Retrn type of the Generic list that is being returned.</typeparam>
        /// <param name="dr">Data rows, that will be maped to the Model Object, whose list will be returned.</param>
        /// <returns>List ot Type T.</returns>
        public static List<T> DataReaderToObjectList<T>(DbDataReader dr)
        {
            try
            {


                List<T> list = new List<T>();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance<T>();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            if (dr[prop.Name] != null)
                            {
                                try
                                {
                                    prop.SetValue(obj, dr[prop.Name]);
                                }
                                catch
                                {
                                    try
                                    {
                                        prop.SetValue(obj, null);
                                    }
                                    catch
                                    { }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            prop.SetValue(obj, null);
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Convert list to data table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)

        {
            DataTable dataTable = null; 
            try
            {



                 dataTable = new DataTable(typeof(T).Name);

                //Get all the properties

                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in Props)

                {

                    //Setting column names as Property names

                    dataTable.Columns.Add(prop.Name);

                }

                foreach (T item in items)

                {

                    var values = new object[Props.Length];

                    for (int i = 0; i < Props.Length; i++)

                    {

                        //inserting property values to datatable rows

                        values[i] = Props[i].GetValue(item, null);

                    }

                    dataTable.Rows.Add(values);

                }

                //put a breakpoint here and check datatable

                return dataTable;
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                dataTable.Dispose();
            }
        }
    }
}
