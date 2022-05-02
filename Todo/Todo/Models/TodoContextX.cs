using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Todo.Models
{
    // partial : 擴充本來的class , 命名相同 ( TodoContext : DbContext )
    //編譯時會合併一起

    //因為TodoContext在跟新資料庫後重新匯入時會自動改寫,所以不建議直接修改
    public partial class TodoContext : DbContext
    {

        public List<T> ExecSQL<T>(string query)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                Database.OpenConnection();

                List<T> list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                Database.CloseConnection();
                return list;
            }
        }

    }
}
