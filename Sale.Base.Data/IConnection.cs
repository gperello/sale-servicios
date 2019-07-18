﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Common;

namespace Sale.Base.Data
{
    public interface IConnection
    {
        JArray GetArray(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text);
        List<string> GetTable(string strSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text);

        IEnumerable<TResult> GetArray<TResult>(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text) where TResult : new();

        JArray GetArrayValues(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text);
        TResult[] GetArrayValues<TResult>(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text);

        JObject GetObject(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text, DbTransaction transaction = null);
        TResult GetObject<TResult>(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text, DbTransaction transaction = null) where TResult : new();

        object GetValue(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text, Int32 paramReturn = 13, DbTransaction transaction = null);

        int Execute(string stringSql, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text, DbTransaction transaction = null);
        Dictionary<string, object> GetParams(object pParams);

        DbTransaction BeginTransaction();
        void Commit(DbTransaction transaction);
        void Rollback(DbTransaction transaction);
        void Close();
    }
}
